using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmItemManagement.xaml
    /// </summary>
    public partial class frmItemManagement : Page
    {
        private Window mainFrame;
        private User user;
        private Event event_;

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
            ExistingLoad();

        }

        private void refreshItemTypes()
        {
            WCFHelperClient client = new WCFHelperClient();
            cboItemType.ItemsSource = client.getItemsTypes();
            client.Close();
        }

        private void ExistingLoad()
        {
            WCFHelperClient client = new WCFHelperClient();
            List<ItemTypes> TypeList = client.getEventSpecificItemType(event_.EventID).ToList<ItemTypes>();
            List<Items> ItemList = client.getItemsByEvent(event_.EventID).ToList<Items>();
            client.Close();

            if (TypeList.Count > 0)
                lvItemType.SetExistingSource(TypeList);
            if (ItemList.Count > 0)
                lvItem.SetExistingSource(ItemList);
            rebindcboItemType4Item();
        }

        private void rebindcboItemType4Item()
        {
            cboItemTypeIL.ItemsSource = lvItemType.GetItemTypeList();
            cboItemTypeIL.DisplayMemberPath = "typeString";
            cboItemTypeIL.SelectedValuePath = "typeString";
        }

        private void clearItemTypeInput()
        {
            txtOthers.Text = "";
            cboItemType.SelectedIndex = -1;
        }

        private void btnAddItemType_Click(object sender, RoutedEventArgs e)
        {
            String itemType2Add = "";
            if (radItemType.IsChecked == true)
            {
                if (cboItemType.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select an Item Type!");
                }
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
            lvItemType.AddNewItemType(user, event_, itemType2Add, chkNecessary.IsChecked.Value);
            rebindcboItemType4Item();
            clearItemTypeInput();
        }

        private void btnToggleItemTypeImpt_Click(object sender, RoutedEventArgs e)
        {
            lvItemType.ToggleItemTypeImpt(user, event_);
            ExistingLoad();
            rebindcboItemType4Item();
        }

        private void btnDeleteItemType_Click(object sender, RoutedEventArgs e)
        {
            lvItemType.DeleteItemType(user, event_);
            rebindcboItemType4Item();
            WCFHelperClient client = new WCFHelperClient();
            List<Items> ItemList = client.getItemsByEvent(event_.EventID).ToList<Items>();
            lvItem.SetExistingSource(ItemList);
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
                decimal price;
                bool isdecimal = decimal.TryParse(txtItemPrice.Text, out price);
                if (!isdecimal)
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
                lvItem.AddNewItem(user, (ItemTypes)cboItemTypeIL.SelectedItem, txtItemName.Text, cboItemTypeIL.SelectedValue.ToString(), price, satisfactionValue);
                clearItemInput();
            }
            else
            {
                MessageBox.Show("Invalid input!");
            }
        }

        private void EnabledItemControl(bool enabled)
        {
            if (enabled)//After Edit
            {
                btnEditItem.Content = "Edit";
                txtItemName.IsReadOnly = false;
                cboItemTypeIL.IsEnabled = true;
                btnAddItem.Visibility = Visibility.Visible;
                btnCancelEditItem.Visibility = Visibility.Collapsed;
            }
            else //Edit Mode
            {
                btnEditItem.Content = "Save";
                txtItemName.IsReadOnly = true;
                cboItemTypeIL.IsEnabled = false;
                btnAddItem.Visibility = Visibility.Collapsed;
                btnCancelEditItem.Visibility = Visibility.Visible;
            }
        }

        private void mapItem(Items Item2Edit)
        {
            txtItemName.Text = Item2Edit.ItemName;

            cboItemTypeIL.SelectedValue = Item2Edit.typeString;
            txtItemPrice.Text = Item2Edit.EstimatedPrice.ToString();
            txtItemSatisfaction.Text = Item2Edit.Satisfaction.ToString();
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            Items Item2Edit = lvItem.GetEditItem();
            if (Item2Edit == null)
            {
                MessageBox.Show("Please Select An Item!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string FunctionName = btnEditItem.Content.ToString().Trim();
            if (FunctionName.CompareTo("Edit") == 0)
            {
                mapItem(Item2Edit);
                EnabledItemControl(false);
            }
            else
            {
                decimal price;
                bool isdecimal = decimal.TryParse(txtItemPrice.Text, out price);
                if (!isdecimal)
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

                lvItem.EditItem(user, (ItemTypes)cboItemTypeIL.SelectedItem, price, satisfactionValue);
                EnabledItemControl(true);
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            lvItem.DeleteItem(user, (ItemTypes)cboItemTypeIL.SelectedItem);
        }

        public bool validateInput()
        {
            if (txtItemName.Text.Trim().Length == 0)
                return false;

            if (cboItemTypeIL.SelectedIndex == -1)
                return false;

            return true;
        }

        private void btnCancelEditItem_Click(object sender, RoutedEventArgs e)
        {
            clearItemInput();
            EnabledItemControl(true);
        }

        private void clearItemInput()
        {
            txtItemName.Text = "";
            txtItemPrice.Text = "";
            txtItemSatisfaction.Text = "";
            cboItemTypeIL.SelectedIndex = -1;
        }
    }
}