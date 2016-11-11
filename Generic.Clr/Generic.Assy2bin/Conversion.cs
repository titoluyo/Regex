using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Generic.Assy2bin
{
    public class Conversion
    {
        public static string GetHexString(string assemblyPath)
        {
            if (!Path.IsPathRooted(assemblyPath))
                assemblyPath = Path.Combine(Environment.CurrentDirectory, assemblyPath);

            var builder = new StringBuilder();
            builder.Append("0x");

            using (var stream = new FileStream(assemblyPath,
                  FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var currentByte = stream.ReadByte();
                while (currentByte > -1)
                {
                    builder.Append(currentByte.ToString("X2", CultureInfo.InvariantCulture));
                    currentByte = stream.ReadByte();
                }
            }

            return builder.ToString();
        }

        public static string GetDdl(string assemblyPath)
        {
            var assy = Assembly.LoadFrom(assemblyPath);
            var hexString = GetHexString(assemblyPath);
            var sql = "CREATE ASSEMBLY [" + assy.GetName().Name + "] FROM " + hexString +
                         " WITH PERMISSION_SET = SAFE";
            return sql;
        }
    }
}
