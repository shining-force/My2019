using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace ImageUploader
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
        System.Timers.Timer m_hImageCreatorTimer;
        int m_iImageNum;
        DAO m_hDao;
		public MainWindow()
		{
			m_hDao = new DAO();

			InitializeComponent();

			m_hImageCreatorTimer = new System.Timers.Timer();
			m_hImageCreatorTimer.Elapsed += new ElapsedEventHandler(Onm_hImageCreatorTimerEvent);
			m_hImageCreatorTimer.Interval = 50;
			m_hImageCreatorTimer.Start();

            m_iImageNum = 0;
        }
        private ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private void Onm_hImageCreatorTimerEvent(object source, ElapsedEventArgs e)
		{
			int iScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			int iScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            Bitmap bitmap = new Bitmap(iScreenWidth, iScreenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
            {
                memoryGrahics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(iScreenWidth, iScreenHeight), CopyPixelOperation.SourceCopy);
            }
            MemoryStream stream = new MemoryStream();
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 0L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bitmap.Save(stream, GetEncoderInfo("image/jpeg"), myEncoderParameters);

            //Dispatcher.Invoke(new Action(delegate
            //{
            //    BitmapImage image = new BitmapImage();
            //    image.BeginInit();
            //    image.CacheOption = BitmapCacheOption.OnLoad;
            //    image.StreamSource = stream;
            //    image.EndInit();
            //    m_hImageShow.Source = image;
            //}));

            m_hDao.m_pImgDataGrp.Add(stream.ToArray());
            if (m_hDao.m_pImgDataGrp.Count >= 20)
            {
				//compression

                m_hDao.m_hImgPkg = new ImageTransmissionType(m_hDao.m_pImgDataGrp, m_iImageNum);
                m_hDao.m_pImgDataGrp.Clear();

                Thread imgUpload = new Thread(ImgUploadFunc);
                imgUpload.Start();
            }
        }

        private void ImgUploadFunc()
        {


			//String szRequestUrl = "http://127.0.0.1:8080/upload";
			String szRequestUrl = "http://WebBGTest-env-1.pef5ybuuuv.ap-northeast-1.elasticbeanstalk.com/upload";

			HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl);
			hRequest.Method = "POST";
			hRequest.ContentType = "application/json";
			hRequest.Timeout = 1000;
				
			try
			{
                hRequest.BeginGetRequestStream(RequestSteamResponse, hRequest);
			}
			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }			
		}

        public void RequestSteamResponse(IAsyncResult hAsyncResult)
        {
            HttpWebRequest hRequest = hAsyncResult.AsyncState as HttpWebRequest;


            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            string json = JsonConvert.SerializeObject(m_hDao.m_hImgPkg, jsSettings);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            try
            {
                using (Stream hPostStream = hRequest.EndGetRequestStream(hAsyncResult))
                {
                    hPostStream.Write(bytes, 0, bytes.Length);
                    hPostStream.Close();
                }
                hRequest.BeginGetResponse(UploaderResponse, hRequest);
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }

        }

        public void UploaderResponse(IAsyncResult hAsyncResult)
        {
            HttpWebRequest hRequest = hAsyncResult.AsyncState as HttpWebRequest;
            HttpWebResponse hResult;
            int iProgress;
            try
            {
                hResult = hRequest.EndGetResponse(hAsyncResult) as HttpWebResponse;
                StreamReader hReader = new StreamReader(hResult.GetResponseStream());
                String szResult = hReader.ReadToEnd();
                iProgress = JsonConvert.DeserializeObject<int>(szResult);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return;
            }
            Dispatcher.Invoke(new Action(delegate
            {
                m_hUploadProgress.Text = iProgress.ToString();
            }));
			//next number
			m_iImageNum++;
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_hImageCreatorTimer.Stop();
        }
    }
}
