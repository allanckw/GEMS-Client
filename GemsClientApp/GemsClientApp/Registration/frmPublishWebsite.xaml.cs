using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Documents;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmPublishWebsite.xaml
    /// </summary>
    public partial class frmPublishWebsite : GEMSPage
    {
        User user;
        Event event_;
        Publish publish;

        public frmPublishWebsite()
        {
            InitializeComponent();
        }

        public frmPublishWebsite(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                publish = client.ViewPublish(event_.EventID);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (publish != null)
            {
                txtRemarks.Text = publish.Remarks;
                dtpStart.SelectedDateTime = publish.StartDateTime;
                dtpEnd.SelectedDateTime = publish.EndDateTime;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }

            dtpStart.dtpDate.SelectedDateChanged += dateChanged;
            dtpEnd.dtpDate.SelectedDateChanged += dateChanged;

            dtpStart.cboHr.SelectionChanged += dateChanged;
            dtpStart.cboMin.SelectionChanged += dateChanged;

            dtpEnd.cboHr.SelectionChanged += dateChanged;
            dtpEnd.cboMin.SelectionChanged += dateChanged;

            dtpStart.cboHr.SelectionChanged += starthourChanged;

            dtpEnd.cboHr.SelectionChanged += endhourChanged;

            changed = false;
        }

        private void starthourChanged(object sender, RoutedEventArgs e)
        {
            if (dtpStart.cboHr.SelectedValue.ToString().Equals("24"))
            {
                dtpStart.cboMin.SelectedIndex = 0;
                dtpStart.cboMin.IsEnabled = false;
            }
            else
                dtpStart.cboMin.IsEnabled = true;
        }

        private void endhourChanged(object sender, RoutedEventArgs e)
        {
            if (dtpEnd.cboHr.SelectedValue.ToString().Equals("24"))
            {
                dtpEnd.cboMin.SelectedIndex = 0;
                dtpEnd.cboMin.IsEnabled = false;
            }
            else
                dtpEnd.cboMin.IsEnabled = true;
        }

        private void dateChanged(object sender, RoutedEventArgs e)
        {
            if (publish != null)
            {
                DateTime start = dtpStart.dtpDate.SelectedDate.Value.AddHours(Double.Parse(dtpStart.cboHr.SelectedValue.ToString())).AddMinutes(Double.Parse(dtpStart.cboMin.SelectedValue.ToString()));
                DateTime end = dtpEnd.dtpDate.SelectedDate.Value.AddHours(Double.Parse(dtpEnd.cboHr.SelectedValue.ToString())).AddMinutes(Double.Parse(dtpEnd.cboMin.SelectedValue.ToString()));


                if (txtRemarks.Text.Equals(publish.Remarks) &&
                    start.Equals(publish.StartDateTime) &&
                    end.Equals(publish.EndDateTime))
                    changed = false;
                else
                    changed = true;
            }
            else
                changed = true;
        }
        public override bool saveChanges()
        {
            if (dtpStart.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Invalid publish start date.",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (dtpEnd.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Invalid publish end date.",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            try
            {
                DateTime startTime = dtpStart.SelectedDateTime;
                DateTime endTime = dtpEnd.SelectedDateTime;

                if (endTime > event_.StartDateTime)
                {
                    MessageBox.Show("Event starts at " + event_.EndDateTime + ", publish date must end before that.");
                    return false;
                }
                if (endTime <= startTime)
                {
                    MessageBox.Show("Publish end date must be after its start date.");
                    return false;
                }
                WCFHelperClient client = new WCFHelperClient();
                if (publish == null)
                    client.AddPublish(user, event_.EventID, startTime, endTime, txtRemarks.Text);
                else
                    client.EditPublish(user, event_.EventID, startTime, endTime, txtRemarks.Text);
                client.Close();
                btnDelete.IsEnabled = true;
                changed = false;
                MessageBox.Show("Operation succeeded!");
                return true;
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                client.DeletePublish(user, event_.EventID);
                client.Close();
                btnClear_Click(null, null);
                btnDelete.IsEnabled = false;
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtRemarks.Text = "";
            dtpStart.clear();
            dtpEnd.clear();
            changed = true;
        }
    }
}
