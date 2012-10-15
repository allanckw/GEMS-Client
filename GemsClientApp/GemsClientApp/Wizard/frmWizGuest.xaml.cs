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
        private User user;
        private List<List<Guest>> guests;
        private List<List<Guest>> OrigGuests;
        private List<EventDay> _day;
        private Events _event;
        //List<Guest> guestList;
        //List<EventDay> _day;
        //EventDay eventDay_;
        //int selectedIndex = -1;

        public frmWizGuest(frmWizard c)
        {
            //_guests = c._guests;
            //_day = c._days;
            _event = c._event;
            user = c._user;
            OrigGuests = c._guests;
            _day = c._days;

            InitializeComponent();
            
            
            guests = new List<List<Guest>>();
            clone(OrigGuests, guests);
           
          
        }

        private void clone(List<List<Guest>> g1, List<List<Guest>> g2)
        {
            g2.Clear();

            for (int x = 0; x < g1.Count; x++)
            {


                List<Guest> lstguest = new List<Guest>();

                for (int y = 0; y < g1[x].Count; y++)
                {
                    Guest g = new Guest();

                    g.Description = g1[x][y].Description;
                    g.Name = g1[x][y].Name;
                    g.Contact = g1[x][y].Contact;
                    

                    lstguest.Add(g);
                }

                g2.Add(lstguest);
            }
        }

        private void loadExisting(int intDay)
        {

            lstGuestList.Items.Clear();
            //for (int i = 0; i < guests.Count(); i++)
            //{
                List<Guest> guest = guests[intDay];

                for (int x = 0; x < guest.Count(); x++)
                {
                    Guest g = guest[x];
                    lstGuestList.Items.Add(g);
                }
            //}
        }

        private void PopulateDays()
        {
            
            cboDay.Items.Clear();

            for (int i = 0; i < _day.Count; i++)
            {

                cboDay.Items.Add(_day[i].DayNumber);
                //cboDay.Items[cboDay.Items.Count-1]
            }

            cboDay.SelectedIndex = 0;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //lstGuestList.SelectedValuePath = "GuestId";
            clearAll();


            PopulateDays();
            loadExisting(cboDay.SelectedIndex);
            txtName.Focus();

            //loadGuests();
        }

        private void lstGuestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGuestList.SelectedIndex == -1)
            {
                //selectedIndex = -1;
                return;
            }
            //if (selectedIndex == lstGuestList.SelectedIndex)
            //    return;
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
            //selectedIndex = lstGuestList.SelectedIndex;
        }

        private void loadGuests()
        {
            try
            {
                //GuestHelper client = new GuestHelper();
                //guestList = client.ViewGuest(eventDay_.DayID).ToList<Guest>();
                //lstGuestList.ItemsSource = guestList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            clearAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter guest's name.");
                return;
            }
            try
            {
                //GuestHelper client = new GuestHelper();

                // adds guest info to listview and clear all info in textboxes
                if (lstGuestList.SelectedIndex == -1)
                {
                    Guest g = new Guest();
                    g.Name = txtName.Text;
                    g.Contact = txtContact.Text;
                    g.Description = txtDescription.Text;
                    //g.GuestId = client.AddGuest(user, eventDay_.DayID, g.Name, g.Contact, g.Description);
                    //guestList.Add(g);
                    //CollectionViewSource.GetDefaultView(lstGuestList.ItemsSource).Refresh();
                    lstGuestList.Items.Add(g);

                    guests[cboDay.SelectedIndex].Add(g);
                    //List<Guest> guestList = new List<Guest>();
                    //guestList.Add(g);

                        //for (int i = 0; i < guests.Count(); i++)
                        //{
                        //    List<Guest> guest = guests[i];
                        //    //List<Guest> guest;
                        //    guest.Add(g);
                        //    guests.Add(guest);

                    //guests.Add(guestList);
                    //guests.Add(
                    clearAll();
                }
                else
                {
                    Guest g = (Guest)lstGuestList.SelectedItem;
                    g.Name = txtName.Text;
                    g.Description = txtDescription.Text;
                    g.Contact = txtContact.Text;
                    // to refresh the listview
                    lstGuestList.BeginInit();
                    lstGuestList.EndInit();

                    //client.EditGuest(user, g.GuestId, g.Name, g.Description, g.Contact);
                    //guestList[selectedIndex] = g;
                    //CollectionViewSource.GetDefaultView(lstGuestList.ItemsSource).Refresh();
                    //changed = false;
                }
                //client.Close();

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
            {
                MessageBox.Show("Please select a guest to delete.");
                return;
            }
            try
            {
                guests[cboDay.SelectedIndex].RemoveAt(lstGuestList.SelectedIndex);

                loadExisting(cboDay.SelectedIndex);
                clearAll();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //loadGuests();
        }

        private void clearAll()
        {
            lstGuestList.SelectedIndex = -1;
            txtName.Text = "";
            txtContact.Text = "";
            txtDescription.Text = "";
            btnAdd.Content = "Add";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
        }

        public override bool Save()
        {
            clone(guests, OrigGuests);


            return true;

            //guests.Clear();
            //List<Guest> guestList = new List<Guest>();
            //for (int i = 0; i < lstGuestList.Items.Count; i++)
            //{
            //    Guest g = (Guest)lstGuestList.Items[i];
            //    guestList.Add(g);

            //    //for (int i = 0; i < guests.Count(); i++)
            //    //{
            //    //    List<Guest> guest = guests[i];
            //    //    //List<Guest> guest;
            //    //    guest.Add(g);
            //    //    guests.Add(guest);

            //}

            //guests.Add(guestList);

            //return true;
        }

        private void cboDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboDay.SelectedIndex == -1)
            {
                lbldaydate.Content = "";
            }
            else
            {
                lbldaydate.Content = _event.StartDateTime.AddDays(cboDay.SelectedIndex).ToShortDateString();
                loadExisting(cboDay.SelectedIndex);
            }
            
        }

        private void GroupBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GemsWizPage_Loaded(object sender, RoutedEventArgs e)
        {

            PopulateDays();
            loadExisting(cboDay.SelectedIndex);
            txtName.Focus();

        }

        
    }
}
