using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsUserLogClient
{
	class UserDataDAO
	{
		public String m_szUserName;
		public String m_szPassword;
		public String m_szBaseUrl;

		public String CreateSW()
		{
			return "";
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
