using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DatabaseConsole
{
    public class ByteStreamToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage img = new BitmapImage();
            byte[] byteStream = value as byte[];
            try
            {
                img.BeginInit();
                img.StreamSource = new MemoryStream(byteStream);
                img.EndInit();
                return img;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                BitmapImage img = value as BitmapImage;
                MemoryStream stream = img.StreamSource as MemoryStream;
                return stream.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
