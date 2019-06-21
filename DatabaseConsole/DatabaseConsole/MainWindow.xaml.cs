using DatabaseConsole;
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

namespace PageSelector
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

		private enum ePage
		{
			UserSetPage,
            ConsolePage,
            AccountPage,
            FlowerInfoPage,
            FlowerPicPage
		}
		private ePage mCurrentPage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mLogInfo_Click(object sender, RoutedEventArgs e)
        {
            if (mCurrentPage == ePage.UserSetPage)
                return;
            mPageFrame.Navigate(new UserSetPage(this));
            mCurrentPage = ePage.UserSetPage;
        }

        private void mPageFrame_Loaded(object sender, RoutedEventArgs e)
        {
            mPageFrame.Navigate(new UserSetPage(this));
            mCurrentPage = ePage.UserSetPage;
        }

        private void mConsloeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mCurrentPage == ePage.ConsolePage)
                return;
            mPageFrame.Navigate(new ConsolePage(this));
            mCurrentPage = ePage.ConsolePage;
        }

        private void mCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void mAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mCurrentPage == ePage.AccountPage)
                return;
            mPageFrame.Navigate(new AccountPage(this));
            mCurrentPage = ePage.AccountPage;
        }

        private void mFlowerInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mCurrentPage == ePage.FlowerInfoPage)
                return;
            mPageFrame.Navigate(new FlowerInfoPage(this));
            mCurrentPage = ePage.FlowerInfoPage;
        }

        private void mFlowerPicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mCurrentPage == ePage.FlowerPicPage)
                return;
            mPageFrame.Navigate(new FlowerPicPage(this));
            mCurrentPage = ePage.FlowerPicPage;
        }
    }
}
