using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows;
using evmsService.entities;

namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for ucLVItemType.xaml
    /// </summary>
    public partial class ucLVItemType : UserControl
    {
        ObservableCollection<ItemTypes> _Collection;
        public ucLVItemType()
        {
            InitializeComponent();
            preprocess();
        }
        public void preprocess()
        {
            _Collection = new ObservableCollection<ItemTypes>();
            lv.ItemsSource = ItemTypeCollection;
        }

        public void AddNewItemType(User u,Event event_, String ItemType, Boolean Important)
        {
            WCFHelperClient client = new WCFHelperClient();
            ItemTypes type = client.addEventItemType(u,event_.EventID,ItemType,Important);
            client.Close();
            ItemTypeCollection.Add(type);
        }

        public void EditItemType(User u, Event event_, Boolean Important)
        {
            ItemTypeCollection[lv.SelectedIndex].IsImportantType = Important;
        }

        public void DeleteItemType(User u, Event event_)
        {
            if (lv.SelectedIndex != -1)
            {
                ItemTypes type2delete = ItemTypeCollection[lv.SelectedIndex];
                ItemTypeCollection.RemoveAt(lv.SelectedIndex);
            }
        }

        public ObservableCollection<ItemTypes> ItemTypeCollection
        { get { return _Collection; } }

        public List<String> GetItemTypeList()
        {
            List<String> temp = new List<String>();
            foreach (ItemTypes item in ItemTypeCollection)
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


}
