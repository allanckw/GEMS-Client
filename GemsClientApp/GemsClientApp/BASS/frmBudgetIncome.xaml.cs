using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for BudgetIncome.xaml
    /// </summary>
    public partial class frmBudgetIncome : Page
    {

        User user;
        Event event_;
        bool changed = false;
        int selectedIndex = -1;
        List<BudgetIncome> incomeList;

        public frmBudgetIncome()
        {
            InitializeComponent();
        }



        public frmBudgetIncome(User user, Event e)
            : this()
        {
            this.user = user;
            this.event_ = e;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadIncome();
        }

        private void loadIncome()
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                txtGstPercent.Text = client.getGST().ToString();
                incomeList = client.ViewBudgetIncome(user, event_.EventID).ToList<BudgetIncome>();
                lstIncomeList.ItemsSource = incomeList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
            clearAll();
        }

        private void onTextChanged(object sender, TextChangedEventArgs e)
        {
            changed = true;

        }

        //TODO: Save Changes and Test Method!
        private bool saveChanges()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the income's name.", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (txtIncb4Gst.Text.Trim().Length == 0)
            {

                MessageBox.Show("Income must be numeric!", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            //try
            //{
            //    WCFHelperClient client = new WCFHelperClient();
            //    if (selectedIndex == -1)
            //    {
            //        BudgetIncome bIncome = new BudgetIncome();
            //        bIncome.Name = txtName.Text;
            //        bIncome.Description = txtDescription.Text;

            //        client.AddGuest(user, event_.EventID, bIncome.Name, bIncome.Contact, bIncome.Description);
            //        incomeList.Add(bIncome);
            //        CollectionViewSource.GetDefaultView(lstIncomeList.ItemsSource).Refresh();
            //        clearAll();
            //    }
            //    else
            //    {
            //        BudgetIncome bIncome = incomeList[selectedIndex];
            //        bIncome.Name = txtName.Text;
            //        bIncome.Description = txtDescription.Text;

            //        client.EditGuest(user, bIncome.GuestId, bIncome.Name, bIncome.Description, bIncome.Contact);
            //        incomeList[selectedIndex] = bIncome;
            //        CollectionViewSource.GetDefaultView(lstIncomeList.ItemsSource).Refresh();
            //        changed = false;
            //    }
            //    client.Close();

            //    MessageBox.Show("Operation succeeded!");
            //    return true;

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return false;
            //}
            return true;
        }

        private void lstIncomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstIncomeList.SelectedIndex == -1)
            {
                selectedIndex = -1;
                return;
            }
            if (selectedIndex == lstIncomeList.SelectedIndex)
                return;
            if (changed)
            {
                MessageBoxResult answer = MessageBox.Show("There are unsaved changes. Would you like to save your changes now?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if ((answer == MessageBoxResult.Yes && !saveChanges()) || answer == MessageBoxResult.Cancel)
                {
                    lstIncomeList.SelectedIndex = selectedIndex;
                    return;
                }
            }
            BudgetIncome bIncome = (BudgetIncome)lstIncomeList.SelectedItem;
            txtName.Text = bIncome.Name;

            txtDescription.Text = bIncome.Description;
            btnAdd.Content = "Save";
            changed = false;
            selectedIndex = lstIncomeList.SelectedIndex;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstIncomeList.SelectedIndex == -1)
                return;
            try
            {
                BudgetIncome bIncome = (BudgetIncome)lstIncomeList.SelectedItem;
                WCFHelperClient client = new WCFHelperClient();
                client.DeleteBudgetIncome(user, bIncome.IncomeID, event_.EventID);
                client.Close();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadIncome();
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {
            clearAll();
        }

        private void clearAll()
        {
            lstIncomeList.SelectedIndex = -1;
            txtName.Text = "";
            txtDescription.Text = "";
            txtGst.Text = "";
            txtIncb4Gst.Text = "";
            txtSource.Text = "";
            btnAdd.Content = "Add";
            changed = false;
        }

        private void txtIncb4Gst_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtIncb4Gst.Text.Trim().Length == 0)
            {
                return;
            }
            decimal income;
            decimal gst = Decimal.Parse(txtGstPercent.Text.Trim()) / 100;
            decimal gstAmt = 0;
            if (Decimal.TryParse(txtIncb4Gst.Text.Trim(), out income))
            {
                if (chkGSTLiable.IsChecked.Value == true)
                {
                    gstAmt = gst * income;
                }
                txtGst.Text = gstAmt.ToString("0.00");
            }
            else
            {
                MessageBox.Show("Income must be numeric!", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
    }
}
