using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using evmsService.entities;

namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for ucLVItem.xaml
    /// </summary>
    public partial class ucLVItem : UserControl
    {
        private ObservableCollection<clsItem> _Collection;

        public ucLVItem()
        {
            InitializeComponent();
            preprocess();
        }

        private void preprocess()
        {
            _Collection = new ObservableCollection<clsItem>();
            refresh();
        }

        private void refresh()
        {
            lv.ItemsSource = ItemCollection;
        }

        public void AddNewItem(ItemTypes itemtype, string n, string t, double p, int s)
        {
            ItemCollection.Add(new clsItem(n,t,p,s));
        }

        public void EditItem(ItemTypes itemtype, double p, int s)
        {
            clsItem temp = ItemCollection[lv.SelectedIndex];
            temp.ItemPrice = p;
            temp.ItemSValue = s;
            ItemCollection[lv.SelectedIndex] = temp;
        }

        public void DeleteItem(ItemTypes itemtype)
        {
            if (lv.SelectedIndex != -1)
            {
                clsItem Item2Delete = ItemCollection[lv.SelectedIndex];
                ItemCollection.RemoveAt(lv.SelectedIndex);
                refresh();
            }
        }

        public ObservableCollection<clsItem> ItemCollection
        { get { return _Collection; } }

    }
    public class clsItem
    {
        public clsItem(string n, string t, double p, int s)
        {
            ItemName = n;
            ItemType = t;
            ItemPrice = p;
            ItemSValue = s;
        }

        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public double ItemPrice { get; set; }
        public int ItemSValue { get; set; }
    }
}
