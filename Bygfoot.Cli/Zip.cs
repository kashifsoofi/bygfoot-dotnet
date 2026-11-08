using System.IO.Compression;

namespace Bygfoot.Cli;

/// <summary>
/// Date/time information for a zip entry. Corresponds to C struct tm_zip in src/zip/zip.h.
/// </summary>
public class TmZip
{
    /// <summary>Seconds after the minute - [0,59]. Corresponds to C: uInt tm_sec.</summary>
    public uint TmSec { get; set; }
    /// <summary>Minutes after the hour - [0,59]. Corresponds to C: uInt tm_min.</summary>
    public uint TmMin { get; set; }
    /// <summary>Hours since midnight - [0,23]. Corresponds to C: uInt tm_hour.</summary>
    public uint TmHour { get; set; }
    /// <summary>Day of the month - [1,31]. Corresponds to C: uInt tm_mday.</summary>
    public uint TmMday { get; set; }
    /// <summary>Months since January - [0,11]. Corresponds to C: uInt tm_mon.</summary>
    public uint TmMon { get; set; }
    /// <summary>Years - [1980..2044]. Corresponds to C: uInt tm_year.</summary>
    public uint TmYear { get; set; }
}

/// <summary>
/// Supplemental information for a zip entry. Corresponds to C struct zip_fileinfo in src/zip/zip.h.
/// </summary>
public class ZipFileinfo
{
    /// <summary>Date in understandable format. Corresponds to C: tm_zip tmz_date.</summary>
    public TmZip TmzDate { get; set; } = new TmZip();
    /// <summary>If dosDate is 0, TmzDate is used. Corresponds to C: uLong dosDate.</summary>
    public uint DosDate { get; set; }
    /// <summary>Internal file attributes. Corresponds to C: uLong internal_fa.</summary>
    public uint InternalFa { get; set; }
    /// <summary>External file attributes. Corresponds to C: uLong external_fa.</summary>
    public uint ExternalFa { get; set; }
}

/// <summary>
/// Zip file writer. Corresponds to the C typedef zipFile and the C functions in
/// src/zip/zip.h and src/zip/zip.c. The storage is implemented with
/// System.IO.Compression instead of zlib.
/// </summary>
public class ZipFile
{
    /// <summary>Corresponds to C: ZIP_OK.</summary>
    public const int ZipOk = 0;
    /// <summary>Corresponds to C: ZIP_ERRNO (Z_ERRNO).</summary>
    public const int ZipErrno = -10001;
    /// <summary>Corresponds to C: ZIP_PARAMERROR.</summary>
    public const int ZipParamerror = -102;
    /// <summary>Corresponds to C: ZIP_INTERNALERROR.</summary>
    public const int ZipInternalerror = -104;

    /// <summary>Compression method: deflate. Corresponds to C: Z_DEFLATED.</summary>
    public const int ZDeflated = 8;
    /// <summary>Default compression level. Corresponds to C: Z_DEFAULT_COMPRESSION.</summary>
    public const int ZDefaultCompression = -1;

    internal Stream? Stream;
    internal ZipArchive? Archive;
    internal ZipArchiveEntry? CurrentEntry;

    /// <summary>
    /// Create a zipfile.
    /// Corresponds to C: zipOpen(const char *pathname, int append).
    /// </summary>
    /// <param name="pathname">The file name of the zip file.</param>
    /// <param name="append">If the file exists and append is 1, the zip is created at the end of the file.</param>
    /// <returns>A zipFile handle, or null if the zip file could not be opened.</returns>
    public static ZipFile? ZipOpen(string pathname, int append)
    {
        try
        {
            FileMode mode;
            if (File.Exists(pathname))
                mode = append != 0 ? FileMode.Open : FileMode.Truncate;
            else
                mode = FileMode.Create;

            var stream = new FileStream(pathname, mode, FileAccess.ReadWrite, FileShare.None);
            var archive = new ZipArchive(stream, ZipArchiveMode.Update, false);
            return new ZipFile { Stream = stream, Archive = archive };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Open a file in the ZIP for writing.
    /// Corresponds to C: zipOpenNewFileInZip(zipFile file, const char* filename, const zip_fileinfo* zipfi,
    /// const void* extrafield_local, uInt size_extrafield_local, const void* extrafield_global,
    /// uInt size_extrafield_global, const char* comment, int method, int level).
    /// </summary>
    /// <param name="file">The zipFile handle.</param>
    /// <param name="filename">The file name in the zip (if null, "-" will be used).</param>
    /// <param name="zipfi">Supplemental information (the date is used, if given).</param>
    /// <param name="extrafieldLocal">Not supported by System.IO.Compression.</param>
    /// <param name="sizeExtrafieldLocal">Not supported by System.IO.Compression.</param>
    /// <param name="extrafieldGlobal">Not supported by System.IO.Compression.</param>
    /// <param name="sizeExtrafieldGlobal">Not supported by System.IO.Compression.</param>
    /// <param name="comment">Not supported by System.IO.Compression.</param>
    /// <param name="method">The compression method (0 for store, ZDeflated for deflate).</param>
    /// <param name="level">The level of compression (can be ZDefaultCompression).</param>
    /// <returns>ZipOk if there is no problem.</returns>
    public static int ZipOpenNewFileInZip(ZipFile file, string? filename, ZipFileinfo? zipfi,
        byte[]? extrafieldLocal, int sizeExtrafieldLocal,
        byte[]? extrafieldGlobal, int sizeExtrafieldGlobal,
        string? comment, int method, int level)
    {
        if (file?.Archive == null)
            return ZipErrno;

        if (string.IsNullOrEmpty(filename))
            filename = "-";

        CompressionLevel compression = method == ZDeflated
            ? CompressionLevel.Optimal
            : CompressionLevel.NoCompression;

        var entry = file.Archive.CreateEntry(filename, compression);

        // Set the modification date from the supplemental information, if given.
        if (zipfi != null)
        {
            if (zipfi.DosDate != 0)
                entry.LastWriteTime = DateTimeFromDosDate(zipfi.DosDate);
            else
                entry.LastWriteTime = new DateTimeOffset(
                    new DateTime(
                        (int)zipfi.TmzDate.TmYear,
                        (int)zipfi.TmzDate.TmMon + 1,
                        (int)zipfi.TmzDate.TmMday,
                        (int)zipfi.TmzDate.TmHour,
                        (int)zipfi.TmzDate.TmMin,
                        (int)zipfi.TmzDate.TmSec),
                    TimeSpan.Zero);
        }

        file.CurrentEntry = entry;
        return ZipOk;
    }

    /// <summary>
    /// Write data in the zipfile.
    /// Corresponds to C: zipWriteInFileInZip(zipFile file, const voidp buf, unsigned len).
    /// </summary>
    /// <param name="file">The zipFile handle.</param>
    /// <param name="buffer">The buffer containing the data.</param>
    /// <param name="len">The number of bytes to write.</param>
    /// <returns>The number of bytes written, or an error code &lt; 0.</returns>
    public static int ZipWriteInFileInZip(ZipFile file, byte[] buffer, int len)
    {
        if (file?.CurrentEntry == null)
            return ZipParamerror;

        using Stream entryStream = file.CurrentEntry.Open();
        entryStream.Write(buffer, 0, len);
        return len;
    }

    /// <summary>
    /// Close the current file in the zipfile.
    /// Corresponds to C: zipCloseFileInZip(zipFile file).
    /// </summary>
    /// <param name="file">The zipFile handle.</param>
    /// <returns>ZipOk if there is no problem.</returns>
    public static int ZipCloseFileInZip(ZipFile file)
    {
        if (file?.Archive == null)
            return ZipErrno;

        // The entry stream is closed by ZipWriteInFileInZip; the entry metadata
        // is written when the archive is closed by ZipClose.
        file.CurrentEntry = null;
        return ZipOk;
    }

    /// <summary>
    /// Close the zipfile.
    /// Corresponds to C: zipClose(zipFile file, const char* global_comment).
    /// </summary>
    /// <param name="file">The zipFile handle.</param>
    /// <param name="globalComment">Not supported by System.IO.Compression.</param>
    /// <returns>ZipOk if there is no problem.</returns>
    public static int ZipClose(ZipFile file, string? globalComment)
    {
        if (file?.Archive == null)
            return ZipErrno;

        file.Archive.Dispose();
        file.Archive = null;
        file.Stream = null;
        return ZipOk;
    }

    /// <summary>
    /// Convert a DOS date/time value to a DateTime.
    /// Corresponds to the dos_date handling of zip.c.
    /// </summary>
    /// <param name="dosDate">The DOS date/time value.</param>
    /// <returns>The corresponding DateTime.</returns>
    internal static DateTime DateTimeFromDosDate(uint dosDate)
    {
        return new DateTime(
            1980 + (int)(dosDate >> 25 & 0x7f),
            (int)(dosDate >> 21 & 0x0f),
            (int)(dosDate >> 16 & 0x1f),
            (int)(dosDate >> 11 & 0x1f),
            (int)(dosDate >> 5 & 0x3f),
            (int)(dosDate & 0x1f) * 2);
    }

    /// <summary>
    /// Convert a DateTime to a DOS date/time value.
    /// Corresponds to the dos_date handling of zip.c.
    /// </summary>
    /// <param name="dateTime">The DateTime.</param>
    /// <returns>The corresponding DOS date/time value.</returns>
    internal static uint DosDateFromDateTime(DateTime dateTime)
    {
        return ((uint)(dateTime.Year - 1980) << 25) |
               ((uint)dateTime.Month << 21) |
               ((uint)dateTime.Day << 16) |
               ((uint)dateTime.Hour << 11) |
               ((uint)dateTime.Minute << 5) |
               ((uint)(dateTime.Second / 2));
    }
}
