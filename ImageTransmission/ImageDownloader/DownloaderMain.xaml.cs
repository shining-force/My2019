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

namespace ImageDownloader
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		System.Timers.Timer m_hImageDownloadTimer;
        int m_iTotalProgress;
        Queue<byte[]> m_pImgDataGrp;

        Thread m_hImgShowFunc;
        bool m_bRunning;
        bool m_bUpdate;
        bool m_bStopUpdate;
        public MainWindow()
		{
            m_iTotalProgress = 0;
            m_pImgDataGrp = new Queue<byte[]>();

            m_hImgShowFunc = new Thread(ImgShowFunc);
            m_bRunning = true;
            m_hImgShowFunc.Start();
            m_bUpdate = false;
            m_bStopUpdate = true;

            InitializeComponent();

			m_hImageDownloadTimer = new System.Timers.Timer();
			m_hImageDownloadTimer.Elapsed += new ElapsedEventHandler(Onm_hImageDownloadTimerEvent);
			m_hImageDownloadTimer.Interval = 100;
			m_hImageDownloadTimer.Start();
        }

		private void Onm_hImageDownloadTimerEvent(object source, ElapsedEventArgs e)
		{
            //String szRequestUrl = "http://127.0.0.1:8080/download";
            String szRequestUrl = "http://WebBGTest-env-1.pef5ybuuuv.ap-northeast-1.elasticbeanstalk.com/download";
            HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl);
            hRequest.Method = "GET";
            hRequest.ContentType = "application/json";
            hRequest.Timeout = 10000;

            try
			{
				hRequest.BeginGetResponse(new AsyncCallback(DownloadResponse), hRequest);
			}
			catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
		}

		public void DownloadResponse(IAsyncResult hAsyncResult)
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
                return;
			}
			StreamReader hReader = new StreamReader(hResult.GetResponseStream());
			String szResult = hReader.ReadToEnd();
			ImageTransmissionType pkg = JsonConvert.DeserializeObject<ImageTransmissionType>(szResult);
			if (pkg != null)
			{
                if (Convert.ToInt32(pkg.m_szImageProgress) < m_iTotalProgress)
                    return;
                
                m_iTotalProgress = Convert.ToInt32(pkg.m_szImageProgress);
                m_bStopUpdate = true;
                m_bUpdate = false;
                foreach (byte[] img in pkg.m_pImgStreamGrp)
                {
                    m_pImgDataGrp.Enqueue(img);
                }
                m_bStopUpdate = false;
                m_bUpdate = true;

            }
		}

        private void ImgShowFunc()
        {
            while (m_bRunning)
            {
                if (m_bUpdate && (m_pImgDataGrp.Count > 0))
                {
                    byte[] img = m_pImgDataGrp.Dequeue();

                    Dispatcher.Invoke(new Action(delegate
                    {
                        MemoryStream memoryStream = new MemoryStream(img);

                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.Default;
                        image.StreamSource = memoryStream;
                        image.EndInit();
                        m_hImageShow.Source = image;

                    }));
                    Thread.Sleep(50);                    
                }
            }

        }
    }
}
