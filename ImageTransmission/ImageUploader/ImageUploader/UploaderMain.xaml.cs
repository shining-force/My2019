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
        System.Timers.Timer m_hImageUpdateTimer;
		DAO m_hDao;
		
		public MainWindow()
		{
			m_hDao = new DAO();

			InitializeComponent();

			m_hImageUpdateTimer = new System.Timers.Timer();
			m_hImageUpdateTimer.Elapsed += new ElapsedEventHandler(Onm_hImageUpdateTimerEvent);
			m_hImageUpdateTimer.Interval = 10;
			m_hImageUpdateTimer.Start();
		}

		private void Onm_hImageUpdateTimerEvent(object source, ElapsedEventArgs e)
		{
            RenderTargetBitmap bmp = new RenderTargetBitmap(240, 100, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
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

            //upload
            String szRequestUrl = "http://127.0.0.1:8080/upload";
            HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl);
            hRequest.Method = "POST";
            hRequest.ContentType = "application/json";
            hRequest.Timeout = 10000;

            ImageTransmissionType pkg = new ImageTransmissionType();
            pkg.imageProgress = m_hDao.m_hImageData.m_iImageProgress;
            pkg.imageStream = m_hDao.m_hImageData.m_pImageStream;

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(pkg, jsSettings);

			try
			{
				Stream req = hRequest.GetRequestStream();
				byte[] bytes = Encoding.UTF8.GetBytes(json);
				req.Write(bytes, 0, bytes.Length);

				hRequest.BeginGetResponse(new AsyncCallback(LogWriteResponse), hRequest);
			}
			catch(Exception ex) { }


            //next number
            m_hDao.m_hImageData.m_iImageProgress++;
            if (m_hDao.m_hImageData.m_iImageProgress > 10000)
                m_hDao.m_hImageData.m_iImageProgress = 0;
        }
        public void LogWriteResponse(IAsyncResult hAsyncResult)
        {
            HttpWebRequest hRequest = hAsyncResult.AsyncState as HttpWebRequest;
            HttpWebResponse hResult;
            try
            {
                hResult = hRequest.EndGetResponse(hAsyncResult) as HttpWebResponse;
            }
            catch (Exception e)
            {

            }    
        }
    }
}
