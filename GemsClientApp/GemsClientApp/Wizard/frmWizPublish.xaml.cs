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
    /// Interaction logic for frmWizPublish.xaml
    /// </summary>
    public partial class frmWizPublish : GemsWizPage
    {
        User user;
        Events event_;
        Publish publish;
        //Publish orginal_publish;

        public frmWizPublish(frmWizard c)
        {
            InitializeComponent();
            publish = c._publish;
            user = c._user;
            event_ = c._event;

            //publish.StartDateTime = orginal_publish.StartDateTime;
            //publish.Remarks = orginal_publish.Remarks;
            //publish.EndDateTime = orginal_publish.EndDateTime;
            //publish.IsPayable = orginal_publish.IsPayable;
            //publish.PaymentAMount = orginal_publish.PaymentAMount;


            if (publish.Remarks != null)
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


           

            //changed = false;
        }

        private void dateChanged(object sender, RoutedEventArgs e)
        {
           
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

        private void txtamount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void onChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            
            txtRemarks.Text = "";
            //dtpStart.clear();
            dtpStart.dtpDate.SelectedDate = DateTime.Now;
            //dtpEnd.clear();
            dtpEnd.dtpDate.SelectedDate = DateTime.Now;
            cboIsPayable.IsChecked = false;


            //changed = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dtpStart.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Invalid publish start date.",
                    "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
                //return false;
            }
            if (dtpEnd.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Invalid publish end date.",
                    "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
                //return false;
            }
            try
            {
                DateTime startTime = dtpStart.SelectedDateTime;
                DateTime endTime = dtpEnd.SelectedDateTime;

                if (endTime > event_.StartDateTime)
                {
                    MessageBox.Show("Event end at " + event_.EndDateTime + ", publish date must end before that.");
                    return;
                }
                if (endTime <= startTime)
                {
                    MessageBox.Show("Publish end date must be after its start date.");
                    return;
                }
                
                decimal result=0;

                if (cboIsPayable.IsChecked.Value && !decimal.TryParse(txtamount.Text, out result))
                {
                    MessageBox.Show("Invalid amount");
                    return;
                }
                //if (publish.Remarks == null)
                
                publish.StartDateTime = startTime;
                publish.EndDateTime = endTime;
                publish.Remarks = txtRemarks.Text;
                publish.IsPayable = cboIsPayable.IsChecked.Value;
                publish.PaymentAMount = result;
                    //client.EditPublish(user, event_.EventID, startTime, endTime, txtRemarks.Text, cboIsPayable.IsChecked.Value, result);
                //client.Close();
                btnDelete.IsEnabled = true;
                //changed = false;
                MessageBox.Show("Operation succeeded!");
                //return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //return false;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                publish.StartDateTime = Convert.ToDateTime("1/1/0001 12:00:00 AM");
                //publish.StartDateTime = DateTime.Now;
                publish.Remarks = null;
                publish.EndDateTime = DateTime.Now;
                publish.IsPayable = false;
                publish.PaymentAMount = 0;
                btnClear_Click(null, null);
                btnDelete.IsEnabled = false;
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void txtamount_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
