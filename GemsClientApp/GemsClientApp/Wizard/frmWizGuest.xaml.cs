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
using System.Windows.Navigation;
using System.Windows.Shapes;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmWizGuest.xaml
    /// </summary>
    public partial class frmWizGuest : GemsWizPage
    {
        User user;
        //List<List<Guest>> _guests;
        List<Guest> guestList;
        //List<EventDay> _day;
        EventDay eventDay_;
        int selectedIndex = -1;

        public frmWizGuest(frmWizard c)
        {
            //_guests = c._guests;
            //_day = c._days;

            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstGuestList.SelectedValuePath = "GuestId";
            loadGuests();
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
            //if (changed)
            //{
            //    MessageBoxResult answer = MessageBox.Show("There are unsaved changes. Would you like to save your changes now?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //    if ((answer == MessageBoxResult.Yes && !saveChanges()) || answer == MessageBoxResult.Cancel)
            //    {
            //        lstGuestList.SelectedIndex = selectedIndex;
            //        return;
            //    }
            //}
            Guest selectedGuest = (Guest)lstGuestList.SelectedItem;
            txtName.Text = selectedGuest.Name;
            txtContact.Text = selectedGuest.Contact;
            txtDescription.Text = selectedGuest.Description;
            btnAdd.Content = "Save";
            //changed = false;
            selectedIndex = lstGuestList.SelectedIndex;
        }

        private void onChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void loadGuests()
        {
            try
            {
                GuestHelper client = new GuestHelper();
                guestList = client.ViewGuest(eventDay_.DayID).ToList<Guest>();
                lstGuestList.ItemsSource = guestList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            clearAll();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter guest's name.");
                return;
            }
            try
            {
                GuestHelper client = new GuestHelper();
                if (selectedIndex == -1)
                {
                    Guest g = new Guest();
                    g.Name = txtName.Text;
                    g.Description = txtDescription.Text;
                    g.Contact = txtContact.Text;
                    g.GuestId = client.AddGuest(user, eventDay_.DayID, g.Name, g.Contact, g.Description);
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
                    //changed = false;
                }
                client.Close();

                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstGuestList.SelectedIndex == -1)
                return;
            try
            {
                GuestHelper client = new GuestHelper();
                client.DeleteGuest(user, (int)lstGuestList.SelectedValue);
                client.Close();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadGuests();
        }

        private void clearAll()
        {
            lstGuestList.SelectedIndex = -1;
            txtName.Text = "";
            txtContact.Text = "";
            txtDescription.Text = "";
            //btnAdd.Content = "Add";
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {
            clearAll();
        }
    }
}
