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

            WCFHelperClient client = new WCFHelperClient();
            publish = client.ViewPublish(event_.EventID);
            client.Close();
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
        }

        public override bool saveChanges()
        {
            if (dtpStart.SelectedDateTime == null)
            {
                MessageBox.Show("Invalid publish start date.",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (dtpEnd.SelectedDateTime == null)
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
        }
    }
}
