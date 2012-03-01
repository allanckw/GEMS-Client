﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmViewUsers.xaml
    /// </summary>
    public partial class frmViewUsers : Window
    {
        User user;
        Window mainFrame;

        public frmViewUsers()
        {
            InitializeComponent();
        }

        public frmViewUsers(User u, frmMain mainFrame)
            : this()
        {
            this.user = u;
            this.mainFrame = mainFrame;
            this.cboRole.ItemsSource = System.Enum.GetValues(typeof(EnumRoles));
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                List<User> list = client.searchUserByRole(txtName.Text.Trim(), txtUserID.Text.Trim(), 
                    (EnumRoles)cboRole.SelectedIndex).ToList<User>();
                lstUsers.SelectedValuePath = "userID";
                lstUsers.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
 
        }

    }
}
