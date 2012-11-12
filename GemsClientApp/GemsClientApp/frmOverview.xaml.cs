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
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class frmOverview : GEMSPage
    {
        User user;
        Events event_;
        EventDay eventday_;

        public frmOverview()
        {
            InitializeComponent();
        }

        public frmOverview(User u, Events e, EventDay evd, Events pEvent)
            : this()
        {
            this.user = u;
            this.event_ = e;
            this.eventday_ = evd;
            if (e != null && evd != null && e.EventID != evd.EventID)
                loadEventItems();
            else if (e != null && evd != null && e.EventID == evd.EventID)
                loadDayItems();
        }

        private void loadEventItems()
        {
            try
            {
                loadFacilityBooking();//Need To Remap
                loadGuests();//Need To Remap
                loadPrograms();//Need To Remap
                loadDayItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadDayItems()
        {
            try
            {
                loadFacilityBooking();//Need To Remap
                loadGuests();//Need To Remap
                loadPrograms();//Need To Remap
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadGuests()
        {
            GuestHelper client = new GuestHelper();
            txtGuestMsg.Text = "There are a total of " + client.ViewGuest(eventday_.DayID).ToList<Guest>().Count +" guest(s) ";
            client.Close();
        }

        private void loadTasks()
        {
            TasksHelper client = new TasksHelper();
            txtTaskMsg.Text = "There are a total of " + client.GetEventTasks(this.event_.EventID).ToList<Task>().Count() + " task(s)";
            client.Close();
        }

        private void loadPrograms()
        {
            ProgrammeHelper client = new ProgrammeHelper();
            List<Program> progs = client.ViewProgram(this.eventday_.DayID).ToList<Program>();

            if (progs.Count() == 0)
                txtProgramMsg.Text = "No Programmes Added Yet.";
            else
            {
                txtProgramMsg.Text = "There are " + progs.Count + " planned programmes";// from " + min.ToShortTimeString() + " to " + max.ToShortTimeString();
            }
            client.Close();
        }

        private void loadRoles()
        {
            RoleHelper client = new RoleHelper();
            List<Role> r = client.ViewRole(user, event_).ToList<Role>();
            client.Close();
            List<string> str_r = new List<string>();
            for (int j = 0; j < r.Count; j++)
            {
                str_r.Add(r[j].Post);
            }
            str_r = str_r.Distinct<string>().ToList<string>();
            int[] r_count = new int[str_r.Count];

            for (int k = 0; k < str_r.Count; k++)
            {
                for (int l = 0; l < r.Count; l++)
                {
                    if (str_r[k] == r[l].Post)
                    {
                        r_count[k]++;
                    }
                }
            }

            txtManpowerMsg.Text = "";
            if (str_r.Count > 0)
            {
                for (int i = 0; i < str_r.Count; i++)
                {
                    txtManpowerMsg.Text = r_count[i].ToString() + " " + str_r[i].ToString() + Environment.NewLine + txtManpowerMsg.Text;
                }
            }
            else
            {
                txtManpowerMsg.Text = "No Manpower have been added yet";
            }


        }

        private void loadItems()
        {
            EventItemsHelper client = new EventItemsHelper();
            List<Items> iten = client.GetItemsByEvent(event_.EventID).ToList<Items>();
            if (iten.Count > 0)
            {
                txtItemMsg.Text = "A total of " + iten.Count + " Item(s) have been added";
            }
            else
            {
                txtItemMsg.Text = "No items have been added yet";
            }
        }
        private void loadFacilityBooking()
        {//Need To Remap
            FacilityBookingsHelper client = new FacilityBookingsHelper();
            List<FacilityBookingConfirmed> fbc = client.GetConfirmedFacBookings(event_.EventID, 
                eventday_.StartDateTime.Date).ToList<FacilityBookingConfirmed>();
                //eventday_.ConfirmedFacilityBooking.ToList<FacilityBookingConfirmed>();
            if (fbc.Count == 0)
            {
                txtLocationMsg.Text = "The venue to hold the event is not confirmed yet";
            }
            else
            {
                txtLocationMsg.Text = "The venue to hold this event are (is) " + Environment.NewLine;
                foreach (FacilityBookingConfirmed confirmedReq in fbc)
                {
                    txtLocationMsg.Text += confirmedReq.Faculty.ToString().Replace('_', ' ') + " " + " at " + confirmedReq.Venue
                        + "  booked from " + confirmedReq.RequestStartDateTime.ToString("HH:mm")
                        + " to " + confirmedReq.RequestEndDateTime.ToString("HH:mm") + Environment.NewLine
                        ;


                }
            }
        }

    }
}
