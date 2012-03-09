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
            loadEventItems();
        }

        private void loadEventItems()
        {
            FacilityBookingConfirmed fbc = event_.ConfirmedFacilityBooking;
            if (fbc == null)
            {
                lblLocationMsg.Content = "The venue to hold the event is not confirmed yet";
            }
            else
            {
                lblLocationMsg.Content = "The venue to hold this event is in " +
                        fbc.Faculty.ToString() + " " + " at " + fbc.Venue + " " + Environment.NewLine
                        + Environment.NewLine +
                        "It is booked from " + fbc.RequestStartDateTime.ToString("dd MMM yyyy HH:mm")
                        + " to " + fbc.RequestEndDateTime.ToString("dd MMM yyyy HH:mm");


            }

            //guest
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                //  List<Event> list = client.ViewEvent(user).ToList<Event>();
                int g_count = client.CountGuest(event_.EventID);
                lblGuestMsg.Content = "There are a total of " + g_count.ToString() + " guest";

                List<Program> p = client.ViewProgram(event_.EventID).ToList<Program>();

                if (p.Count == 0)
                    lblProgramMsg.Content = "No Programs Added Yet.";
                else
                {
                    DateTime max = DateTime.MinValue, min = DateTime.MaxValue;
                    for (int i = 0; i < p.Count; i++)
                    {
                        if (max < p[i].EndDateTime)
                        {
                            max = p[i].EndDateTime;
                        }
                        if (min > p[i].StartDateTime)
                        {
                            min = p[i].StartDateTime;
                        }
                    }

                    lblProgramMsg.Content = "There are " + p.Count + " items in the programs from " + min.ToShortTimeString() + " to " + max.ToShortTimeString();
                }

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

                lblManpowerMsg.Content = "";
                for (int i = 0; i < str_r.Count; i++)
                {
                    lblManpowerMsg.Content = r_count[i].ToString() + " " + str_r[i].ToString() + Environment.NewLine + lblManpowerMsg.Content;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
