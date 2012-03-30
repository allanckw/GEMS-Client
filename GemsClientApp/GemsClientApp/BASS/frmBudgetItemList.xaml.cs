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
    public partial class frmBudgetItemList : Page
    {
        //TODO:
        //@KAI Please fix
        //SelectedIndex Change and Coloring of bought items
        //I will go and figure out what you want with tasks allocation form 
        //Selected Index Change, 
        //call client.getItemDetail(BudgetItems bItem); to get item details
        //Saving of new price: 
        //call client.updateActualPrice(User user, Items iten, decimal price);
        //The above 2 server methods not tested, 
        //notify me if got problems, or maybe u can try fixing it urself lol

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
            loadBudgetItems();
        }

        private void loadBudgetItems()
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                Budget budget = client.getBudget(event_.EventID);
                txtTotalPrice.Text = budget.TotalEstimatedPrice.ToString("0.00");
                txtTotalSat.Text = budget.TotalSatisfaction.ToString();
                txtGenDate.Text = budget.GeneratedDate.ToString("dd MMM yyyy");
                btnCancelEditItem.IsEnabled = btnUpdateItem.IsEnabled = true;
                //loadList
                lvItem.SetExistingSource(budget.budgetItemsList.ToList<BudgetItems>());

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
    }
}
