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
    public partial class frmOverview : Page
    {
        User user;
        Event event_;
        public frmOverview()
        {
            InitializeComponent();
        }

        public frmOverview(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;
            if (e != null)
                loadEventItems();
        }

        private void loadEventItems()
        {
            try
            {
                loadFacilityBooking();
                loadGuests();
                loadPrograms();
                loadRoles();
                loadItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadGuests()
        {

            txtGuestMsg.Text = "There are a total of " + event_.Guests.Count() + " guests ";
        }

        private void loadPrograms()
        {
            if (event_.Programs.Count() == 0)
                txtProgramMsg.Text = "No Programmes Added Yet.";
            else
            {
                DateTime max = DateTime.MinValue, min = DateTime.MaxValue;
                for (int i = 0; i < event_.Programs.Count(); i++)
                {
                    if (max < event_.Programs[i].EndDateTime)
                    {
                        max = event_.Programs[i].EndDateTime;
                    }
                    if (min > event_.Programs[i].StartDateTime)
                    {
                        min = event_.Programs[i].StartDateTime;
                    }
                }

                txtProgramMsg.Text = "There are " + event_.Programs.Count() + " planned programmes from " + min.ToShortTimeString() + " to " + max.ToShortTimeString();

            }
        }

        private void loadRoles()
        {
            WCFHelperClient client = new WCFHelperClient();
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
            if (event_.ItemCount > 0)
            {
                txtItemMsg.Text = "A total of " + event_.ItemCount + " Item(s) have been added";
                //Add Budget Allocation, after choice list is set
                //Check how many bought, and how many not bought.
            }
            else
            {
                txtItemMsg.Text = "No items have been added yet";
            }
        }
        private void loadFacilityBooking()
        {
            FacilityBookingConfirmed fbc = event_.ConfirmedFacilityBooking;
            if (fbc == null)
            {
                txtLocationMsg.Text = "The venue to hold the event is not confirmed yet";
            }
            else
            {
                txtLocationMsg.Text = "The venue to hold this event is in " +
                        fbc.Faculty.ToString() + " " + " at " + fbc.Venue + " " + Environment.NewLine
                        + Environment.NewLine +
                        "It is booked from " + fbc.RequestStartDateTime.ToString("dd MMM yyyy HH:mm")
                        + " to " + fbc.RequestEndDateTime.ToString("dd MMM yyyy HH:mm");


            }
        }

    }
}
