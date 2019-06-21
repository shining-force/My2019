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
using DatabaseConsole;

namespace PageSelector
{
	/// <summary>
	/// UserSetPage.xaml 的交互逻辑
	/// </summary>
	public partial class UserSetPage : Page
	{
        private Window mParent;
		public UserSetPage(Window parent)
		{
			InitializeComponent();
            mParent = parent;
        }

        //*****************************functions*********************************//
        private void mPageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mParent.DragMove();
        }

        private void mSetBtn_Click(object sender, RoutedEventArgs e)
        {
            DAO.sUserName = mUserNameBox.Text;
            DAO.sPassword = mPasswordBox.Password;
            DAO.sConsoleUrl = mUrlBox.Text;
            MessageBox.Show("设置完成！");
        }

        private void mTitleLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mParent.DragMove();
        }
    }
}
