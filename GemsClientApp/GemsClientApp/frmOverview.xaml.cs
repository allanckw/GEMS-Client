using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmOverView.xaml
    /// </summary>
    public partial class frmOverView : GEMSPage
    {
        User user;
        Events event_;
        List<EventDay> DayList;

        public frmOverView()
        {
            InitializeComponent();
        }

        public frmOverView(User u, Events e)
            : this()
        {
            this.user = u;
            this.event_ = e;
            loadDays();

            if (e != null)// && (pEvent == null || pEvent.EventID != e.EventID))
            {
                //when evd and e are not null, pEvent not null or pEvent ID is not equal to EID
                loadEventItems();
            }
            //else if (e != null && evd != null && e.EventID == evd.EventID)
            //{
            //    //when both id are the same
            //    loadDayItems();
            //}

        }

        private void loadEventItems()
        {
            try
            {
                loadRoles();
                loadTasks();
                loadProgram();
                loadGuest();
                //loadItems();
                //loadDayItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadDays()
        {
            EventHelper clientEvent = new EventHelper();
            DayList = clientEvent.GetDays(event_.EventID).ToList<EventDay>();
            clientEvent.Close();

            cboSelectBookingDay.ItemsSource = DayList;
            cboSelectBookingDay.SelectedIndex = 0;
        }

        private void loadRoles()
        {
            RoleHelper client = new RoleHelper();

            try
            {
                List<RoleWithUser> RoleList = client.ViewEventRoles(user, event_).ToList<RoleWithUser>();
                if (RoleList.Count > 0)
                {
                    lblNoManpower.Visibility = Visibility.Collapsed;
                    lstRole.Visibility = Visibility.Visible;
                    lstRole.ItemsSource = RoleList;
                }
                else
                {
                    lblNoManpower.Visibility = Visibility.Visible;
                    lstRole.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private void loadTasks()
        {
            TasksHelper client = new TasksHelper();
            List<Task> TaskList= client.GetTasksByEvent(user.UserID, event_.EventID).ToList<Task>();
            client.Close();

            if (TaskList.Count > 0)
            {
                lblNoTask.Visibility = Visibility.Collapsed;
                lstTasks.Visibility = Visibility.Visible;
                lstTasks.ItemsSource = TaskList;
            }
            else
            {
                lblNoTask.Visibility = Visibility.Visible;
                lstTasks.Visibility = Visibility.Collapsed;
            }
        }

        private void loadProgram()
        {
            lstProgram.Items.Clear();
            ProgrammeHelper client = new ProgrammeHelper();
            List<int> countList = client.GetEventProgCount(event_.EventID).ToList<int>();
            client.Close();
            for (int i = 0; i < countList.Count; i++)
            {
                string tempResult = DayList[i].StartDateTime.ToShortDateString() + "\t\t-\t" + countList[i] + " programme(s)";
                lstProgram.Items.Add(tempResult);
            }
        }

        private void loadGuest()
        {
            lstGuest.Items.Clear();
            GuestHelper client = new GuestHelper();
            List<int> countList = client.GetEventGuestCount(event_.EventID).ToList<int>();
            client.Close();
            for (int i = 0; i < countList.Count; i++)
            {
                string tempResult = DayList[i].StartDateTime.ToShortDateString() + "\t\t-\t" + countList[i] + " guest(s)";
                lstGuest.Items.Add(tempResult);
            }
        }

        private void loadFacilities()
        {

        }

        private void lstGuest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGuest.SelectedIndex==-1)
            {
                return;
            }

            if (lstProgram.SelectedIndex != lstGuest.SelectedIndex)
            {
                lstProgram.SelectedIndex = lstGuest.SelectedIndex;
                lstProgram.SelectedItem = lstProgram.Items.GetItemAt(lstGuest.SelectedIndex);
                lstProgram.ScrollIntoView(lstProgram.Items[0]);
                lstProgram.ScrollIntoView(lstProgram.SelectedItem);
            }
        }

        private void lstProgram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstProgram.SelectedIndex == -1)
            {
                return;
            }

            if (lstGuest.SelectedIndex != lstProgram.SelectedIndex)
            {
                lstGuest.SelectedIndex = lstProgram.SelectedIndex;
                lstGuest.SelectedItem = lstGuest.Items.GetItemAt(lstProgram.SelectedIndex);
                lstGuest.ScrollIntoView(lstGuest.Items[0]);
                lstGuest.ScrollIntoView(lstGuest.SelectedItem);
            }
        }


        //private void loadDayItems()
        //{
        //    try
        //    {
        //        loadFacilityBooking();//Need To Remap
        //        loadGuests();//Need To Remap
        //        loadPrograms();//Need To Remap
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void loadGuests()
        //{
        //    GuestHelper client = new GuestHelper();
        //    //txtGuestMsg.Text = "There are a total of " + client.ViewGuest(eventday_.DayID).ToList<Guest>().Count + " guest(s) ";
        //    client.Close();
        //}

        //private void loadTasks()
        //{
        //    TasksHelper client = new TasksHelper();
        //    //txtTaskMsg.Text = "There are a total of " + client.GetEventTasks(this.event_.EventID).ToList<Task>().Count() + " task(s)";
        //    client.Close();
        //}

        //private void loadPrograms()
        //{
        //    ProgrammeHelper client = new ProgrammeHelper();
        //    //List<Program> progs = client.ViewProgram(this.eventday_.DayID).ToList<Program>();
        //    //string tempResult = "";
        //    //for (int i = 0; i < progs.Count(); i++)
        //    //{
        //    //     tempResult = _event.StartDateTime.AddDays(i).ToShortDateString() + "\t\t-\t" + _programs.ElementAt(i).Count() + " programme(s)";

        //    //    lstProgram.Items.Add(tempResult);
        //    //}
        //    client.Close();
        //}

        //private void loadRoles()
        //{
        //    RoleHelper client = new RoleHelper();
        //    List<Role> r = client.ViewRole(user, event_).ToList<Role>();
        //    try
        //    {
        //        lstRole.ItemsSource = client.ViewEventRoles(user, event_).ToList<RoleWithUser>();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        client.Close();
        //    }
        //}

        //private void loadItems()
        //{
        //    //EventItemsHelper client = new EventItemsHelper();
        //    //List<Items> iten = client.GetItemsByEvent(event_.EventID).ToList<Items>();
        //    //if (iten.Count > 0)
        //    //{
        //    //    txtItemMsg.Text = "A total of " + iten.Count + " Item(s) have been added";
        //    //}
        //    //else
        //    //{
        //    //    txtItemMsg.Text = "No items have been added yet";
        //    //}
        //}
        //private void loadFacilityBooking()
        //{//Need To Remap
        //    FacilityBookingsHelper client = new FacilityBookingsHelper();
        //    List<FacilityBookingConfirmed> fbc = client.GetConfirmedFacBookings(event_.EventID,
        //        eventday_.StartDateTime.Date).ToList<FacilityBookingConfirmed>();
        //    //eventday_.ConfirmedFacilityBooking.ToList<FacilityBookingConfirmed>();
        //    if (fbc.Count == 0)
        //    {
        //        txtLocationMsg.Text = "The venue to hold the event is not confirmed yet";
        //    }
        //    else
        //    {
        //        txtLocationMsg.Text = "The venue to hold this event are (is) " + Environment.NewLine;
        //        foreach (FacilityBookingConfirmed confirmedReq in fbc)
        //        {
        //            txtLocationMsg.Text += confirmedReq.Faculty.ToString().Replace('_', ' ') + " " + " at " + confirmedReq.Venue
        //                + "  booked from " + confirmedReq.RequestStartDateTime.ToString("HH:mm")
        //                + " to " + confirmedReq.RequestEndDateTime.ToString("HH:mm") + Environment.NewLine
        //                ;


        //        }
        //    }
        //}
    }
}
