using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
using System.Windows.Controls;
using System.Data;
using System.Windows.Media;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmBookingFacilityPriority.xaml
    /// </summary>
    public partial class frmFacBookingDetails : Window
    {
        List<Facility> models;
        Event event_;
        User user;
        public frmFacBookingDetails()
        {
            this.InitializeComponent();
            CreateDTPData();
        }

        public frmFacBookingDetails(User u, Event e, List<Facility> m)
            : this()
        {
            this.user = u;
            this.models = m;
            this.event_ = e;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgFacility.ItemsSource = models;
            dgFacility.AutoGenerateColumns = false;
            dgFacility.CanUserAddRows = false;
            cboAdd();

            dtpStart.DisplayDateStart = DateTime.Now;
            dtpEnd.DisplayDateStart = DateTime.Now;

            dtpStart.SelectedDate = event_.StartDateTime;
            dtpEnd.SelectedDate = event_.EndDateTime;

            cboStartHr.SelectedValue = event_.StartDateTime.Hour;
            cboEndHr.SelectedValue = event_.EndDateTime.Hour;
            cboStartMin.SelectedValue = event_.StartDateTime.Minute;
            cboEndMin.SelectedValue = event_.EndDateTime.Minute;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //DTP preprocessing
        public void CreateDTPData()
        {
            for (int i = 0; i <= 23; i++)
            {
                cboStartHr.Items.Add(string.Format("{0:00}", i));
                cboEndHr.Items.Add(string.Format("{0:00}", i));
            }

            for (int i = 0; i <= 55; i += 30)
            {
                cboStartMin.Items.Add(string.Format("{0:00}", i));
                cboEndMin.Items.Add(string.Format("{0:00}", i));
            }
            cboStartHr.SelectedIndex = cboStartMin.SelectedIndex = cboEndHr.SelectedIndex = cboEndMin.SelectedIndex = 0;
        }

        private void saveRequest(DateTime start, DateTime end)
        {
            List<int> priorityList = new List<int>();

            //To grab cbo selected value in datagrid
            List<FacilityBookingRequestDetails> list = new List<FacilityBookingRequestDetails>();
            for (int i = 0; i < dgFacility.Items.Count; i++)
            {
                Facility f = (Facility)dgFacility.Items[i];
                DataGridRow row = dgFacility.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                ComboBox cbo = dgFacility.Columns[0].GetCellContent(row) as ComboBox;

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


            WCFHelperClient client = new WCFHelperClient();
            bool success = client.addFacilityBookingRequest(user, event_, list[0].Faculty, start, end, list.ToArray());
            client.Close();

            if (success)
            {
                MessageBox.Show("Your request for the facilities booking is submitted, please wait for an administrator to respond to your request",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }

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
            if ((dtpEnd.SelectedDate == null) || (dtpStart.SelectedDate == null))
            {
                MessageBox.Show("Please select the Dates", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            DateTime start = (DateTime)dtpStart.SelectedDate;
            start = start.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
            start = start.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));

            DateTime end = (DateTime)dtpEnd.SelectedDate;
            end = end.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));
            end = end.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));

            if (end < start)
            {
                MessageBox.Show("Start date cannot be later than end date", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            try
            {
                saveRequest(start, end);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}