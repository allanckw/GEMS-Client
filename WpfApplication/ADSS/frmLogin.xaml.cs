using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    public partial class frmLogin : Window
    {
        public frmLogin()
        {
            InitializeComponent();
            this.txtUserID.Focus();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.Compare(txtSvAddr.Text.Trim(), ConfigHelper.GetEndpointAddress(), true) != 0)
            {
                ConfigHelper.SaveEndpointAddress(txtSvAddr.Text.Trim());
            }

            WCFHelperClient client = new WCFHelperClient();
            try
            {
                User u = client.login(txtUserID.Text.Trim(), txtPassword.Password);
                var admForm = new frmMain(u, this);
                this.Visibility = Visibility.Collapsed;
                this.txtPassword.Clear();
                this.txtUserID.Clear();
                this.txtUserID.Focus();
                admForm.Show();
                client.Close();  // Always close the client.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSvAddr.Text = ConfigHelper.GetEndpointAddress();
        }

    }
}

