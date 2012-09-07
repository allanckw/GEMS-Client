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
	public partial class frmBudgetReport
	{
        User user;
        Events event_;
        List<BudgetIncome> incomeList;
        List<BudgetItemState> budgetItemList;
        Decimal income;
        Decimal expenditure;

        public frmBudgetReport()
        {
            InitializeComponent();
        }

        public frmBudgetReport(User user, Events e)
            : this()
        {
            this.user = user;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadReport();
        }

        private void loadReport()
        {
            loadIncome();
            loadBudgetItems();
            Decimal nett = income - expenditure;
            txtTotalNett.Text = "$" + nett.ToString("0.00");
        }

        private void loadIncome()
        {
            BudgetHelper client = new BudgetHelper();
            try
            {
                decimal totalAmount = 0;
                incomeList = client.ViewBudgetIncome(user, event_.EventID).ToList<BudgetIncome>();
                lvBudgetIncome.ItemsSource = incomeList;
                foreach (BudgetIncome item in incomeList)
                {
                    totalAmount += item.IncomeAfterGST;
                }
                //.ItemsSource = incomeList;
                this.txtTotalIncome.Text = "$" + totalAmount.ToString("0.00");
                income = totalAmount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        private void loadBudgetItems()
        {
            budgetItemList = new List<BudgetItemState>();
            BudgetHelper client = new BudgetHelper();
            try
            {
                decimal totalAmount = 0;
                OptimizedBudgetItems budgetItems = client.GetBudgetItem(event_.EventID);
                if (budgetItems != null)
                {
                    List<OptimizedBudgetItemsDetails> budgetItemsList = budgetItems.BudgetItemsList.ToList<OptimizedBudgetItemsDetails>();

                    foreach (OptimizedBudgetItemsDetails budgetItem in budgetItemsList)
                    {
                        Items item = client.GetItemDetail(budgetItem);
                        BudgetItemState bis = new BudgetItemState(item, budgetItem.IsBought);
                        if (budgetItem.IsBought)
                        {
                            totalAmount += item.ActualPrice;
                        }
                        else
                        {
                            item.ActualPrice = 0;
                            totalAmount += item.EstimatedPrice;
                        }

                        budgetItemList.Add(bis);
                    }

                    lvBudgetItem.ItemsSource = budgetItemList;
                }
                //.ItemsSource = incomeList;
                txtTotalExpenditure.Text = "$" + totalAmount.ToString("0.00");
                expenditure = totalAmount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }
	}
}