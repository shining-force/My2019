using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploader
{
    public class ImageTransmissionType
    {
        public String m_szImageProgress;
        public List<byte[]> m_pImgStreamGrp;

        public ImageTransmissionType(List<byte[]> pDataGrp, int iProgress)
        {
            m_szImageProgress = iProgress.ToString();

            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, pDataGrp);
                objectStream.Seek(0, SeekOrigin.Begin);
                m_pImgStreamGrp = formatter.Deserialize(objectStream) as List<byte[]>;
            }
        }
    }

}
