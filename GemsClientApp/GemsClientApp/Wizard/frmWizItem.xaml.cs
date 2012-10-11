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
    /// Interaction logic for frmWizItem.xaml
    /// </summary>
    public partial class frmWizItem : GemsWizPage
    {
        private User user;
        private Events event_;

        public frmWizItem(frmWizard c)
        {
            InitializeComponent();
            user = c._user;
            event_ = c._event;

            radItemType.IsChecked = true;
            refreshItemTypes();
            ExistingLoad();
            cboItemType.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void refreshItemTypes()
        {
            EventItemsHelper client = new EventItemsHelper();
            cboItemType.ItemsSource = client.GetItemsTypes();
            client.Close();
        }

        private void ExistingLoad()
        {
            EventItemsHelper client = new EventItemsHelper();
            List<ItemTypes> TypeList = client.GetEventSpecificItemType(event_.EventID).ToList<ItemTypes>();
            List<Items> ItemList = client.GetItemsByEvent(event_.EventID).ToList<Items>();
            client.Close();

            if (TypeList.Count > 0)
                //lvItemType.SetExistingSource(TypeList);
            if (ItemList.Count > 0)
                //lvItem.SetExistingSource(ItemList);
            rebindcboItemType4Item();
        }

        private void rebindcboItemType4Item()
        {
            //cboItemTypeIL.ItemsSource = lvItemType.GetItemTypeList();

            // to bind items to here

            cboItemTypeIL.DisplayMemberPath = "typeString";
            cboItemTypeIL.SelectedValuePath = "typeString";
        }

        private void clearItemTypeInput()
        {
            txtOthers.Text = "";
            cboItemType.SelectedIndex = -1;
        }

        public bool validateInput()
        {
            if (txtItemName.Text.Trim().Length == 0)
            {
                txtItemName.Focus();
                return false;
            }
            if (cboItemTypeIL.SelectedIndex == -1)
            {
                cboItemTypeIL.Focus();
                return false;
            }
            return true;
        }

        private void clearItemInput()
        {
            txtItemName.Text = "";
            txtItemPrice.Text = "";
            txtItemSatisfaction.Text = "";
            cboItemTypeIL.SelectedIndex = -1;
            EventItemsHelper client = new EventItemsHelper();
            List<Items> ItemList = client.GetItemsByEvent(event_.EventID).ToList<Items>();
            //lvItem.SetExistingSource(ItemList);
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
                

                refreshItemTypes();
            }
            
            //lvItemType.AddNewItemType(user, event_, itemType2Add, chkNecessary.IsChecked.Value);
            //lvItemType.Items.Add("Column1Text").SubItems.AddRange(row1);

            ItemTypes it = new ItemTypes();
            it.typeString = itemType2Add;
            it.IsImportantType = chkNecessary.IsChecked.Value;
            lvItemType.Items.Add(it);

            rebindcboItemType4Item();
            clearItemTypeInput();
        }

        private void btnToggleItemTypeImpt_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemType.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select an Item Type to toggle!", "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ItemTypes it = (ItemTypes)lvItemType.Items.GetItemAt(lvItemType.SelectedIndex);
            bool Important = it.IsImportantType;

            if (Important)
                it.IsImportantType = false;
            else
                it.IsImportantType = true;

            int tempindex = lvItemType.SelectedIndex;
            lvItemType.Items.RemoveAt(lvItemType.SelectedIndex);

            //if (lvItemType.Items.Count != 0)
                lvItemType.Items.Insert(tempindex, it);
            //else
            //    lvItemType.Items.Add(it);

            //lvItemType.ToggleItemTypeImpt(user, event_);

            //ItemTypes it = new ItemTypes();
            //it.IsImportantType = chkNecessary.IsChecked.Value;
            //lvItemType.Items.Add(it);

            //int selected = lvItemType.SelectedIndex;
            //lvItemType.Items.GetItemAt(lvItemType.SelectedIndex);

            ExistingLoad();
            rebindcboItemType4Item();
        }

        private void btnDeleteItemType_Click(object sender, RoutedEventArgs e)
        {
            //lvItemType.DeleteItemType(user, event_);
            if (lvItemType.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select an Item Type to delete!", "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lvItemType.Items.RemoveAt(lvItemType.SelectedIndex);
            rebindcboItemType4Item();

            //EventItemsHelper client = new EventItemsHelper();
            //List<Items> ItemList = client.GetItemsByEvent(event_.EventID).ToList<Items>();
            //lvItem.SetExistingSource(ItemList);
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
                    txtItemPrice.Text = "";
                    txtItemPrice.Focus();
                    return;
                }
                int satisfactionValue;
                bool isInt = int.TryParse(txtItemSatisfaction.Text, out satisfactionValue);
                if (!isInt)
                {
                    MessageBox.Show("Invalid Satisfaction Value");
                    txtItemSatisfaction.Text = "";
                    txtItemSatisfaction.Focus();
                    return;
                }
                //lvItem.AddNewItem(user, (ItemTypes)cboItemTypeIL.SelectedItem, txtItemName.Text, cboItemTypeIL.SelectedValue.ToString(), price, satisfactionValue);
                clearItemInput();
            }
            else
            {
                MessageBox.Show("Invalid input!");
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            //lvItem.DeleteItem(user, (ItemTypes)cboItemTypeIL.SelectedItem);
        }

        private void btnCalculateBudget_Click(object sender, RoutedEventArgs e)
        {
            ////decimal maxBudget;
            //if (lvItemType.GetItemTypeList().Count == 0)
            //{
            //    MessageBox.Show("There are no added Item types!", "Add Item Type First",
            //        MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return;
            //}
            //else if (lvItem.getExistingSource().Count == 0)
            //{
            //    MessageBox.Show("There are no added Items!", "Add Items First",
            //        MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return;
            //}
            //else if (Decimal.TryParse(txtMaxBudget.Text.Trim(), out maxBudget))
            //{

            //    var budgetForm = new frmAllocateBudget(user, event_,
            //        lvItemType.GetItemTypeList(), lvItem.getExistingSource(), maxBudget);
            //    budgetForm.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Maximum Budget must be numeric!", "Invalid input",
            //            MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        private void btnCancelEditItem_Click(object sender, RoutedEventArgs e)
        {
            clearItemInput();
            EnabledItemControl(true);
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            //Items Item2Edit = lvItem.GetEditItem();
            //if (Item2Edit == null)
            //{
            //    MessageBox.Show("Please Select An Item!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            string FunctionName = btnEditItem.Content.ToString().Trim();
            if (FunctionName.CompareTo("Edit") == 0)
            {
               // mapItem(Item2Edit);
                EnabledItemControl(false);
            }
            else
            {
                decimal price;
                bool isdecimal = decimal.TryParse(txtItemPrice.Text, out price);
                if (!isdecimal)
                {
                    MessageBox.Show("Invalid Price");
                    txtItemPrice.Text = "";
                    txtItemPrice.Focus();
                    return;
                }
                int satisfactionValue;
                bool isInt = int.TryParse(txtItemSatisfaction.Text, out satisfactionValue);
                if (!isInt)
                {
                    MessageBox.Show("Invalid Satisfaction Value");
                    txtItemSatisfaction.Text = "";
                    txtItemSatisfaction.Focus();
                    return;
                }

                //lvItem.EditItem(user, (ItemTypes)cboItemTypeIL.SelectedItem, price, satisfactionValue);
                EnabledItemControl(true);
                clearItemInput();
            }
        }

        private void cboItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            radItemType.IsChecked = true;
        }

        private void txtOthers_TextChanged(object sender, TextChangedEventArgs e)
        {
            radOthers.IsChecked = true;
        }
    }
}
