using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Data;

namespace DatabaseConsole
{
    /// <summary>
    /// AccountTable.xaml 的交互逻辑
    /// </summary>
    public partial class FlowerInfoPage : Page
    {
        private class FlowerInfoType
        {
            public int mId
            {
                get { return id; }
                set { if (mInited) return; else id = value; }
            }
            public string mName
            {
                get { return name; }
                set { name = value;if (mInited) sync();  }
            }
            public string mDescription
            {
                get { return description; }
                set { description = value;if (mInited) sync();  }
            }
            public string mInfo
            {
                get { return info; }
                set { info = value;if (mInited) sync();  }
            }

            public FlowerInfoType(int id) 
            {
                mId = id;
                //read from database
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11R1";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPFI&" + id;
                ConsoleCodeDownTransmissionType readDown = new ConsoleCodeDownTransmissionType();
                HttpHandlerException readE = handler.goSingle(up, out readDown, "POST");
                if (readE.IsOK())
                {
                    string[] dataGrp = readDown.mServiceAnwser.Split('&');
                    mName = dataGrp[0];
                    mDescription = dataGrp[1];
                    mInfo = dataGrp[2];
                    mInited = true;
                }
                else
                    throw new Exception(readE.getMessage());
            }

            public FlowerInfoType(string name, string description, string info)
            {
                mName = name;
                mDescription = description;
                mInfo = info;
                //add to database
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11N1";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPFI&" + mName + "&" + mDescription + "&" + mInfo;
                ConsoleCodeDownTransmissionType newDown = new ConsoleCodeDownTransmissionType();
                HttpHandlerException newE = handler.goSingle(up, out newDown, "POST");
                if (newE.IsOK())
                {
                    mId = Convert.ToInt32(newDown.mServiceAnwser);
                    mInited = true;
                }
                else
                    throw new Exception(newE.getMessage());
            }

            private bool mInited;

            private int id;
            private string name;
            private string description;
            private string info;
            private void sync()
            {
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11U0";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPFI&" + mId + "&" + mName + "&" + mDescription + "&" + mInfo;
                ConsoleCodeDownTransmissionType upDateDown = new ConsoleCodeDownTransmissionType();
                HttpHandlerException syncE = handler.goSingle(up, out upDateDown, "POST");
                if (!syncE.IsOK())
                {
                    throw new Exception(syncE.getMessage());
                }
            }

            public void remove()
            {
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11D0";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPFI&" + mId;
                ConsoleCodeDownTransmissionType deleteDown = new ConsoleCodeDownTransmissionType();
                HttpHandlerException removeE = handler.goSingle(up, out deleteDown, "POST");
                if (!removeE.IsOK())
                {
                    throw new Exception(removeE.getMessage());
                }
            }
        }


        private static int sPageSize = 15;

        private Window mParent;
        private int mPageNo;
        public FlowerInfoPage(Window parent)
        {
            InitializeComponent();
            mParent = parent;
            mPageNo = 0;
        }

        private void refreshTable()
        {
            List<FlowerInfoType> infoList = new List<FlowerInfoType>();

            HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "11P1";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            up.mParamU = "RPFI&" + (sPageSize * mPageNo).ToString() + "&" + sPageSize.ToString();
            handler.goSingleAsync(up, "POST", new Action<ConsoleCodeDownTransmissionType, HttpHandlerException>(
                    delegate(ConsoleCodeDownTransmissionType idDown, HttpHandlerException idE) {
                        if (!idE.IsOK())
                        {
                            this.Dispatcher.Invoke(new Action(delegate {
                                mHttpState.Content = idE.getMessage();
                            }));
                            return;
                        }

                        string[] idGrp = idDown.mServiceAnwser.Split('&');
                        foreach (string id in idGrp)
                        {
                            if (id.Equals(""))
                                break;
                            FlowerInfoType info = new FlowerInfoType(Convert.ToInt32(id));
                            infoList.Add(info);
                        }
                        this.Dispatcher.Invoke(new Action(delegate {
                            mInfoTable.ItemsSource = infoList;
                            mHttpState.Content = "OK";
                        }));
            }));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mHttpState.Content = "Loading";
            refreshTable();               
        }

        private void mTitleLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mParent.DragMove();
        }

        private void mPageUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mPageNo > 0)
            {
                --mPageNo;
                refreshTable();
            }
            else if (mPageNo == -1)
            {
                mPageNo = 0;
                refreshTable();
            }
        }

        private void mPageDownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((List<FlowerInfoType>)mInfoTable.ItemsSource).Count >= sPageSize)
            {
                ++mPageNo;
                refreshTable();
            }
            else if (mPageNo == -1)
            {
                mPageNo = 0;
                refreshTable();
            }
            return;

        }

        private void mDeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            List<FlowerInfoType> list = mInfoTable.ItemsSource as List<FlowerInfoType>;
            list[mInfoTable.SelectedIndex].remove();
            refreshTable();
        }

        private void mAddMenu_Click(object sender, RoutedEventArgs e)
        {
            List<FlowerInfoType> list = new List<FlowerInfoType>();
            list.Add(new FlowerInfoType("new", "des", "info"));
            mInfoTable.ItemsSource = list;

            mPageNo = -1;
        }
    }
}
