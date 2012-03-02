using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for frmItemManagement.xaml
	/// </summary>
    public partial class frmItemManagement : Window
    {


        public frmItemManagement()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
            preprocessing();
        }

        private void preprocessing()
        {
            cboItemType.Items.Add("Food");
            cboItemType.Items.Add("Entertainment");
        }

        private void btnAddItemType_Click(object sender, RoutedEventArgs e)
        {
            if (radItemType.IsChecked == false)
            {
                if (radOthers.IsChecked == false)
                {
                    return;
                }
            }

            String ItemType2Add = "";
            if (radItemType.IsChecked == true)
            {
                ItemType2Add = cboItemType.SelectedItem.ToString();
            }
            else
            {
                ItemType2Add = txtOthers.Text.ToString().Trim();
            }
            lvItemType.AddNewItemType(ItemType2Add, (bool)chkNecessary.IsChecked);
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
            lvItemType.DeleteItemType();
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
            if (cont2AddItem())
            {
                double price;
                bool isDouble = double.TryParse(txtItemPrice.Text, out price);
                if (!isDouble)
                {
                    MessageBox.Show("Invalid Price");//fill in later
                    return;
                }
                int satisfactionValue;
                bool isInt = int.TryParse(txtItemSatisfaction.Text, out satisfactionValue);
                if (!isInt)
                {
                    MessageBox.Show("Invalid Satisfaction Value");//fill in later
                    return;
                }

                lvItem.AddNewItem(txtItemName.Text, this.cboItemTypeIL.SelectedValue.ToString(), price, satisfactionValue);
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool cont2AddItem()
        {
            if (txtItemName.Text.Trim() == "")
                return false;

            if (cboItemTypeIL.SelectedIndex == -1)
                return false;

            return true;
        }
    }
}