using System;
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
    /// Interaction logic for frmSysAdmin.xaml
    /// </summary>
    public partial class frmSysAdmin : Window
    {
        User user;
        frmLogin mainFrame;
        public frmSysAdmin()
        {
            InitializeComponent();
        }

        public frmSysAdmin(User u, frmLogin mainFrame):this()
        {
            this.user = u;
            this.mainFrame = mainFrame;
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            EvmsServiceClient client = new EvmsServiceClient();

            // Use the 'client' variable to call operations on the service.

            // Always close the client.
            try
            {
                
                //User u = client.login(txtUserID.Text.Trim(), txtPassword.Text.Trim());
                this.richTextBox1.AppendText(" " + user.Name + " " + user.isSystemAdmin);
                //var admForm = new frmSysAdmin(u);
                //admForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();


        }
    }
}
