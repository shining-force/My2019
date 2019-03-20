using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AwsUserLogClient
{
	public class UserDataDAO
	{
		public static String m_szUserName;
		public static String m_szPassword;
		public static String m_szBaseUrl;

        public static List<AwsLog_Transmit> m_pUseLogList;

        public static String m_szLogTitleOnWriting;
        public static String m_szLogDetailOnWriting;
		public static String CreateSW()
        {
            MD5 hMd5 = new MD5CryptoServiceProvider();
            byte[] pOutput = hMd5.ComputeHash(Encoding.Default.GetBytes(m_szUserName + m_szPassword));
            char[] szRaw = Convert.ToBase64String(pOutput).ToArray();
            for (int iIndex = 0; iIndex < szRaw.LongLength; ++iIndex)
            {
                if (!isCharacter(szRaw[iIndex]))
                {
                    szRaw[iIndex] = 'X';
                }
            }
            return new string(szRaw);
		}
        public static Boolean isCharacter(char c)
        {
            return (((c <= 'z') && (c >= 'a')) || ((c <= 'Z') && (c >= 'A')));
        }
    }

    public class AwsLog_Transmit
    {
        public String mFormatDate { get; set; }
        public String mFormat { get; set; }
        public String mTitle { get; set; }
        public String mDetail { get; set; }
        public String mFromUser { get; set; }
        public String mSW { get; set; }
    }
}
