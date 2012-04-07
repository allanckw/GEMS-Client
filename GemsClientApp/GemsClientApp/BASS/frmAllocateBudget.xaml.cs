using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmAllocateBudget.xaml
    /// </summary>
    // Slider Example
    //http://codingsense.wordpress.com/2010/02/01/customize-a-slider-in-wpf-step-by-step-tutorial/
    public partial class frmAllocateBudget : Window
    {
        private User user;
        private Event event_;
        private List<Items> itemList;
        private List<ItemTypes> typeList;
        decimal maxBudget;
        BudgetAllocator bAllocator;

        private List<List<Items>> results;

        public frmAllocateBudget()
        {
            InitializeComponent();
        }

        public frmAllocateBudget(User u, Event e,
            List<ItemTypes> typeList, List<Items> itemList, decimal maxBudget)
            : this()
        {
            this.user = u;
            this.event_ = e;
            this.typeList = typeList;
            this.itemList = itemList;
            this.maxBudget = maxBudget;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            txtMinBudget.Text = maxBudget.ToString("0.00");
            generateListOfSuitableItems(maxBudget);
        }

        private void generateListOfSuitableItems(decimal maxBudget)
        {

            //Use threading to stop system from "Hanging" as it may take a long time to compute
            //Due to Complexity
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
                            bAllocator = new BudgetAllocator(itemList, typeList, maxBudget);
                        }
                        catch (ArgumentOutOfRangeException argEx)
                        {
                            MessageBox.Show(argEx.Message, "Error", MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                            this.Close();
                        }
                    }
                ));

                    dispatcherOp.Completed += new EventHandler(optimalListGeneration_Completed);
                }
            ));

            thread.Start();
        }


        void optimalListGeneration_Completed(object sender, EventArgs e)
        {
            if (bAllocator == null)
                return;

            this.budgetSlider.Minimum = 0;
            int totalSatisfaction =0;
            decimal totalPrice =0;
            try
            {
                results = bAllocator.optimalItems(maxBudget);
                this.budgetSlider.Maximum = results.Count - 1;
                foreach (Items i in results[0])
                {
                    totalSatisfaction += i.Satisfaction;
                    totalPrice += i.EstimatedPrice;
                }

                txtTotalPrice.Text = totalPrice.ToString("0.00");
                txtTotalSat.Text = totalSatisfaction.ToString();

                MessageBox.Show("Computation Completed!");
                lstItemList.ItemsSource = results[0];
            }
            catch (ArgumentOutOfRangeException argEx)
            {
                MessageBox.Show(argEx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnCompute_Click(object sender, RoutedEventArgs e)
        {
            decimal minBudget = 0;

            if (Decimal.TryParse(txtMinBudget.Text.Trim(), out minBudget))
            {
                int totalSat = 0;
                decimal totalPrice = 0;
                try
                {
                    results = bAllocator.optimalItems(minBudget);
                    foreach (Items i in results[0])
                    {
                        totalSat += i.Satisfaction;
                        totalPrice += i.EstimatedPrice;
                    }
                    txtTotalPrice.Text = totalPrice.ToString("0.00");
                    txtTotalSat.Text = totalSat.ToString();
                    this.budgetSlider.Maximum = results.Count - 1;
                    MessageBox.Show("Computation Completed!");
                    lstItemList.ItemsSource = results[0];
                }
                catch (ArgumentOutOfRangeException argEx)
                {
                    this.budgetSlider.Maximum = 0;
                    txtTotalPrice.Text = "";
                    txtTotalSat.Text = "";
                    lstItemList.ItemsSource = null;
                    MessageBox.Show(argEx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Minimum budget must be numeric!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void budgetSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = Convert.ToInt32(budgetSlider.Value);
            lstItemList.ItemsSource = results[index];
            int totalSat = 0;
            decimal totalPrice = 0;

            foreach (Items i in results[index])
            {
                totalSat += i.Satisfaction;
                totalPrice += i.EstimatedPrice;
            }

            txtTotalPrice.Text = totalPrice.ToString("0.00");
            txtTotalSat.Text = totalSat.ToString();

        }

        private void saveOptimalList()
        {
            //Use threading to stop system from "Hanging" as it may take a long time to save
            //as a list of objects are sent over via SOAP
            System.Threading.Thread thread = new System.Threading.Thread(
                new System.Threading.ThreadStart(
                delegate()
                {
                    System.Windows.Threading.DispatcherOperation
                    dispatcherOp = this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(delegate()
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        MessageBox.Show("Please wait while we save your desired optimal list...");

                        WCFHelperClient client = new WCFHelperClient();
                        try
                        {
                            int sat = int.Parse(txtTotalSat.Text.Trim());
                            decimal price = decimal.Parse(txtTotalPrice.Text.Trim());
                            client.SaveBudgetList(user, event_.EventID, sat, price,
                                ((List<Items>)lstItemList.ItemsSource).ToArray());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An Expection has occured: " + ex.Message, "Exception while saving optimal item list",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        client.Close();
                    }
                ));

                    dispatcherOp.Completed += new EventHandler(saveOptimalList_Completed);
                }
            ));

            thread.Start();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            OptimizedBudgetItems budget = client.GetBudgetItem(event_.EventID);
            client.Close();

            if (budget != null)
            {
                if (MessageBox.Show("You already have an optimized item list, all previous changes will be overwritten" +
                Environment.NewLine + Environment.NewLine +
                "Do you want to overwrite? (It cannot be undone)", "Confirm Overwrite",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    save();
                }
                else
                {
                    return;
                }
            }
            else
            {
                save();
            }

            
        }

        private void save()
        {
            try
            {
                saveOptimalList();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void saveOptimalList_Completed(object sender, EventArgs e)
        {
            MessageBox.Show("Your Optimal List of Items have been saved", 
                "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            Mouse.OverrideCursor = Cursors.Arrow;
            this.Close();
        }
    }
}
