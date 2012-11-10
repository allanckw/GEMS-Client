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
        Events event_;
        Publish publish;

        public frmPublishWebsite()
        {
            InitializeComponent();
        }

        public frmPublishWebsite(User u, Events e)
            : this()
        {
            this.user = u;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistrationHelper client = new RegistrationHelper();
            try
            {

                publish = client.ViewPublish(event_.EventID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }

            if (publish != null)
            {
                txtRemarks.Text = publish.Remarks;
                dtpStart.SelectedDateTime = publish.StartDateTime;
                dtpEnd.SelectedDateTime = publish.EndDateTime;
                cboIsPayable.IsChecked = publish.IsPayable;
                txtamount.Text = publish.PaymentAMount.ToString();
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
            RegistrationHelper client = new RegistrationHelper(); 
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
                

                decimal result = 0;
                if (cboIsPayable.IsChecked == true)
                {
                    if (!decimal.TryParse(txtamount.Text, out result))
                    {
                        MessageBox.Show("invalid amount");
                        return false;
                    }
                }

                if (publish == null)
                {// need to change here
                    client.AddPublish(user, event_.EventID, startTime, endTime, txtRemarks.Text, cboIsPayable.IsChecked.Value, result);
                }
                else
                    client.EditPublish(user, event_.EventID, startTime, endTime, txtRemarks.Text, cboIsPayable.IsChecked.Value, result);

                btnDelete.IsEnabled = true;
                changed = false;
                MessageBox.Show("Operation succeeded!");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }finally{
                client.Close();
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            RegistrationHelper client = new RegistrationHelper();
            try
            {
                client.DeletePublish(user, event_.EventID);
                btnClear_Click(null, null);
                btnDelete.IsEnabled = false;
                MessageBox.Show("Operation succeeded!");
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            publish = null;
            txtRemarks.Text = "";
            //dtpStart.clear();
            dtpStart.dtpDate.SelectedDate = DateTime.Now;
            //dtpEnd.clear();
            dtpEnd.dtpDate.SelectedDate = DateTime.Now;
            cboIsPayable.IsChecked = false;


            changed = true;
        }

        private void cboIsPayable_Checked(object sender, RoutedEventArgs e)
        {
            txtamount.IsEnabled = true;

        }

        private void cboIsPayable_Unchecked(object sender, RoutedEventArgs e)
        {
            txtamount.Text = "0";
            txtamount.IsEnabled = false;
        }

        private void txtamount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void txtamount_LostFocus(object sender, RoutedEventArgs e)
        {
            //double result;
            //if (!double.TryParse(txtamount.Text, out result))
            //{
            //    MessageBox.Show("Please input a numeric number for amount");
            //    txtamount.Focus();
            //}
        }
    }
}
