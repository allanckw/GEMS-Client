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
    /// Interaction logic for frmWizSummary.xaml
    /// </summary>
    public partial class frmWizSummary : GemsWizPage
    {
        public Events _event;
        public List<EventDay> _days;
        public List<List<Program>> _programs;
        public List<Items> _items;
        public List<ItemTypes> _itemTypes;
        public List<List<Guest>> _guests;
        public Publish _publish;
        public List<Task> _task;

        //private User user;
        //private List<List<Guest>> guests;
        //private List<List<Guest>> OrigGuests;
        //private List<EventDay> _day;
        //private Events _event;
        //List<Guest> guestList;
        //List<EventDay> _day;
        //EventDay eventDay_;
        //int selectedIndex = -1;

        public frmWizSummary(frmWizard c)
        {
            //_guests = c._guests;
            //_day = c._days;
            _event = c._event;
            _days = c._days;
            //_programs = new List<List<Program>>();
            _programs = c._programs;
            _items = c._items;
            _itemTypes = new List<ItemTypes>();
            _guests = c._guests;
            //_guests = new List<List<Guest>>();
            _publish = c._publish;
            _task = c._task;

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //lstGuestList.SelectedValuePath = "GuestId";
            //clearAll();


            //PopulateDays();
            //loadExisting(cboDay.SelectedIndex);
            //txtName.Focus();

            //loadGuests();
        }

        public override bool Save()
        {


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

        private void GroupBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GemsWizPage_Loaded(object sender, RoutedEventArgs e)
        {

            //PopulateDays();
            //loadExisting(cboDay.SelectedIndex);
            //txtName.Focus();

            //lblEvtName.Content = "Event Name: " + _event.Name;
            //lblEvtStartDate.Content = "Event Start Date: " + _event.StartDateTime.ToString();
            //lblEvtEndDate.Content = "Event End Date: " + _event.EndDateTime.ToString();
            
            //txtTaskMsg.Text = "There are a total of " + _event.Tasks.Count() + " task(s)";
            LoadEventAndPublish();
            LoadProgrammes();
            LoadItems();
            LoadTasks();
            LoadGuests();
            //LoadPublish();
        }

        private void LoadEventAndPublish()
        {
            //txtEventMsg.Text = "Event Name: " + _event.Name + "\nDuration: " + _event.StartDateTime.ToString() + " to " + _event.EndDateTime.ToString();
            if (_publish.StartDateTime.Year == 1)
            {
                txtEventAndPublishMsg.Text = "Event Name: " + _event.Name + "\nEvent Duration: " + 
                    _event.StartDateTime.ToString() + " to " + _event.EndDateTime.ToString() + 
                    "\nRegistration Duration: -";
                //txtPublishMsg.Text = "No publish details have been added";
            }
            else
            {
                //txtPublishMsg.Text = "Registration Start Date: " + _publish.StartDateTime.ToString() +
                //    "\nRegistration End Date: " + _publish.EndDateTime.ToString();
                txtEventAndPublishMsg.Text = "Event Name: " + _event.Name + "\nEvent Duration: " + 
                    _event.StartDateTime.ToString() + " to " + _event.EndDateTime.ToString() + 
                    "\nRegistration Duration: " + _publish.StartDateTime.ToString() +
                    " to " + _publish.EndDateTime.ToString(); ;
            }
        }

        private void LoadProgrammes()
        {
            //if (_programs.ElementAt(0).Count() == 0)
            //{
            //    txtProgramMsg.Text = "No programmes have been added";
            //}
            //else
            //{
                string tempResult = "";
                for (int i = 0; i < _programs.Count(); i++)
                {
                    //tempResult = "Day " + _days.ElementAt(i).DayNumber + "\t\t-\t" +  _programs.ElementAt(i).Count() + " programme(s)";
                    tempResult = _event.StartDateTime.AddDays(i).ToShortDateString() +"\t\t-\t" + _programs.ElementAt(i).Count() + " programme(s)";
                    //_event.StartDateTime.AddDays(_days.ElementAt(i).DayNumber).ToShortDateString();

                    lstProgram.Items.Add(tempResult);
                }

                //txtProgramMsg.Text = tempResult.Remove(tempResult.LastIndexOf(','));
            //}
            //if (_programs.Count() == 0)
            //    txtProgramMsg.Text = "No programmes have been added";
            //else
            //{
            //    txtProgramMsg.Text = "A total of " + _programs.Count() + " planned programme(s) have been added";// from " + min.ToShortTimeString() + " to " + max.ToShortTimeString();
            //}

            
        }

        private void LoadItems()
        {
            if (_items.Count() == 0)
            {
                txtItemMsg.Text = "No items have been added";
            }
            else
            {
                txtItemMsg.Text = "A total of " + _items.Count() + " item(s) have been added";
            }
        }

        private void LoadTasks()
        {
            if (_task.Count() == 0)
            {
                txtTaskMsg.Text = "No tasks have been added";
            }
            else
            {
                txtTaskMsg.Text = "A total of " + _task.Count() + " task(s) have been added";
            }
        }

        private void LoadGuests()
        {

            //if (_guests.ElementAt(0).Count() == 0)           // change condition!!!
            //{
            //    txtGuestMsg.Text = "No guests have been added";
            //}
            //else
            //{
            string tempResult = "";
            for (int i = 0; i < _guests.Count(); i++)
            {
                tempResult = _event.StartDateTime.AddDays(i).ToShortDateString() + "\t\t-\t\t" + _guests.ElementAt(i).Count() + " guest(s)";
                lstGuest.Items.Add(tempResult);
            }

                //txtGuestMsg.Text = tempResult.Remove(tempResult.LastIndexOf(','));
            //}

            //if (_guests.Count() == 0)
            //{
            //    txtGuestMsg.Text = "No guests have been added";
            //}
            //else
            //{
            //    txtGuestMsg.Text = "A total of " + _guests.Count() + " guest(s) have been added";
            //}

            //txtGuestMsg.Text = "There are a total of " + eventday_.Guests.Count() + " guest(s) ";
        }

        //private void LoadPublish()
        //{
        //    if (_publish.StartDateTime.Year == 1)
        //    {
        //        txtPublishMsg.Text = "No publish details have been added";
        //    }
        //    else
        //    {
        //        txtPublishMsg.Text = "Registration Start Date: " + _publish.StartDateTime.ToString() +
        //            "\nRegistration End Date: " + _publish.EndDateTime.ToString();
        //    }
        //}
    }
}
