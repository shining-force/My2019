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
				m_hImage = new BitmapImage();
				m_hImageStream = new MemoryStream();
			}
			public BitmapImage m_hImage;
			public MemoryStream m_hImageStream;
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
