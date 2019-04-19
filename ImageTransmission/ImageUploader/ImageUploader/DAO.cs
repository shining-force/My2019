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
        public DAO()
		{
            m_hSettings = new Settings();
            m_pImgDataGrp = new List<byte[]>();
        }

        public Settings m_hSettings;
        public List<byte[]> m_pImgDataGrp;
        public ImageTransmissionType m_hImgPkg;
    }
}
