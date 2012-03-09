using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmGuestList.xaml
    /// </summary>
    public partial class frmGuestList : Page
    {

        User user;
        Window mainFrame;
        Event event_;
        List<Guest> guestList;
        int selectedIndex = -1;
        bool changed = false;

        public frmGuestList()
        {
            this.InitializeComponent();
        }

        public frmGuestList(User u, frmMain f, Event e)
            : this()
        {
            this.user = u;
            this.mainFrame = f;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstGuestList.SelectedValuePath = "GuestId";
            loadGuests();
        }

        private void loadGuests()
        {
            try
            {
                guestList = event_.Guests.ToList<Guest>();
                lstGuestList.ItemsSource = guestList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            clearAll();
        }

        private bool saveChanges()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter guest's name.");
                return false;
            }
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                if (selectedIndex == -1)
                {
                    Guest g = new Guest();
                    g.Name = txtName.Text;
                    g.Description = txtDescription.Text;
                    g.Contact = txtContact.Text;
                    client.AddGuest(user, event_.EventID, g.Name, g.Contact, g.Description);
                    guestList.Add(g);
                    CollectionViewSource.GetDefaultView(lstGuestList.ItemsSource).Refresh();
                    clearAll();
                }
                else
                {
                    Guest g = guestList[selectedIndex];
                    g.Name = txtName.Text;
                    g.Description = txtDescription.Text;
                    g.Contact = txtContact.Text;
                    client.EditGuest(user, g.GuestId, g.Name, g.Description, g.Contact);
                    guestList[selectedIndex] = g;
                    CollectionViewSource.GetDefaultView(lstGuestList.ItemsSource).Refresh();
                    changed = false;
                }
                client.Close();
                return true;
                MessageBox.Show("Operation succeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
        }

        private void lstGuestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGuestList.SelectedIndex == -1)
            {
                selectedIndex = -1;
                return;
            }
            if (selectedIndex == lstGuestList.SelectedIndex)
                return;
            if (changed)
            {
                MessageBoxResult answer = MessageBox.Show("There are unsaved changes. Would you like to save your changes now?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if ((answer == MessageBoxResult.Yes && !saveChanges()) || answer == MessageBoxResult.Cancel)
                {
                    lstGuestList.SelectedIndex = selectedIndex;
                    return;
                }
            }
            Guest selectedGuest = (Guest)lstGuestList.SelectedItem;
            txtName.Text = selectedGuest.Name;
            txtContact.Text = selectedGuest.Contact;
            txtDescription.Text = selectedGuest.Description;
            btnAdd.Content = "Save";
            changed = false;
            selectedIndex = lstGuestList.SelectedIndex;
        }

        private void clearAll()
        {
            lstGuestList.SelectedIndex = -1;
            txtName.Text = "";
            txtContact.Text = "";
            txtDescription.Text = "";
            btnAdd.Content = "Add";
            changed = false;
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {
            clearAll();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstGuestList.SelectedIndex == -1)
                return;
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                client.DeleteGuest(user, (int)lstGuestList.SelectedValue);
                client.Close();
                MessageBox.Show("Operation succeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadGuests();
        }

        private void onTextChanged(object sender, TextChangedEventArgs e)
        {
            changed = true;
        }
    }
}
