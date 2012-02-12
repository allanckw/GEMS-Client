using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using evmsService.entities;
using System.ComponentModel;
using System.Windows.Threading;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for WindowStyleSampleTest.xaml
    /// </summary>
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
            EvmsServiceClient client = new EvmsServiceClient();
            try
            {
                User u = client.login(txtUserID.Text.Trim(), txtPassword.Password);
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
            finally
            {
                client.Close();  // Always close the client.
            }
           
        }

    }
}

