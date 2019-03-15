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
	/// WriteLogPage.xaml 的交互逻辑
	/// </summary>
	public partial class WriteLogPage : Page
	{
		public WriteLogPage()
		{
			InitializeComponent();
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
        }

        private void m_hLogDetailLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hLogDetailLabel.FontSize = m_hLogDetailLabel.ActualHeight * 0.5;
        }

        private void m_hLogDetailBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hLogDetailBox.FontSize = m_hLogDetailBox.ActualHeight * 0.5;
        }

        private void m_hSendBtn_LayoutUpdated(object sender, EventArgs e)
        {
            m_hSendBtn.FontSize = m_hSendBtn.ActualHeight * 0.5;
        }
    }
}
