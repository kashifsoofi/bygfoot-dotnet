using System.IO.Compression;

namespace Bygfoot.Cli;

/// <summary>
/// Date/time information for a zip entry. Corresponds to C struct tm_unz in src/zip/unzip.h.
/// </summary>
public class TmUnz
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
/// Global data about a zip file, from the end of central directory.
/// Corresponds to C struct unz_global_info in src/zip/unzip.h.
/// </summary>
public class UnzGlobalInfo
{
    /// <summary>Total number of entries in the central directory on this disk. Corresponds to C: uLong number_entry.</summary>
    public uint NumberEntry { get; set; }
    /// <summary>Size of the global comment of the zip file. Corresponds to C: uLong size_comment.</summary>
    public uint SizeComment { get; set; }
}

/// <summary>
/// Information about a file in the zip file. Corresponds to C struct unz_file_info in src/zip/unzip.h.
/// </summary>
public class UnzFileInfo
{
    /// <summary>Version made by. Corresponds to C: uLong version.</summary>
    public uint Version { get; set; }
    /// <summary>Version needed to extract. Corresponds to C: uLong version_needed.</summary>
    public uint VersionNeeded { get; set; }
    /// <summary>General purpose bit flag. Corresponds to C: uLong flag.</summary>
    public uint Flag { get; set; }
    /// <summary>Compression method. Corresponds to C: uLong compression_method.</summary>
    public uint CompressionMethod { get; set; }
    /// <summary>Last modification file date in DOS format. Corresponds to C: uLong dosDate.</summary>
    public uint DosDate { get; set; }
    /// <summary>CRC-32. Corresponds to C: uLong crc.</summary>
    public uint Crc { get; set; }
    /// <summary>Compressed size. Corresponds to C: uLong compressed_size.</summary>
    public uint CompressedSize { get; set; }
    /// <summary>Uncompressed size. Corresponds to C: uLong uncompressed_size.</summary>
    public uint UncompressedSize { get; set; }
    /// <summary>File name length. Corresponds to C: uLong size_filename.</summary>
    public uint SizeFilename { get; set; }
    /// <summary>Extra field length. Corresponds to C: uLong size_file_extra.</summary>
    public uint SizeFileExtra { get; set; }
    /// <summary>File comment length. Corresponds to C: uLong size_file_comment.</summary>
    public uint SizeFileComment { get; set; }
    /// <summary>Disk number start. Corresponds to C: uLong disk_num_start.</summary>
    public uint DiskNumStart { get; set; }
    /// <summary>Internal file attributes. Corresponds to C: uLong internal_fa.</summary>
    public uint InternalFa { get; set; }
    /// <summary>External file attributes. Corresponds to C: uLong external_fa.</summary>
    public uint ExternalFa { get; set; }
    /// <summary>Date in understandable format. Corresponds to C: tm_unz tmu_date.</summary>
    public TmUnz TmuDate { get; set; } = new TmUnz();
}

/// <summary>
/// Zip file reader. Corresponds to the C typedef unzFile and the C functions in
/// src/zip/unzip.h and src/zip/unzip.c. The storage is implemented with
/// System.IO.Compression instead of zlib.
/// </summary>
public class UnzFile
{
    /// <summary>Corresponds to C: UNZ_OK.</summary>
    public const int UnzOk = 0;
    /// <summary>Corresponds to C: UNZ_END_OF_LIST_OF_FILE.</summary>
    public const int UnzEndOfListOfFile = -100;
    /// <summary>Corresponds to C: UNZ_ERRNO (Z_ERRNO).</summary>
    public const int UnzErrno = -10001;
    /// <summary>Corresponds to C: UNZ_EOF.</summary>
    public const int UnzEof = 0;
    /// <summary>Corresponds to C: UNZ_PARAMERROR.</summary>
    public const int UnzParamerror = -102;
    /// <summary>Corresponds to C: UNZ_BADZIPFILE.</summary>
    public const int UnzBadzipfile = -103;
    /// <summary>Corresponds to C: UNZ_INTERNALERROR.</summary>
    public const int UnzInternalerror = -104;
    /// <summary>Corresponds to C: UNZ_CRCERROR.</summary>
    public const int UnzCrcerror = -105;

    internal Stream? Stream;
    internal ZipArchive? Archive;
    internal ZipArchiveEntry? CurrentEntry;
    internal Stream? CurrentEntryStream;
    internal int CurrentIndex = -1;

    /// <summary>
    /// Compare two file names.
    /// Corresponds to C: unzStringFileNameCompare(const char* fileName1, const char* fileName2, int iCaseSensitivity).
    /// </summary>
    /// <param name="fileName1">The first file name.</param>
    /// <param name="fileName2">The second file name.</param>
    /// <param name="iCaseSensitivity">1: case sensitive, 2: case insensitive, 0: default of the operating system (1 on Unix, 2 on Windows).</param>
    /// <returns>0 if the names are equal, &lt; 0 or &gt; 0 otherwise.</returns>
    public static int UnzStringFileNameCompare(string fileName1, string fileName2, int iCaseSensitivity)
    {
        StringComparison comparison = iCaseSensitivity switch
        {
            1 => StringComparison.Ordinal,
            2 => StringComparison.OrdinalIgnoreCase,
            _ => Variables.OsIsUnix ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase
        };
        return string.Compare(fileName1, fileName2, comparison);
    }

    /// <summary>
    /// Open a zip file.
    /// Corresponds to C: unzOpen(const char *path).
    /// </summary>
    /// <param name="path">The full path name of the zip file.</param>
    /// <returns>An unzFile handle, or null if the zip file cannot be opened.</returns>
    public static UnzFile? UnzOpen(string path)
    {
        try
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var archive = new ZipArchive(stream, ZipArchiveMode.Read, false);
            return new UnzFile { Stream = stream, Archive = archive };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Close a zip file opened with UnzOpen.
    /// Corresponds to C: unzClose(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>UnzOk if there is no problem.</returns>
    public static int UnzClose(UnzFile file)
    {
        if (file?.Archive == null)
            return UnzErrno;

        if (file.CurrentEntryStream != null)
        {
            file.CurrentEntryStream.Dispose();
            file.CurrentEntryStream = null;
            file.CurrentEntry = null;
        }

        file.Archive.Dispose();
        file.Archive = null;
        file.Stream = null;
        return UnzOk;
    }

    /// <summary>
    /// Get info about the zip file.
    /// Corresponds to C: unzGetGlobalInfo(unzFile file, unz_global_info *pglobal_info).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <param name="globalInfo">The global info of the zip file.</param>
    /// <returns>UnzOk if there is no problem.</returns>
    public static int UnzGetGlobalInfo(UnzFile file, out UnzGlobalInfo globalInfo)
    {
        globalInfo = new UnzGlobalInfo
        {
            NumberEntry = (uint)(file?.Archive?.Entries.Count ?? 0)
        };
        // The global comment is not exposed by System.IO.Compression.
        globalInfo.SizeComment = 0;

        return file?.Archive != null ? UnzOk : UnzErrno;
    }

    /// <summary>
    /// Get the global comment string of the zip file.
    /// Corresponds to C: unzGetGlobalComment(unzFile file, char *szComment, uLong uSizeBuf).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <param name="comment">The global comment (empty, not exposed by System.IO.Compression).</param>
    /// <returns>The number of bytes copied, or an error code &lt; 0.</returns>
    public static int UnzGetGlobalComment(UnzFile file, out string comment)
    {
        // The global comment is not exposed by System.IO.Compression.
        comment = "";

        return file?.Archive != null ? 0 : UnzErrno;
    }

    /// <summary>
    /// Set the current file of the zip file to the first file.
    /// Corresponds to C: unzGoToFirstFile(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>UnzOk if there is no problem, UnzEndOfListOfFile if the zip file is empty.</returns>
    public static int UnzGoToFirstFile(UnzFile file)
    {
        if (file?.Archive == null || file.Archive.Entries.Count == 0)
            return UnzEndOfListOfFile;

        file.CurrentIndex = 0;
        return UnzOk;
    }

    /// <summary>
    /// Set the current file of the zip file to the next file.
    /// Corresponds to C: unzGoToNextFile(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>UnzOk if there is no problem, UnzEndOfListOfFile if the actual file was the latest.</returns>
    public static int UnzGoToNextFile(UnzFile file)
    {
        if (file?.Archive == null)
            return UnzErrno;

        if (file.CurrentIndex >= file.Archive.Entries.Count - 1)
            return UnzEndOfListOfFile;

        file.CurrentIndex++;
        return UnzOk;
    }

    /// <summary>
    /// Try to locate a file in the zip file.
    /// Corresponds to C: unzLocateFile(unzFile file, const char *szFileName, int iCaseSensitivity).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <param name="szFileName">The file name to look for.</param>
    /// <param name="iCaseSensitivity">See UnzStringFileNameCompare.</param>
    /// <returns>UnzOk if the file is found (it becomes the current file), UnzEndOfListOfFile if it is not found.</returns>
    public static int UnzLocateFile(UnzFile file, string szFileName, int iCaseSensitivity)
    {
        if (file?.Archive == null)
            return UnzErrno;

        for (int i = 0; i < file.Archive.Entries.Count; i++)
        {
            if (UnzStringFileNameCompare(file.Archive.Entries[i].FullName, szFileName, iCaseSensitivity) == 0)
            {
                file.CurrentIndex = i;
                return UnzOk;
            }
        }

        return UnzEndOfListOfFile;
    }

    /// <summary>
    /// Get info about the current file.
    /// Corresponds to C: unzGetCurrentFileInfo(unzFile file, unz_file_info *pfile_info, char *szFileName,
    /// uLong fileNameBufferSize, void *extraField, uLong extraFieldBufferSize, char *szComment, uLong commentBufferSize).
    /// The extra field and comment buffers of the C API are omitted because they are not
    /// exposed by System.IO.Compression; the file name is returned as an out parameter.
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <param name="fileInfo">The info about the current file.</param>
    /// <param name="fileName">The file name of the current file in the zip.</param>
    /// <returns>UnzOk if there is no problem.</returns>
    public static int UnzGetCurrentFileInfo(UnzFile file, out UnzFileInfo fileInfo, out string fileName)
    {
        fileInfo = new UnzFileInfo();
        fileName = "";

        if (file?.Archive == null ||
            file.CurrentIndex < 0 || file.CurrentIndex >= file.Archive.Entries.Count)
            return UnzErrno;

        ZipArchiveEntry entry = file.Archive.Entries[file.CurrentIndex];
        DateTime dateTime = entry.LastWriteTime.DateTime;

        // System.IO.Compression does not expose the compression method of an entry;
        // assume stored when the compressed and uncompressed sizes are equal, otherwise deflate.
        fileInfo.CompressionMethod = entry.CompressedLength == entry.Length ? 0 : (uint)ZipFile.ZDeflated;
        fileInfo.DosDate = ZipFile.DosDateFromDateTime(dateTime);
        fileInfo.CompressedSize = (uint)entry.CompressedLength;
        fileInfo.UncompressedSize = (uint)entry.Length;
        fileInfo.SizeFilename = (uint)entry.FullName.Length;
        // Version, version_needed, flag, crc, extra field, comment, disk_num_start
        // and the file attributes are not exposed by System.IO.Compression.
        fileInfo.TmuDate = new TmUnz
        {
            TmSec = (uint)dateTime.Second,
            TmMin = (uint)dateTime.Minute,
            TmHour = (uint)dateTime.Hour,
            TmMday = (uint)dateTime.Day,
            TmMon = (uint)(dateTime.Month - 1),
            TmYear = (uint)dateTime.Year
        };

        fileName = entry.FullName;
        return UnzOk;
    }

    /// <summary>
    /// Open for reading data the current file in the zip file.
    /// Corresponds to C: unzOpenCurrentFile(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>UnzOk if there is no error.</returns>
    public static int UnzOpenCurrentFile(UnzFile file)
    {
        if (file?.Archive == null ||
            file.CurrentIndex < 0 || file.CurrentIndex >= file.Archive.Entries.Count)
            return UnzErrno;

        file.CurrentEntry = file.Archive.Entries[file.CurrentIndex];
        file.CurrentEntryStream = file.CurrentEntry.Open();
        return UnzOk;
    }

    /// <summary>
    /// Close the file in the zip opened with UnzOpenCurrentFile.
    /// Corresponds to C: unzCloseCurrentFile(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>UnzOk if there is no problem.</returns>
    public static int UnzCloseCurrentFile(UnzFile file)
    {
        if (file?.CurrentEntryStream == null)
            return UnzErrno;

        // System.IO.Compression does not verify the CRC of the extracted data.
        file.CurrentEntryStream.Dispose();
        file.CurrentEntryStream = null;
        file.CurrentEntry = null;
        return UnzOk;
    }

    /// <summary>
    /// Read bytes from the current file (opened by UnzOpenCurrentFile).
    /// Corresponds to C: unzReadCurrentFile(unzFile file, voidp buf, unsigned len).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <param name="buffer">The buffer where the data is copied.</param>
    /// <param name="len">The size of the buffer.</param>
    /// <returns>The number of bytes copied, 0 if the end of file was reached, or an error code &lt; 0.</returns>
    public static int UnzReadCurrentFile(UnzFile file, byte[] buffer, int len)
    {
        if (file?.CurrentEntryStream == null)
            return UnzErrno;

        try
        {
            return file.CurrentEntryStream.Read(buffer, 0, len);
        }
        catch
        {
            return UnzErrno;
        }
    }

    /// <summary>
    /// Give the current position in uncompressed data.
    /// Corresponds to C: unztell(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>The current position in uncompressed data.</returns>
    public static long Unztell(UnzFile file)
    {
        return file?.CurrentEntryStream?.Position ?? 0;
    }

    /// <summary>
    /// Check whether the end of the current file was reached.
    /// Corresponds to C: unzeof(unzFile file).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <returns>1 if the end of file was reached, 0 elsewhere.</returns>
    public static int Unzeof(UnzFile file)
    {
        if (file?.CurrentEntryStream == null)
            return 1;

        long position = file.CurrentEntryStream.Position;
        int read = file.CurrentEntryStream.ReadByte();
        if (read > 0)
            file.CurrentEntryStream.Seek(position, SeekOrigin.Begin);

        return read == 0 ? 1 : 0;
    }

    /// <summary>
    /// Read the extra field from the current file.
    /// Corresponds to C: unzGetLocalExtrafield(unzFile file, voidp buf, unsigned len).
    /// </summary>
    /// <param name="file">The unzFile handle.</param>
    /// <param name="buffer">The buffer (unused, the local extra field is not exposed by System.IO.Compression).</param>
    /// <param name="len">The size of the buffer.</param>
    /// <returns>The number of bytes copied (always 0), or an error code &lt; 0.</returns>
    public static int UnzGetLocalExtrafield(UnzFile file, byte[]? buffer, int len)
    {
        // The local extra field is not exposed by System.IO.Compression.
        return 0;
    }
}
