using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Generic.Assy2bin
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
