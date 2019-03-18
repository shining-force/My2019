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

namespace AwsUserLogClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
		private enum E_PAGE
		{
			e_UserSetPage,
			e_ReadLogPage,
			e_WriteLogPage,
			e_OutputPage
		}
		private E_PAGE m_eCurrentPage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void m_hCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

		private void M_hUserSetPageBtn_Click(object sender, RoutedEventArgs e)
		{
			if (m_eCurrentPage == E_PAGE.e_UserSetPage)
				return;
			m_hView.Navigate(new UserSetPage(this));
			m_eCurrentPage = E_PAGE.e_UserSetPage;
		}

		private void M_hReadLogPageBtn_Click(object sender, RoutedEventArgs e)
		{
			if (m_eCurrentPage == E_PAGE.e_ReadLogPage)
				return;
			m_hView.Navigate(new ReadLogPage(this));
			m_eCurrentPage = E_PAGE.e_ReadLogPage;
		}

		private void M_hWriteLogPageBtn_Click(object sender, RoutedEventArgs e)
		{
			if (m_eCurrentPage == E_PAGE.e_WriteLogPage)
				return;
			m_hView.Navigate(new WriteLogPage());
			m_eCurrentPage = E_PAGE.e_WriteLogPage;
		}


		private void M_hView_Loaded(object sender, RoutedEventArgs e)
		{
			m_hView.Navigate(new UserSetPage(this));
			m_eCurrentPage = E_PAGE.e_UserSetPage;
		}
	}
}
