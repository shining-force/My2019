using Newtonsoft.Json;
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

namespace AwsUserLogClient
{
	/// <summary>
	/// WriteLogPage.xaml 的交互逻辑
	/// </summary>
	public partial class WriteLogPage : Page
	{
        private Window m_hParent;
		public WriteLogPage(Window parent)
		{
			InitializeComponent();
            m_hParent = parent;
        }

        private void m_hPageTitle_Loaded(object sender, RoutedEventArgs e)
        {
            m_hPageTitle.FontSize = m_hPageTitle.ActualHeight * 0.5;
        }

        private void m_hLogTitleLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hLogTitleLabel.FontSize = m_hLogTitleLabel.ActualHeight * 0.5;
        }

        private void m_hLogTitleBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hLogTitleBox.FontSize = m_hLogTitleBox.ActualHeight * 0.5;
            m_hLogTitleBox.Text = UserDataDAO.m_szLogTitleOnWriting;
        }

        private void m_hLogDetailLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hLogDetailLabel.FontSize = m_hLogDetailLabel.ActualHeight * 0.5;
        }

        private void m_hLogDetailBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hLogDetailBox.Text = UserDataDAO.m_szLogDetailOnWriting;
        }

        private void m_hSendBtn_LayoutUpdated(object sender, EventArgs e)
        {
            m_hSendBtn.FontSize = m_hSendBtn.ActualHeight * 0.5;
        }

        //**********************************functions**********************************//
        private void m_hPageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            m_hParent.DragMove();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            UserDataDAO.m_szLogTitleOnWriting = m_hLogTitleBox.Text;
            UserDataDAO.m_szLogDetailOnWriting = m_hLogDetailBox.Text;
        }

        private void m_hSendBtn_Click(object sender, RoutedEventArgs e)
        {
            String szRequestUrl = UserDataDAO.m_szBaseUrl + "/NewLog";
            String szSW = UserDataDAO.CreateSW();
            HttpWebRequest hRequest = (HttpWebRequest)WebRequest.Create(szRequestUrl);
            hRequest.Method = "POST";
            hRequest.ContentType = "application/json";
            hRequest.Timeout = 10000;

            AwsLog_Transmit log = new AwsLog_Transmit();
            log.mSW = UserDataDAO.CreateSW();
            log.mFormat = "yyyy-MM-dd";
            log.mFormatDate = DateTime.Now.ToString(log.mFormat);
            log.mTitle = m_hLogTitleBox.Text;
            log.mDetail = m_hLogDetailBox.Text;

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(log, jsSettings);

            Stream req = hRequest.GetRequestStream();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            req.Write(bytes, 0, bytes.Length);

            hRequest.BeginGetResponse(new AsyncCallback(LogWriteResponse), hRequest);
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
                MessageBox.Show(e.Message);
                return;
            }
            MessageBox.Show("发送成功。");
            Dispatcher.Invoke(new Action(delegate { m_hLogTitleBox.Text = ""; m_hLogDetailBox.Text = ""; }));

        }
    }
}
