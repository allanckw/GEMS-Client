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
    /// Interaction logic for SelectedItemList.xaml
    /// </summary>
    public partial class frmBudgetItemList : GEMSPage
    {

        User user;
        Event event_;
        public frmBudgetItemList()
        {
            InitializeComponent();
        }

        public frmBudgetItemList(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;
            this.lvBItem.lv.SelectionChanged += lv_SelectionChanged;
            loadBudgetItems();
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvBItem.lv.SelectedIndex == -1)
            {
                clearBudgetItemForm();
                return;
            }

            OptimizedBudgetItemsDetails budgetItem = lvBItem.GetEditItem();
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                Items item2Edit = client.GetItemDetail(budgetItem);
                if (item2Edit != null)
                {
                    mapItem(item2Edit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }

        private void clearBudgetItemForm()
        {
            txtItemName.Text = "";
            txtItemType.Text = "";
            txtItemPrice.Text = "";
            txtItemSatisfaction.Text = "";
            txtItemActualPrice.Text = "";
        }

        private void mapItem(Items Item2Edit)
        {
            OptimizedBudgetItemsDetails budgetItem = lvBItem.GetEditItem();
            bool isbought = budgetItem.IsBought;
            txtItemName.Text = Item2Edit.ItemName;
            txtItemType.Text = Item2Edit.typeString;
            txtItemPrice.Text = Item2Edit.EstimatedPrice.ToString("0.00");
            txtItemSatisfaction.Text = Item2Edit.Satisfaction.ToString();
            if (isbought)
            {
                txtItemActualPrice.Text = Item2Edit.ActualPrice.ToString("0.00");
            }
            else
                txtItemActualPrice.Text = "N.A";
        }

        private void loadBudgetItems()
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                OptimizedBudgetItems budget = client.GetBudgetItem(event_.EventID);
                txtTotalPrice.Text = budget.TotalEstimatedPrice.ToString("0.00");
                txtTotalSat.Text = budget.TotalSatisfaction.ToString();
                txtGenDate.Text = budget.GeneratedDate.ToString("dd MMM yyyy");
                btnCancelEditItem.IsEnabled = btnUpdateItem.IsEnabled = true;
                //loadList
                lvBItem.SetExistingSource(budget.BudgetItemsList.ToList<OptimizedBudgetItemsDetails>());

            }
            catch (NullReferenceException)
            {
                txtTotalPrice.Text = "N.A";
                txtTotalSat.Text = "N.A";
                txtGenDate.Text = "N.A";
                btnCancelEditItem.IsEnabled = btnUpdateItem.IsEnabled = false;
                MessageBox.Show("There is no optimized list yet");

            }
            catch (Exception ex)
            {
                txtTotalPrice.Text = "N.A";
                txtTotalSat.Text = "N.A";
                txtGenDate.Text = "N.A";
                btnCancelEditItem.IsEnabled = btnUpdateItem.IsEnabled = false;
                MessageBox.Show("An error has occured: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }

        private void btnUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            decimal price;
            if (txtItemActualPrice.Text.Trim().CompareTo("N.A") == 0)
            {
                txtItemActualPrice.Text = "-1.00";
            }
            bool isdecimal = decimal.TryParse(txtItemActualPrice.Text, out price);
            if (!isdecimal)
            {
                MessageBox.Show("Invalid Actual Price");
                txtItemActualPrice.Text = "";
                txtItemActualPrice.Focus();
                return;
            }

            OptimizedBudgetItemsDetails budgetItem = lvBItem.GetEditItem();
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                Items item2Edit = client.GetItemDetail(budgetItem);
                client.UpdateActualPrice(user, item2Edit, price);
                clearBudgetItemForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();

            loadBudgetItems();
        }

        private void btnCancelEditItem_Click(object sender, RoutedEventArgs e)
        {
            lvBItem.lv.SelectedIndex = -1;
        }
    }
}
