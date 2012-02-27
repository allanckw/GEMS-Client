using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for ucLVItem.xaml
    /// </summary>
    public partial class ucLVItem : UserControl
    {
        private ObservableCollection<string> _AvailableItemType =
          new ObservableCollection<string>();
        private ObservableCollection<clsItem> _ItemCollection =
        new ObservableCollection<clsItem>();

        public ucLVItem()
        {
            InitializeComponent();
        }

        public ObservableCollection<clsItem> ItemCollection
        { get { return _ItemCollection; } }

        public ObservableCollection<string> ItemTypeCollection
        { get { return _AvailableItemType; } }

        public void UpdateItemType(ObservableCollection<string> temp)
        {
            _AvailableItemType = temp;
        }

        public void AddNewRow()
        {
            if (ItemCollection.Count > 0)
            {
                if (!Cont2Add())
                {
                    MessageBox.Show("Please fill in all fields to proceed!");
                    return;
                }
            }


            ItemCollection.Add(new clsItem
            {
                ItemName = "<<Value>>",
                ItemDesc = "<<Value>>",
                ItemType = "<<Select a type>>",
                ItemPrice = "<<Value>>",
                ItemSatisfactionValue = "<<Value>>"
                
            });
            ScrollToItem(ItemCollection.Count - 1);
        }

        private Boolean Cont2Add()
        {
            clsItem temp = ItemCollection[ItemCollection.Count - 1];
            if (temp.ItemName.Contains("<<"))
                return false;
            if (temp.ItemDesc.Contains("<<"))
                return false;
            if (temp.ItemType.Contains("<<"))
                return false;
            if (temp.ItemPrice.Contains("<<"))
                return false;
            if (temp.ItemSatisfactionValue.Contains("<<"))
                return false;

            return true;
        }

        public void ScrollToItem(int posIdx)
        {
            var listView = lv; ;
            listView.SelectedItem = listView.Items.GetItemAt(posIdx);
            listView.ScrollIntoView(listView.Items[0]);
            listView.ScrollIntoView(listView.SelectedItem);
        }

        public void DeleteRow()
        {
            if (lv.SelectedIndex != -1)
            {
                ItemCollection.RemoveAt(lv.SelectedIndex);
            }
        }

        public void DeleteRowByItemType(string ItemType2Delete)
        {
            int idx = 0;
            while (idx != -1)
            {
                idx=-1;
                for (int i = 0; i < ItemCollection.Count-1; i++)
                {
                    clsItem temp = ItemCollection[i];
                    if (temp.ItemType.Contains(ItemType2Delete))
                    {
                        idx = i;
                        break;
                    }
                }
                if (idx!=-1)
                {
                    ItemCollection.RemoveAt(idx);
                }
                
            }
        }
    }

    public class clsItem : DependencyObject
    {
        public static readonly DependencyProperty ItemNameProperty =
          DependencyProperty.Register("ItemName", typeof(string),
          typeof(clsItem), new UIPropertyMetadata(null));

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public static readonly DependencyProperty ItemDescProperty =
          DependencyProperty.Register("ItemDesc", typeof(string),
          typeof(clsItem), new UIPropertyMetadata(null));

        public string ItemDesc
        {
            get { return (string)GetValue(ItemDescProperty); }
            set { SetValue(ItemDescProperty, value); }
        }

        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register("ItemType", typeof(string),
            typeof(clsItem), new UIPropertyMetadata(null));

        public string ItemType
        {
            get { return (string)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }

        public static readonly DependencyProperty ItemPriceProperty =
        DependencyProperty.Register("ItemPrice", typeof(string),
        typeof(clsItem), new UIPropertyMetadata(null));

        public string ItemPrice
        {
            get { return (string)GetValue(ItemPriceProperty); }
            set { SetValue(ItemPriceProperty, value); }
        }

        public static readonly DependencyProperty ItemSatisfactionValueProperty =
        DependencyProperty.Register("ItemSatisfactionValue", typeof(string),
        typeof(clsItem), new UIPropertyMetadata(null));

        public string ItemSatisfactionValue
        {
            get { return (string)GetValue(ItemSatisfactionValueProperty); }
            set { SetValue(ItemSatisfactionValueProperty, value); }
        }
    }
}
