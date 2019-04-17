using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        System.Timers.Timer m_hImageUploaderTimer;
        DAO m_hDao;
		public MainWindow()
		{
			m_hDao = new DAO();

			InitializeComponent();

			m_hImageCreatorTimer = new System.Timers.Timer();
			m_hImageCreatorTimer.Elapsed += new ElapsedEventHandler(Onm_hImageCreatorTimerEvent);
			m_hImageCreatorTimer.Interval = 20;
			m_hImageCreatorTimer.Start();

            m_hImageUploaderTimer = new System.Timers.Timer();
            m_hImageUploaderTimer.Elapsed += new ElapsedEventHandler(Onm_hImageUploaderTimerEvent);
            m_hImageUploaderTimer.Interval = 20;
            m_hImageUploaderTimer.Start();
        }

		private void Onm_hImageCreatorTimerEvent(object source, ElapsedEventArgs e)
		{
            RenderTargetBitmap bmp = new RenderTargetBitmap(240, 100, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 100, 200, 150));
            drawingContext.DrawText(new FormattedText(m_hDao.m_hImageData.m_iImageProgress.ToString(), Thread.CurrentThread.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 32, brush), new Point(0, 0));

            drawingContext.Close();
            bmp.Render(drawingVisual);

            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            MemoryStream stream = new MemoryStream();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bmp));
            bitmapEncoder.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            m_hDao.m_hImageData.m_pImageStream = stream.ToArray();

            Dispatcher.Invoke(new Action(delegate
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                m_hImageShow.Source = image;
            }));

			//next number
			m_hDao.m_hImageData.m_iImageProgress++;
            if (m_hDao.m_hImageData.m_iImageProgress > 10000)
                m_hDao.m_hImageData.m_iImageProgress = 0;
        }

        private void Onm_hImageUploaderTimerEvent(object source, ElapsedEventArgs e)
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

            ImageTransmissionType pkg = new ImageTransmissionType();
            pkg.imageProgress = m_hDao.m_hImageData.m_iImageProgress;
            pkg.imageStream = m_hDao.m_hImageData.m_pImageStream;

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            string json = JsonConvert.SerializeObject(pkg, jsSettings);
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
            try
            {
                hResult = hRequest.EndGetResponse(hAsyncResult) as HttpWebResponse;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
