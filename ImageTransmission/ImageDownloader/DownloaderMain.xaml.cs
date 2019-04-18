﻿using Newtonsoft.Json;
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
        int m_iTotalProgress;
        Queue<byte[]> m_pImgDataGrp;

        Thread m_hImgShowFunc;
        bool m_bRunning;
        bool m_bUpdate;

        public MainWindow()
		{
            m_iTotalProgress = 0;
            m_pImgDataGrp = new Queue<byte[]>();

            m_hImgShowFunc = new Thread(ImgShowFunc);
            m_bRunning = true;
            m_hImgShowFunc.Start();
            m_bUpdate = false;

			InitializeComponent();

			Thread pDownloadProc = new Thread(DownloadImgDataFunc);
			pDownloadProc.Start();
		}

		private void DownloadImgDataFunc()
		{
			//String szRequestUrl = "http://127.0.0.1:8080/download";
			String szRequestUrl = "http://WebBGTest-env-1.pef5ybuuuv.ap-northeast-1.elasticbeanstalk.com/download";
			while (m_bRunning)
			{
				HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl + "?imgProgress=" + m_iTotalProgress.ToString());
				hRequest.Method = "GET";
				hRequest.ContentType = "application/json";
				hRequest.Timeout = 250;

				HttpWebResponse hResult;
				try
				{
					hResult = hRequest.GetResponse() as HttpWebResponse;
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
					continue;
				}
				StreamReader hReader = new StreamReader(hResult.GetResponseStream());
				String szResult = hReader.ReadToEnd();
				ImageTransmissionType pkg = JsonConvert.DeserializeObject<ImageTransmissionType>(szResult);
				if (pkg != null)
				{
					if (pkg.m_szImageProgress < m_iTotalProgress)
						continue;

					m_iTotalProgress = pkg.m_szImageProgress;
					m_bUpdate = false;
					foreach (byte[] img in pkg.m_pImgStreamGrp)
					{
						m_pImgDataGrp.Enqueue(img);
					}
					m_bUpdate = true;
				}
			}
		}

		//public void DownloadResponse(IAsyncResult hAsyncResult)
		//{
		//	HttpWebRequest hRequest = hAsyncResult.AsyncState as HttpWebRequest;
		//	HttpWebResponse hResult;
		//	try
		//	{
		//		hResult = hRequest.EndGetResponse(hAsyncResult) as HttpWebResponse;
		//	}
		//	catch (Exception e)
		//	{
  //              System.Diagnostics.Debug.WriteLine(e.Message);
  //              return;
		//	}
		//	StreamReader hReader = new StreamReader(hResult.GetResponseStream());
		//	String szResult = hReader.ReadToEnd();
		//	ImageTransmissionType pkg = JsonConvert.DeserializeObject<ImageTransmissionType>(szResult);
		//	if (pkg != null)
		//	{
  //              if (Convert.ToInt32(pkg.m_szImageProgress) < m_iTotalProgress)
  //                  return;
                
  //              m_iTotalProgress = Convert.ToInt32(pkg.m_szImageProgress);
  //              m_bUpdate = false;
  //              foreach (byte[] img in pkg.m_pImgStreamGrp)
  //              {
  //                  m_pImgDataGrp.Enqueue(img);
  //              }
  //              m_bUpdate = true;

  //          }
		//}

        private void ImgShowFunc()
        {
            while (m_bRunning)
            {
                if (m_bUpdate && (m_pImgDataGrp.Count > 0))
                {
                    byte[] img = m_pImgDataGrp.Dequeue();
					MemoryStream memoryStream = new MemoryStream(img);

                    Dispatcher.Invoke(new Action(delegate
                    {  
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = memoryStream;
                        image.EndInit();
                        m_hImageShow.Source = image;

                    }));
					if (m_pImgDataGrp.Count < 10)
					{
						Thread.Sleep(50 + 300 / (m_pImgDataGrp.Count + 1));
					}
					else
					{
						Thread.Sleep(50);
					}                                 
                }
            }

        }

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			m_bRunning = false;
		}
	}
}
