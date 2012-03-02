using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for ucLVItemType.xaml
    /// </summary>
    public partial class ucLVItemType : UserControl
    {
        //private List<clsItemType> _Collection;
        ObservableCollection<clsItemType> _Collection;
        public ucLVItemType()
        {
            InitializeComponent();
            preprocess();
        }
        public void preprocess()
        {
//            _Collection = new List<clsItemType>();
            _Collection = new ObservableCollection<clsItemType>();
            lv.ItemsSource = ItemTypeCollection;
        }

        public void AddNewItemType(String ItemType, Boolean Important)
        {
            ItemTypeCollection.Add(new clsItemType(ItemType, Important));
        }
        
        public void DeleteItemType()
        {
            if (lv.SelectedIndex != -1)
            {
                ItemTypeCollection.RemoveAt(lv.SelectedIndex);
            }
        }

        //public List<clsItemType> Collection
        //{ get { return _Collection; } }

        public ObservableCollection<clsItemType> ItemTypeCollection
        { get { return _Collection; } }

        public List<String> GetItemTypeList()
        {
            List<String> temp = new List<String>();
            foreach (clsItemType item in ItemTypeCollection)
            {
                temp.Add(item.ItemType);
            }
            return temp;
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //clsItemType temp = (clsItemType)lv.SelectedItem;
            //MessageBox.Show(temp.ItemType.ToString());
        }
    }

    public class clsItemType
    {
        public clsItemType(String _ItemType, Boolean _Important)
        {
            ItemType = _ItemType;
            Important = _Important;
        }

        public string ItemType { get; set; }
        public Boolean Important { get; set; }
    }
}
