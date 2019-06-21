using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
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
    /// FlowerPicCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class FlowerPicCtrl : UserControl, INotifyPropertyChanged
    {
        public int mId
        {
            get { return id; }
            set { if (mInited) return; else id = value; }
        }
        public string mName
        {
            get { return name; }
            set { name = value; if (mInited) { sync(); PropertyChanged(this, new PropertyChangedEventArgs("mPicBuf")); } }
        }
        public string mDescription
        {
            get { return description; }
            set { description = value; if (mInited) { sync(); PropertyChanged(this, new PropertyChangedEventArgs("mDescription")); } }
        }
        public byte[] mPicBuf
        {
            get { return picBuf; }
            set
            {
                picBuf = value;
                
                if (mInited) { sync(); PropertyChanged(this, new PropertyChangedEventArgs("mPicBuf"));}
            }
        }
        public string mType
        {
            get { return type; }
            set { type = value; if (mInited) { sync();} }
        }

        private int id;
        private string name;
        private string description;
        private byte[] picBuf;
        private string type;

        private bool mInited;
        private Action mOnDelete;

        public event PropertyChangedEventHandler PropertyChanged;

        public FlowerPicCtrl(int id, Action onDelete)
        {
            mId = id;
            mOnDelete = onDelete;
            //read from database
            HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "11R1";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            up.mParamU = "RPFP&" + id;
            ConsoleCodeDownTransmissionType readDown = new ConsoleCodeDownTransmissionType();
            HttpHandlerException readE = handler.goSingle(up, out readDown, "POST");
            if (readE.IsOK())
            {
                string[] dataGrp = readDown.mServiceAnwser.Split('&');
                mName = dataGrp[0];
                mDescription = dataGrp[1];
                mType = dataGrp[2];
                StringBuilder picData = new StringBuilder();
                picData.Append(dataGrp[3]);
                if (dataGrp.Length > 4)
                {
                    //recover data
                    for (int index = 4; index < dataGrp.Length; ++index)
                    {
                        picData.Append("&").Append(dataGrp[index]);
                    }
                }
                mPicBuf = Convert.FromBase64String(picData.ToString());

                mInited = true;
            }
            else
                throw new Exception(readE.getMessage());

            InitializeComponent();
        }
        public FlowerPicCtrl(string name, string description, byte[] buf, string type, Action onDelete)
        {
            mName = name;
            mDescription = description;
            mPicBuf = buf;
            mType = type;
            mOnDelete = onDelete;
            //add to database
            HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "11N1";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            up.mParamU = "RPFP&" + mName + "&" + mDescription + "&" + mType + "&" + Convert.ToBase64String(mPicBuf);
            ConsoleCodeDownTransmissionType newDown = new ConsoleCodeDownTransmissionType();
            HttpHandlerException newE = handler.goSingle(up, out newDown, "POST");
            if (newE.IsOK())
            {
                mId = Convert.ToInt32(newDown.mServiceAnwser);
                mInited = true;
            }
            else
                throw new Exception(newE.getMessage());

            InitializeComponent();
        }

        private void sync()
        {
            HttpHandler handler = new HttpHandler(DAO.sConsoleUrl);
            ConsoleCodeUpTransmissionType up = new ConsoleCodeUpTransmissionType();
            up.mCode = "11U0";
            up.mParamL = StdMd5Maker.toMd5String(DAO.sUserName + DAO.sPassword);
            up.mParamU = "RPFP&" + mId + "&" + mName + "&" + mDescription + "&" + mType + "&" + Convert.ToBase64String(mPicBuf);
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
            up.mParamU = "RPFP&" + mId;
            ConsoleCodeDownTransmissionType deleteDown = new ConsoleCodeDownTransmissionType();
            HttpHandlerException removeE = handler.goSingle(up, out deleteDown, "POST");
            if (!removeE.IsOK())
            {
                throw new Exception(removeE.getMessage());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mNameBox.DataContext = this;
            mImage.DataContext = this;
            mDescriptionBox.DataContext = this;
        }

        private void mDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            remove();
            mOnDelete();
        }

        private void mImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog selectWin = new System.Windows.Forms.OpenFileDialog();
            selectWin.DefaultExt = ".";
            selectWin.Filter = "所有文件(*.*)|*.*";
            System.Windows.Forms.DialogResult ret = selectWin.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Bitmap bitMap = new Bitmap(selectWin.FileName);
                        bitMap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        mPicBuf = stream.ToArray();
                    }
                }
                catch { }
            }
        }
    }
}
