﻿using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System;

namespace Gems.UIWPF
{


    /// <summary>
    /// Interaction logic for frmAssign.xaml
    /// </summary>
    public partial class frmAssign : Window
    {
        User user;
        frmSearchUsers admFrame;
        EnumRoles action;

        public frmAssign()
        {
            InitializeComponent();
        }

        public frmAssign(User u, string uid, EnumRoles x, frmSearchUsers f)
            : this()
        {
            this.user = u;
            this.action = x;
            this.admFrame = f;

            this.txtAssn.Text = x.ToString();
            this.txtUserID.Text = uid;
            EvmsServiceClient client = new EvmsServiceClient();

            this.txtCurrRole.Text = ((EnumRoles)client.viewUserRole(uid)).ToString();
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

            try
            {
                if (txtCurrRole.Text.CompareTo(EnumRoles.Nil.ToString()) == 0)
                {
                    if (action == EnumRoles.Event_Organizer)
                    {
                        client.assignEventOrganizer(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                    }
                    else if (action == EnumRoles.System_Admin)
                    {
                        client.assignSystemAdmin(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                    }
                }
                else
                {
                    if (MessageBox.Show("Are you sure that you want to overwrite " + txtUserID.Text + "'s Role?",
                        "Confirm Role Change", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (action == EnumRoles.Event_Organizer)
                        {
                            client.assignEventOrganizer(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                        }
                        else if (action == EnumRoles.System_Admin)
                        {
                            client.assignSystemAdmin(user, txtUserID.Text.Trim(), txtDesc.Text.Trim());
                        }
                    }
                }
                MessageBox.Show("Role have been added/updated", "Updated Role",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                admFrame.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (txtAssn.Text.Trim().CompareTo(txtCurrRole.Text.Trim()) == 0)
            {
                MessageBox.Show("User " + txtUserID.Text + " has already been assigned as " + txtCurrRole.Text,
                    "Already Assigned", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                btnExit_Click(this.btnExit, new RoutedEventArgs());
            }
        }
    }
}