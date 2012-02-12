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
        frmMain mainFrame;
        public frmSysAdmin()
        {
            InitializeComponent();
        }

        public frmSysAdmin(User u, frmMain mainFrame)
            : this()
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            EvmsServiceClient client = new EvmsServiceClient();
            try
            {
                
                List<User> list = client.searchUser(txtName.Text.Trim(), txtUserID.Text.Trim()).ToList<User>();
                lstUsers.SelectedValuePath = "userID";
                lstUsers.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        private void btnAssLocAdm_Click(object sender, RoutedEventArgs e)
        {
            if (lstUsers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a user!");
                return;
            }
            string uid = lstUsers.SelectedValue.ToString();
            Console.WriteLine(uid);
            var assignForm = new frmAssign(this.user, uid, EnumRoles.LocationAdmin, this);
            assignForm.ShowDialog();
        }

        private void btnAssEventOrg_Click(object sender, RoutedEventArgs e)
        {
            if (lstUsers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a user!");
                return;
            }
            string uid = lstUsers.SelectedValue.ToString();
            var assignForm = new frmAssign(this.user, uid, EnumRoles.EventExco,this);
            assignForm.ShowDialog();
        }
    }
}
