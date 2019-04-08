using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageUploader
{
	class DAO
	{
		public class Settings
		{

		}

		public class ImageData
		{
			public ImageData()
			{
                m_iImageProgress = 0;
                m_pImageStream = null;
            }
            public int m_iImageProgress;
			public byte[] m_pImageStream;
		}

		public DAO()
		{
			m_hImageData = new ImageData();
			m_hSettings = new Settings();
		}

		public Settings m_hSettings;
		public ImageData m_hImageData;
	}
}
