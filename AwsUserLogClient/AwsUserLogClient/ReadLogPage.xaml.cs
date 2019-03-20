using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace AwsUserLogClient
{
	/// <summary>
	/// ReadLogPage.xaml 的交互逻辑
	/// </summary>
	public partial class ReadLogPage : Page
	{
        private Window m_hParent;
		public ReadLogPage(Window parent)
		{
			InitializeComponent();
            m_hParent = parent;
		}

        private void m_hLogViewer_Loaded(object sender, RoutedEventArgs e)
        {
            m_hViewerCol_Date.Width = m_hLogViewer.ActualWidth / 5;
            m_hViewerCol_LogTitle.Width = m_hLogViewer.ActualWidth / 5;
            m_hViewerCol_FromUser.Width = m_hLogViewer.ActualWidth / 5;
            m_hViewerCol_LogDetail.Width = m_hLogViewer.ActualWidth / 3;
            m_hLogViewer.ItemsSource = UserDataDAO.m_pUseLogList;
        }

        private void m_hPageTitle_Loaded(object sender, RoutedEventArgs e)
        {
            m_hPageTitle.FontSize = m_hPageTitle.ActualHeight * 0.5;
        }

        private void m_hStDataLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hStDataLabel.FontSize = m_hStDataLabel.ActualHeight * 0.4;
        }

        private void m_hEdDataLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hEdDataLabel.FontSize = m_hEdDataLabel.ActualHeight * 0.4;
        }

        private void m_hReadDataBtn_Loaded(object sender, RoutedEventArgs e)
        {
            m_hReadDataBtn.FontSize = m_hReadDataBtn.ActualHeight * 0.4;
            m_hReadDataBtn.Margin = new Thickness(0, 0, m_hReadDataBtn.ActualWidth * 0.2, m_hReadDataBtn.ActualHeight * 0.1);
        }

        private void m_hStDateBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hStDateBox.FontSize = m_hStDateBox.ActualHeight * 0.4;
            m_hStDateBox.Margin = new Thickness(0, 0, m_hStDateBox.ActualWidth * 0.1, m_hStDateBox.ActualHeight * 0.1);
        }

        private void m_hEdDateBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hEdDateBox.FontSize = m_hEdDateBox.ActualHeight * 0.4;
            m_hEdDateBox.Margin = new Thickness(0, 0, m_hEdDateBox.ActualWidth * 0.1, m_hEdDateBox.ActualHeight * 0.1);
        }

        private void m_hState_Loaded(object sender, RoutedEventArgs e)
        {
            m_hState.FontSize = m_hState.ActualHeight * 0.4;
        }

        private void m_hOutputDataBtn_Loaded(object sender, RoutedEventArgs e)
        {  
            m_hOutputDataBtn.FontSize = m_hOutputDataBtn.ActualHeight * 0.4;
            m_hOutputDataBtn.Margin = new Thickness(0, 0, m_hOutputDataBtn.ActualWidth * 0.2, m_hOutputDataBtn.ActualHeight * 0.1);
        }


        //*****************************functions*****************************//
        private void m_hPageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            m_hParent.DragMove();
        }

        private void m_hReadDataBtn_Click(object sender, RoutedEventArgs e)
        {
            String szStDate;
            String szEdDate;
            String szFormat = "yyyy-MM-dd";
            if ((m_hStDateBox.SelectedDate == null) || (m_hEdDateBox.SelectedDate == null))
            {
                szStDate = "2000-01-01";
                szEdDate = "3000-01-01";
            }
            else
            {
                szStDate = m_hStDateBox.SelectedDate.Value.ToString(szFormat);
                szEdDate = m_hEdDateBox.SelectedDate.Value.ToString(szFormat);
            }
            String szRequestUrl = UserDataDAO.m_szBaseUrl + "/UseLogs?mSW={0}&st={1}&ed={2}&format={3}";
            String szSW = UserDataDAO.CreateSW();

            szRequestUrl = String.Format(szRequestUrl, szSW, szStDate, szEdDate, szFormat);
            HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl);
            hRequest.Method = "GET";
            hRequest.Timeout = 10000;            

            hRequest.BeginGetResponse(new AsyncCallback(LogReadResponse), hRequest);

            m_hState.Content = "查询中...";
        }

        public void LogReadResponse(IAsyncResult hAsyncResult)
        {
            HttpWebRequest hRequest = hAsyncResult.AsyncState as HttpWebRequest;
            HttpWebResponse hResult;
            try
            {
                hResult = hRequest.EndGetResponse(hAsyncResult) as HttpWebResponse;
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(new Action(delegate { m_hState.Content = e.Message; }));
                
                return;
            }

            StreamReader hReader = new StreamReader(hResult.GetResponseStream());
            String szResult = hReader.ReadToEnd();
            UserDataDAO.m_pUseLogList = JsonConvert.DeserializeObject<List<AwsLog_Transmit>>(szResult);

            Dispatcher.Invoke(new Action(delegate 
            {
                m_hState.Content = "完成";
                m_hLogViewer.ItemsSource = UserDataDAO.m_pUseLogList;
            }));
        }

        private void m_hLogViewer_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            AwsLog_Transmit log = (AwsLog_Transmit)m_hLogViewer.SelectedItem;
            m_hDetailbox.Text = "详细：" + log.mDetail;
        }

        private void m_hOutputDataBtn_Click(object sender, RoutedEventArgs e)
        {
            String szFileName = UserDataDAO.m_pUseLogList.First().mFormatDate + UserDataDAO.m_pUseLogList.Count.ToString() + ".csv";
            List<String> pContant = new List<string>();
            foreach (AwsLog_Transmit log in UserDataDAO.m_pUseLogList)
            {
                pContant.Add(log.mFormatDate + "," + log.mTitle + "," + log.mFromUser + "," + log.mDetail);
            }
            File.WriteAllLines(szFileName, pContant);
            m_hState.Content = "日志已输出在当前目录，文件名" + szFileName;
        }
    }
}
