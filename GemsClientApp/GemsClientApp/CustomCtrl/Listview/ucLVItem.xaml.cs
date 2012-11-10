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

        public void SetExistingSource(List<Items> lstItemType)
        {
            _Collection = new ObservableCollection<Items>();
            lstItemType.ForEach(x => _Collection.Add(x));
            refresh();
        }

        public List<Items> getExistingSource()
        {
            IEnumerable<Items> obsCollection = (IEnumerable<Items>)_Collection;
            return new List<Items>(obsCollection);
        }

        public void AddNewItem(User user, ItemTypes itemtype, string n, string t, decimal p, int s)
        {
            EventItemsHelper client = new EventItemsHelper();
            try
            {

                Items Item2Add = client.AddEventItem(user, itemtype, n, s, p);
                ItemCollection.Add(Item2Add);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error have occured: " + ex.Message, "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                client.Close();
            }

        }

        public void EditItem(User user, ItemTypes itemtype, decimal p, int s)
        {
            EventItemsHelper client = new EventItemsHelper();
            try
            {

                Items Item2Edit = ItemCollection[lv.SelectedIndex];
                Item2Edit.EstimatedPrice = p;
                Item2Edit.Satisfaction = s;
                ItemCollection[lv.SelectedIndex] = Item2Edit;
                client.UpdateSatifactionAndEstPrice(user, Item2Edit, s, p);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error have occured: " + ex.Message, "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                client.Close();
            }
        }

        public void DeleteItem(User user, ItemTypes itemtype)
        {
            EventItemsHelper client = new EventItemsHelper();
            try
            {
                if (lv.SelectedIndex != -1)
                {

                    Items Item2Delete = ItemCollection[lv.SelectedIndex];
                    client.DeleteEventItem(user, Item2Delete);
                    client.Close();

                    ItemCollection.RemoveAt(lv.SelectedIndex);
                    refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error have occured: " + ex.Message, "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                client.Close();
            }
        }

        public ObservableCollection<Items> ItemCollection
        { get { return _Collection; } }

        public Items GetEditItem()
        {
            if (lv.SelectedIndex==-1)
            {
                return null;
            }
            return ItemCollection[lv.SelectedIndex];
        }
    }

}
