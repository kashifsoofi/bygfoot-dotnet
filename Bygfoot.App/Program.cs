using System.Runtime.InteropServices;
using Bygfoot.Gtk;
using Bygfoot.Store;

public class Program
{
    [DllImport("libbygfoot.so", CharSet = CharSet.Unicode)]
    private static extern int c_main(int argc, string[] argv);

    public static void Main(string[] args)
    {
        var app = new App(new FileStore());
        app.Run();
    }
}