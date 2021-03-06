﻿using System;
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
	/// UserSetPage.xaml 的交互逻辑
	/// </summary>
	public partial class UserSetPage : Page
	{
        private Window m_hParent;
		public UserSetPage(Window parent)
		{
			InitializeComponent();
            m_hParent = parent;
        }

		private void M_hTitleLabel_Loaded(object sender, RoutedEventArgs e)
		{
			m_hTitleLabel.FontSize = m_hTitleLabel.ActualHeight * 0.5;
		}

		private void M_hUserNameLabel_Loaded(object sender, RoutedEventArgs e)
		{
			m_hUserNameLabel.FontSize = m_hUserNameLabel.ActualHeight * 0.25;
		}

		private void M_hPasswordLabel_Loaded(object sender, RoutedEventArgs e)
		{
			m_hPasswordLabel.FontSize = m_hPasswordLabel.ActualHeight * 0.25;
		}

		private void M_hUserNameBox_Loaded(object sender, RoutedEventArgs e)
		{
			m_hUserNameBox.FontSize = m_hUserNameBox.ActualHeight * 0.3;
			m_hUserNameBox.Margin = new Thickness(0, m_hUserNameBox.ActualHeight * 0.2, 0, m_hUserNameBox.ActualHeight * 0.2);
            m_hUserNameBox.Text = UserDataDAO.m_szUserName;          
        }

		private void M_hPasswordBox_Loaded(object sender, RoutedEventArgs e)
		{
			m_hPasswordBox.FontSize = m_hPasswordBox.ActualHeight * 0.3;
			m_hPasswordBox.Margin = new Thickness(0, m_hPasswordBox.ActualHeight * 0.2, 0, m_hPasswordBox.ActualHeight * 0.2);
            m_hPasswordBox.Password = UserDataDAO.m_szPassword;
        }

		private void M_hSetBtn_Loaded(object sender, RoutedEventArgs e)
		{
			m_hSetBtn.FontSize = m_hSetBtn.ActualHeight * 0.5;
			m_hSetBtn.Margin = 
				new Thickness(m_hSetBtn.ActualWidth * 0.2, m_hSetBtn.ActualHeight * 0.15, m_hSetBtn.ActualWidth * 0.2, m_hSetBtn.ActualHeight * 0.15);
		}

        private void m_hPageTitle_Loaded(object sender, RoutedEventArgs e)
        {
            m_hPageTitle.FontSize = m_hPageTitle.ActualHeight * 0.5;
        }

        //*****************************functions*********************************//
        private void m_hPageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            m_hParent.DragMove();
        }

        private void m_hSetBtn_Click(object sender, RoutedEventArgs e)
        {
            UserDataDAO.m_szUserName = m_hUserNameBox.Text;
            UserDataDAO.m_szPassword = m_hPasswordBox.Password;

            MessageBox.Show("设置完成！");
        }
    }
}
