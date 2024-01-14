using System.Runtime.InteropServices;

public class Program
{
    [DllImport("libbygfoot.so", CharSet = CharSet.Unicode)]
    private static extern int c_main(int argc, string[] argv);

    public static void Main(string[] args)
    {
        c_main(args.Length, args);
    }
}