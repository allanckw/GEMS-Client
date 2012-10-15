using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
using System.Windows.Controls;
using System.Data;


namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmBookingFacilityPriority.xaml
    /// </summary>
    public partial class frmFacBookingDetails : Window
    {
        List<Facility> models;
        Events event_;
        EventDay eventDay_;
        User user;

        public frmFacBookingDetails()
        {
            this.InitializeComponent();

        }

        public frmFacBookingDetails(User u, Events e, EventDay d,List<Facility> m)
            : this()
        {
            this.user = u;
            this.models = m;
            this.event_ = e;
            this.eventDay_ = d;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgFacility.ItemsSource = models;
            dgFacility.AutoGenerateColumns = false;
            dgFacility.CanUserAddRows = false;
            cboAdd();

            dtpStart.cboHr.SelectionChanged += OnChanged;
            dtpStart.cboMin.SelectionChanged += OnChanged;

            dtpStart.SelectedDateTime = eventDay_.StartDateTime;

            loadDuration();
        }

        private void OnChanged(object sender, SelectionChangedEventArgs e)
        {
            loadDuration();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private bool saveRequest(DateTime start, DateTime end)
        {
            //Use threading to stop system from "Hanging" as it may take a long time to save
            //as a list of objects are sent over via SOAP
            bool success = false;
            System.Threading.Thread thread = new System.Threading.Thread(
                new System.Threading.ThreadStart(
                delegate()
                {
                    System.Windows.Threading.DispatcherOperation
                    dispatcherOp = this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(delegate()
                    {
                        try
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            MessageBox.Show("Please wait while we process your request...");

                            List<int> priorityList = new List<int>();

                            //To grab cbo selected value in datagrid
                            List<FacilityBookingRequestDetails> list = new List<FacilityBookingRequestDetails>();
                            for (int i = 0; i < dgFacility.Items.Count; i++)
                            {
                                Facility f = (Facility)dgFacility.Items[i];
                                DataGridRow row = dgFacility.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                                ComboBox cbo = dgFacility.Columns[0].GetCellContent(row) as ComboBox;

                                if (cbo.SelectedIndex == -1)
                                {
                                    MessageBox.Show("Please select the priority in all your selected venues!",
                                        "Invalid Input!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    return;
                                }

                                if (priorityList.Contains(cbo.SelectedIndex + 1))
                                {
                                    MessageBox.Show("You have facilities with the same priority!",
                                        "1 priority per facility", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    return;
                                }

                                FacilityBookingRequestDetails fbreqDetails = new FacilityBookingRequestDetails();
                                fbreqDetails.EventID = event_.EventID;
                                fbreqDetails.FacilityID = f.FacilityID;
                                fbreqDetails.Faculty = f.Faculty;
                                fbreqDetails.Priority = cbo.SelectedIndex + 1;
                                priorityList.Add(cbo.SelectedIndex + 1);

                                list.Add(fbreqDetails);
                            }


                            FacilityBookingsHelper client = new FacilityBookingsHelper();
                            success = client.AddFacilityBookingRequest(user, eventDay_, list[0].Faculty, start, end, list.ToArray());
                            client.Close();

                            if (success)
                            {
                                MessageBox.Show("Your request for the facilities booking is submitted,"
                                    + "please wait for an administrator to respond to your request",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            
                        }
                    }
                ));

                    //dispatcherOp.Completed += new EventHandler(dispatcherOp_Completed);
                    dispatcherOp.Completed += (s, e) => 
                    {
                        if (success)
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                            this.Close();
                        }
                    };

                }
            ));

            thread.Start();
            return success;
        }

        private void loadDuration()
        {
            cboBookDuration.Items.Clear();
            int hr;
            int.TryParse(dtpStart.cboHr.SelectedValue.ToString(), out hr);
            int min;
            int.TryParse(dtpStart.cboMin.SelectedValue.ToString(), out min);
            int maxIdx = (24 - hr) * 2;
            if (min > 0)
            {
                maxIdx -= 1;
            }

            if (maxIdx < 1)
            {
                cboBookDuration.IsEnabled = false;
                btnSubmit.IsEnabled = false;
                return;
            }

            //TimeSpan duration = new TimeSpan(0,0,0);

            for (int i = 0; i <= maxIdx; i++)
            {
                TimeSpan duration = new TimeSpan(0, i * 30, 0);
                if (i == 48)
                    cboBookDuration.Items.Add(string.Format("{0:00}", 24) + " H " + string.Format("{0:00}", 0) + " Min");
                else
                    cboBookDuration.Items.Add(string.Format("{0:00}", duration.Hours) + " H " + string.Format("{0:00}", duration.Minutes) + " Min");

                //duration.Add(new TimeSpan(0, 30, 0));
            }
            cboBookDuration.Items.RemoveAt(0);
            cboBookDuration.SelectedIndex = 0;
            btnSubmit.IsEnabled = true;
            cboBookDuration.IsEnabled = true;
        }

        void dispatcherOp_Completed(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           
        }

        public void cboAdd()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.dgFacility.Items.Count; i++)
            {
                list.Add(i + 1);
            }
            cboPriority.ItemsSource = list;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (dtpStart.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Please select the Dates", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            DateTime start = dtpStart.SelectedDateTime;
            DateTime end = getEndDateTime();


            FacilityBookingsHelper client = new FacilityBookingsHelper();
            bool exist = client.CheckRequestExist(event_.EventID, start ,end);
            client.Close();

            if (exist)
            {
                MessageBox.Show("The event already have a pending or confirmed request at the selected time frame!",
                    "Request already Exist", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                bool success = saveRequest(start, end);
                if (success)
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DateTime getEndDateTime()
        {
            int i = cboBookDuration.SelectedIndex + 1;
            TimeSpan duration = new TimeSpan(0, i * 30, 0);
            DateTime bookingEndDateTime = dtpStart.SelectedDateTime.Add(duration);
            return bookingEndDateTime;
        }

           private void cboBookDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime start = dtpStart.SelectedDateTime;

            DateTime end = getEndDateTime();
            //if (end == start)
            //{
            //    MessageBox.Show("End datetime must be later than start datetime");
            //    return;
            //}
            this.txtEndTime.Text = end.ToString("dd MMM yyyy HH:mm");
        }
    }
}