using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DatabaseConsole
{
    /// <summary>
    /// consolePage.xaml 的交互逻辑
    /// </summary>
    public partial class ConsolePage : Page
    {
        private static string[] sHttpMethodInfo = 
        {
            "POST方法，在下方直接写入要发送的数据。",
            "GET方法，在下放写入url中\"?\"后要添加的数据"
        };

        private KeyPairsSheet mGetParams;
        private JsonSheet mPostParams;
        private Window mParent;
        public ConsolePage(Window parent)
        {
            InitializeComponent();
            mGetParams = new KeyPairsSheet();
            mPostParams = new JsonSheet();
            mParent = parent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mHttpMethodBox.SelectedIndex = 0;
            mPostParams.layObject(new ConsoleCodeUpTransmissionType());
            mConsoleUrlBox.Text = DAO.sConsoleUrl;

            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "00T1";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            HttpHandler handler = new HttpHandler(mConsoleUrlBox.Text);
            handler.goSingleAsync<ConsoleCodeDownTransmissionType>(up, "POST", handleHttpResponse);
        }

        private void mHttpMethodBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((mHttpMethodBox.SelectedIndex >= 0) && (mHttpMethodBox.SelectedIndex < sHttpMethodInfo.Length))
            {
                mHttpParamInfo.Content = sHttpMethodInfo[mHttpMethodBox.SelectedIndex];
                switch (((ComboBoxItem)mHttpMethodBox.SelectedItem).Content.ToString())
                {
                    case "POST":
                        mHttpPostParamFrame.Navigate(mPostParams);
                        break;
                    case "GET":
                    default:
                        mHttpPostParamFrame.Navigate(mGetParams);
                        break;
                }
            }
                
        }

        private void mSendBtn_Click(object sender, RoutedEventArgs e)
        {
            HttpHandler handler = new HttpHandler(mConsoleUrlBox.Text);
            switch (((ComboBoxItem)mHttpMethodBox.SelectedItem).Content.ToString())
            {
                case "POST":
                    handler.goSingleAsync<ConsoleCodeDownTransmissionType>(mPostParams.getObject<ConsoleCodeUpTransmissionType>(), "POST", handleHttpResponse);
                    mHttpResponseBox.Text = "...";
                    break;
                case "GET":
                default:
                    handler.goSingleAsync<ConsoleCodeDownTransmissionType>(mGetParams.getKeyPairs(), "GET", handleHttpResponse);
                    mHttpResponseBox.Text = "...";
                    break;
            }
        }

        private void handleHttpResponse(ConsoleCodeDownTransmissionType response, HttpHandlerException e)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                if (e.IsOK())
                {
                    mHttpResponseBox.Text = response.mServiceAnwser;
                    mHttpResponseStateBox.Content = "";
                }
                else
                {
                    mHttpResponseStateBox.Content = e.getMessage();
                    mHttpResponseBox.Text = "";
                }
            }));                  
        }

        private void mPageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mParent.DragMove();
        }
    }
}
