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
    public enum EnumRoles
    {
        System_Admin = 0,
        Location_Admin = 1,
        Event_Organizer = 2,
        Nil = 3

    }

    /// <summary>
    /// Interaction logic for frmAssign.xaml
    /// </summary>
    public partial class frmAssign : Window
    {
        User user;
        frmSearchUsers admFrame;

        string idToAssign;
        EnumRoles action;

        public frmAssign()
        {
            InitializeComponent();
        }

        public frmAssign(User u, string uid, EnumRoles x, frmSearchUsers f)
            : this()
        {
            this.user = u;
            this.idToAssign = uid;
            this.action = x;
            this.admFrame = f;

            this.txtAssn.Text = x.ToString().Replace("_", " ");
            this.txtUserID.Text = uid;
            EvmsServiceClient client = new EvmsServiceClient();

            this.txtCurrRole.Text = ((EnumRoles)client.viewUserRole(uid).RoleLevel).ToString();
            client.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            admFrame.Visibility = Visibility.Visible;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            EvmsServiceClient client = new EvmsServiceClient();
            bool success = false;
            if (txtCurrRole.Text.CompareTo(EnumRoles.Nil.ToString()) == 0)
            {
                if (action == EnumRoles.Event_Organizer)
                {
                    success = client.assignEventOrganizer(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                }
                else if (action == EnumRoles.Location_Admin)
                {
                    success = client.assignLocationAdmin(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                }
                else if (action == EnumRoles.System_Admin)
                {
                    success = client.assignSystemAdmin(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                }
            }
            else
            {
                if(MessageBox.Show("Are you sure that you want to overwrite " + txtUserID.Text + "'s Role?",
                    "Confirm Role Change",MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes){
                        if (action == EnumRoles.Event_Organizer)
                        {
                            success = client.assignEventOrganizer(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                        }
                        else if (action == EnumRoles.Location_Admin)
                        {
                            success = client.assignLocationAdmin(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                        }
                        else if (action == EnumRoles.System_Admin)
                        {
                            success = client.assignSystemAdmin(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                        }
                    }
            }

            if (success)
            {
                MessageBox.Show("Role have been added/updated", "Updated Role", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                admFrame.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("An Error have occured, please try again or contact service administrator!", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
