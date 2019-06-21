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
    public partial class AccountPage : Page
    {
        private class AccountType
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
            public string mPsw
            {
                get { return psw; }
                set { psw = value;if (mInited) sync();  }
            }
            public string mLvl
            {
                get { return lvl; }
                set { lvl = value;if (mInited) sync();  }
            }

            public AccountType(int id) 
            {
                mId = id;
                //read from database
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11R1";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPAT&" + id;
                ConsoleCodeDownTransmissionType readDown = new ConsoleCodeDownTransmissionType();
                HttpHandlerException readE = handler.goSingle(up, out readDown, "POST");
                if (readE.IsOK())
                {
                    string[] dataGrp = readDown.mServiceAnwser.Split('&');
                    mName = dataGrp[0];
                    mPsw = dataGrp[1];
                    mLvl = dataGrp[2];
                    mInited = true;
                }
                else
                    throw new Exception(readE.getMessage());
            }

            public AccountType(string name, string psw, string lvl)
            {
                mName = name;
                mPsw = psw;
                mLvl = lvl;
                //add to database
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11N1";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPAT&" + mName + "&" + mPsw + "&" + mLvl;
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
            private string psw;
            private string lvl;
            private void sync()
            {
                HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
                ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
                up.mCode = "11U0";
                up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
                up.mParamU = "RPAT&" + mId + "&" + mName + "&" + mPsw + "&" + mLvl;
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
                up.mParamU = "RPAT&" + mId;
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
        public AccountPage(Window parent)
        {
            InitializeComponent();
            mParent = parent;
            mPageNo = 0;
        }

        private void refreshTable()
        {
            List<AccountType> accountList = new List<AccountType>();

            HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "11P1";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            up.mParamU = "RPAT&" + (sPageSize * mPageNo).ToString() + "&" + sPageSize.ToString();
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
                            AccountType account = new AccountType(Convert.ToInt32(id));
                            accountList.Add(account);
                        }
                        this.Dispatcher.Invoke(new Action(delegate {
                            mAccountTable.ItemsSource = accountList;
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
            if (((List<AccountType>)mAccountTable.ItemsSource).Count >= sPageSize)
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
            List<AccountType> list = mAccountTable.ItemsSource as List<AccountType>;
            list[mAccountTable.SelectedIndex].remove();
            refreshTable();
        }

        private void mAddMenu_Click(object sender, RoutedEventArgs e)
        {
            List<AccountType> list = new List<AccountType>();
            list.Add(new AccountType("new", "psw", "2"));
            mAccountTable.ItemsSource = list;

            mPageNo = -1;
        }
    }
}
