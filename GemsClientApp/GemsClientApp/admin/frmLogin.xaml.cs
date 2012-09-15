using System;
using System.Windows;
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserID.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your user id", "Invalid Input", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }
            
            if (txtPassword.Password.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your password!", "Invalid Input", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            if (String.Compare(txtSvAddr.Text.Trim(), ConfigHelper.GetEndpointAddress(), true) != 0)
            {
                ConfigHelper.SaveEndpointAddress(txtSvAddr.Text.Trim());
            }

            AdminHelper client = new AdminHelper();
            try
            {
                //User u = client.Authenticate(txtUserID.Text.Trim(), txtPassword.Password);
                Credentials c = new Credentials();
                c.UserID = txtUserID.Text.Trim();
                c.Password = Helper.KeyGen.Encrypt(txtPassword.Password);
               
                User u = client.SecureAuthenticate(c);
                var admForm = new frmMain(u, this);
                this.Visibility = Visibility.Collapsed;
                this.txtPassword.Clear();
                this.txtUserID.Clear();
                this.txtUserID.Focus();
                admForm.Show();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();  // Always close the client.
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSvAddr.Text = ConfigHelper.GetEndpointAddress();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            txtPassword.Clear();
            if (txtUserID.Text.Length == 0)
            {
                txtUserID.Focus();
            }
            else
            {
                txtPassword.Focus();
            }
           
        }

    }
}

