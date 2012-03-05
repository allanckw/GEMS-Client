using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmItemManagement.xaml
    /// </summary>
    public partial class frmItemManagement : Window
    {
        private Window mainFrame;
        private User user;
        private Event event_;
        private ItemTypes itemtype;
        
        public frmItemManagement()
        {
            this.InitializeComponent();
        }

        public frmItemManagement(User u, Event e, Window f)
            : this()
        {
            this.user = u;
            this.event_ = e;
            this.mainFrame = f;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            radItemType.IsChecked = true;
            refreshItemTypes();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }


        private void refreshItemTypes()
        {
            WCFHelperClient client = new WCFHelperClient();
            cboItemType.ItemsSource = client.getItemsTypes();
            client.Close();
        }

        private void btnAddItemType_Click(object sender, RoutedEventArgs e)
        {
            String itemType2Add = "";
            if (radItemType.IsChecked == true)
            {

                itemType2Add = cboItemType.SelectedItem.ToString();
            }
            else
            {
                //Save to item type repository here
                itemType2Add = txtOthers.Text.ToString().Trim();
                WCFHelperClient client = new WCFHelperClient();
                client.addItemsTypes(itemType2Add);
                client.Close();
                refreshItemTypes();
            }
            lvItemType.AddNewItemType(user,event_ ,itemType2Add, (bool)chkNecessary.IsChecked);
            rebindcboItemType4Item();
            clear();
        }

        private void rebindcboItemType4Item()
        {
            cboItemTypeIL.ItemsSource = lvItemType.GetItemTypeList();
        }

        private void clear()
        {
            txtOthers.Text = "";
            cboItemType.SelectedIndex = -1;
        }

        private void btnDeleteItemType_Click(object sender, RoutedEventArgs e)
        {
            lvItemType.DeleteItemType(user,event_);
        }

        private void cboItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            radItemType.IsChecked = true;
        }

        private void txtOthers_TextChanged(object sender, TextChangedEventArgs e)
        {
            radOthers.IsChecked = true;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (validateInput())
            {
                double price;
                bool isDouble = double.TryParse(txtItemPrice.Text, out price);
                if (!isDouble)
                {
                    MessageBox.Show("Invalid Price");
                    return;
                }
                int satisfactionValue;
                bool isInt = int.TryParse(txtItemSatisfaction.Text, out satisfactionValue);
                if (!isInt)
                {
                    MessageBox.Show("Invalid Satisfaction Value");
                    return;
                }

                lvItem.AddNewItem(itemtype, txtItemName.Text, this.cboItemTypeIL.SelectedValue.ToString(), price, satisfactionValue);
            }
            else
            {
                MessageBox.Show("Invalid input!");
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            lvItem.DeleteItem(itemtype);
        }

        public bool validateInput()
        {
            if (txtItemName.Text.Trim().Length == 0)
                return false;

            if (cboItemTypeIL.SelectedIndex == -1)
                return false;

            return true;
        }

        private void btnEditItemType_Click(object sender, RoutedEventArgs e)
        {
            lvItemType.EditItemType(user, event_, (bool)chkNecessary.IsChecked);
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            double price;
            bool isDouble = double.TryParse(txtItemPrice.Text, out price);
            if (!isDouble)
            {
                MessageBox.Show("Invalid Price");
                return;
            }
            int satisfactionValue;
            bool isInt = int.TryParse(txtItemSatisfaction.Text, out satisfactionValue);
            if (!isInt)
            {
                MessageBox.Show("Invalid Satisfaction Value");
                return;
            }

            lvItem.EditItem(itemtype, price, satisfactionValue);
        }
    }
}