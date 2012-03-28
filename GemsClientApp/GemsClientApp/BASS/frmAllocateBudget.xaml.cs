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
using System.Windows.Shapes;
using evmsService.entities;
using System.Collections;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmAllocateBudget.xaml
    /// </summary>
    public partial class frmAllocateBudget : Window
    {
        private User user;
        private Event event_;
        private List<Items> itemList;
        private List<ItemTypes> typeList;
        decimal maxBudget;
        BudgetAllocator bAllocator;

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
            //Use threading to stop system to "Hang" as it may take a long time to compute
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
                        bAllocator = new BudgetAllocator(itemList, typeList, maxBudget);
                    }
                ));

                    dispatcherOp.Completed += new EventHandler(dispatcherOp_Completed);
                }
   ));

            thread.Start();

        }

        void dispatcherOp_Completed(object sender, EventArgs e)
        {
            this.budgetSlider.Minimum = 0;
            int totalSatisfaction;
            decimal totalPrice;
            this.budgetSlider.Maximum = bAllocator.optimalItems
                (maxBudget, out totalPrice, out totalSatisfaction).Count-1;

            MessageBox.Show("Computation Completed!");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
