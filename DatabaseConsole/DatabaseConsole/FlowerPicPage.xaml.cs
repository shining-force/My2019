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
using System.IO;
using System.Resources;
using System.Drawing;

namespace DatabaseConsole
{
    /// <summary>
    /// AccountTable.xaml 的交互逻辑
    /// </summary>
    public partial class FlowerPicPage : Page
    {
        private static int sPageSize = 6;

        private Window mParent;
        private int mPageNo;
        public FlowerPicPage(Window parent)
        {
            InitializeComponent();
            mParent = parent;
            mPageNo = 0;
        }

        private void refreshTable()
        {
            this.Dispatcher.Invoke(new Action(delegate {
                mPicTable.Children.Clear();
            }));
            HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "11P1";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            up.mParamU = "RPFP&" + (sPageSize * mPageNo).ToString() + "&" + sPageSize.ToString();
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
                        Action onDelete = new Action(delegate { refreshTable(); });
                        foreach (string id in idGrp)
                        {
                            if (id.Equals(""))
                                break;
                            
                            this.Dispatcher.Invoke(new Action(delegate {
                                FlowerPicCtrl ctrl = new FlowerPicCtrl(Convert.ToInt32(id), onDelete);
                                mPicTable.Children.Add(ctrl);
                            }));                            
                        }
                        this.Dispatcher.Invoke(new Action(delegate {
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
            if (mPicTable.Children.Count >= sPageSize)
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
        private void mAddMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Bitmap bitmap = new Bitmap(".\\Resources\\newPic.bmp");
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    Action onDelete = new Action(delegate { refreshTable(); });

                    mPicTable.Children.Clear();                
                    mPicTable.Children.Add(new FlowerPicCtrl("new", " ", stream.ToArray(), " ", onDelete));

                }
                mPageNo = -1;
            }
            catch { }
        }
    }
}
