using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConsole
{
    public class StdMd5Maker
    {
        public static String toMd5String(string src)
        {
            if (src == null)
                return "";
            MD5 hMd5 = new MD5CryptoServiceProvider();
            byte[] pOutput = hMd5.ComputeHash(Encoding.Default.GetBytes(src));
            char[] szRaw = Convert.ToBase64String(pOutput).ToArray();

            return new string(szRaw);
        }
    }
}
