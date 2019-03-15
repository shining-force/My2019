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
	/// ReadLogPage.xaml 的交互逻辑
	/// </summary>
	public partial class ReadLogPage : Page
	{
		public ReadLogPage()
		{
			InitializeComponent();
		}

        private void m_hLogViewer_Loaded(object sender, RoutedEventArgs e)
        {
            m_hViewerCol_Date.Width = m_hLogViewer.ActualWidth / 5;
            m_hViewerCol_LogTitle.Width = m_hLogViewer.ActualWidth / 5;
            m_hViewerCol_FromUser.Width = m_hLogViewer.ActualWidth / 5;
            m_hViewerCol_LogDetail.Width = m_hLogViewer.ActualWidth / 5 * 2; 
        }

        private void m_hPageTitle_Loaded(object sender, RoutedEventArgs e)
        {
            m_hPageTitle.FontSize = m_hPageTitle.ActualHeight * 0.5;
        }

        private void m_hStDataLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hStDataLabel.FontSize = m_hStDataLabel.ActualHeight * 0.4;
        }

        private void m_hEdDataLabel_Loaded(object sender, RoutedEventArgs e)
        {
            m_hEdDataLabel.FontSize = m_hEdDataLabel.ActualHeight * 0.4;
        }

        private void m_hReadDataBtn_Loaded(object sender, RoutedEventArgs e)
        {
            m_hReadDataBtn.FontSize = m_hReadDataBtn.ActualHeight * 0.4;
            m_hReadDataBtn.Margin = new Thickness(0, 0, m_hReadDataBtn.ActualWidth * 0.2, m_hReadDataBtn.ActualHeight * 0.1);
        }

        private void m_hStDateBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hStDateBox.FontSize = m_hStDateBox.ActualHeight * 0.4;
            m_hStDateBox.Margin = new Thickness(0, 0, m_hStDateBox.ActualWidth * 0.1, m_hStDateBox.ActualHeight * 0.1);
        }

        private void m_hEdDateBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_hEdDateBox.FontSize = m_hEdDateBox.ActualHeight * 0.4;
            m_hEdDateBox.Margin = new Thickness(0, 0, m_hEdDateBox.ActualWidth * 0.1, m_hEdDateBox.ActualHeight * 0.1);
        }

        private void m_hState_Loaded(object sender, RoutedEventArgs e)
        {
            m_hState.FontSize = m_hState.ActualHeight * 0.4;
        }

        private void m_hOutputDataBtn_Loaded(object sender, RoutedEventArgs e)
        {  
            m_hOutputDataBtn.FontSize = m_hOutputDataBtn.ActualHeight * 0.4;
            m_hOutputDataBtn.Margin = new Thickness(0, 0, m_hOutputDataBtn.ActualWidth * 0.2, m_hOutputDataBtn.ActualHeight * 0.1);
        }
    }
}
