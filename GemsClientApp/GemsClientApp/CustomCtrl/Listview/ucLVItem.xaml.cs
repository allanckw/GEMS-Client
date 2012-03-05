using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using evmsService.entities;
using System.Windows;

namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for ucLVItem.xaml
    /// </summary>
    public partial class ucLVItem : UserControl
    {
        private ObservableCollection<Items> _Collection;

        public ucLVItem()
        {
            InitializeComponent();
            preprocess();
        }

        private void preprocess()
        {
            _Collection = new ObservableCollection<Items>();
            refresh();
        }

        private void refresh()
        {
            lv.ItemsSource = ItemCollection;
        }

        public void AddNewItem(User user, ItemTypes itemtype, string n, string t, double p, int s)
        {//Need User
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                ItemCollection.Add(client.addItem(user, itemtype, n, s, p));
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error have occured: " + ex.Message, "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        public void EditItem(User user, ItemTypes itemtype, double p, int s)
        {//Need User
            Items temp = ItemCollection[lv.SelectedIndex];
            //temp.ItemPrice = p;
            //temp.ItemSValue = s;
            ItemCollection[lv.SelectedIndex] = temp;
        }

        public void DeleteItem(User user, ItemTypes itemtype)
        {//Need User
            if (lv.SelectedIndex != -1)
            {
                Items Item2Delete = ItemCollection[lv.SelectedIndex];
                ItemCollection.RemoveAt(lv.SelectedIndex);
                refresh();
            }
        }

        public ObservableCollection<Items> ItemCollection
        { get { return _Collection; } }

    }

}
