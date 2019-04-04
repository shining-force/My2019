using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
		public MainWindow()
		{
			InitializeComponent();
			m_hImageDownloadTimer = new System.Timers.Timer();
			m_hImageDownloadTimer.Elapsed += new ElapsedEventHandler(Onm_hImageDownloadTimerEvent);
			m_hImageDownloadTimer.Interval = 10;
			m_hImageDownloadTimer.Start();
		}

		private void Onm_hImageDownloadTimerEvent(object source, ElapsedEventArgs e)
		{
			String szRequestUrl = "http://127.0.0.1:8080/download";
			HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl);
			hRequest.Method = "GET";
			hRequest.ContentType = "application/json";
			hRequest.Timeout = 10000;

			try
			{
				hRequest.BeginGetResponse(new AsyncCallback(DownloadResponse), hRequest);
			}
			catch(Exception ex) { }
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
				return;
			}
			StreamReader hReader = new StreamReader(hResult.GetResponseStream());
			String szResult = hReader.ReadToEnd();
			ImageTransmissionType pkg = JsonConvert.DeserializeObject<ImageTransmissionType>(szResult);


			Dispatcher.Invoke(new Action(delegate
			{
				MemoryStream memoryStream = new MemoryStream(pkg.imageStream);

				BitmapImage image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = memoryStream;
				image.EndInit();
				m_hImageShow.Source = image;
			}));
		}
	}
}
