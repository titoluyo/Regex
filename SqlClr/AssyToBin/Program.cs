using System;
using System.Windows.Forms;

namespace tlm.AssyToBin
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var result = Conversion.GetDdl(args[0]);
            Clipboard.SetText(result);
            //Console.WriteLine(result);
        }
    }
}
