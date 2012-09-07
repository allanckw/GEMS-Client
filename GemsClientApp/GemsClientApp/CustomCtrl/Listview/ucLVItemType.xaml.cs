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

        public void SetExistingSource(List<ItemTypes> lstItemType)
        {
            _Collection = new ObservableCollection<ItemTypes>();
            lstItemType.ForEach(x => _Collection.Add(x));
            lv.ItemsSource = ItemTypeCollection;
        }

        public void AddNewItemType(User u,Events event_, String ItemType, Boolean Important)
        {
            EventItemsHelper client = new EventItemsHelper();
            try
            {
                ItemTypes type = client.AddEventItemType(u, event_.EventID, ItemType, Important);
                ItemTypeCollection.Add(type);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error have occured: " + ex.Message, "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
        }

        public void ToggleItemTypeImpt(User u, Events event_)
        {
            if (lv.SelectedIndex==-1)
            {
                MessageBox.Show("Please Select an Item Type to toggle!", "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            EventItemsHelper client = new EventItemsHelper();

            //Insert server code here
            ItemTypes Item2Edit = ItemTypeCollection[lv.SelectedIndex];
            bool Important=!Item2Edit.IsImportantType;
            ItemTypeCollection[lv.SelectedIndex].IsImportantType = Important;
            client.SetItemTypeImportance(u, Item2Edit, Important);
            client.Close();
        }

        public void EditItemType(User u, Events event_, Boolean Important)
        {
            EventItemsHelper client = new EventItemsHelper();

            //Insert server code here
            ItemTypes Item2Edit=ItemTypeCollection[lv.SelectedIndex];
            ItemTypeCollection[lv.SelectedIndex].IsImportantType = Important;
            client.SetItemTypeImportance(u, Item2Edit, Important);
            client.Close();
        }

        public void DeleteItemType(User u, Events event_)
        {
            EventItemsHelper client = new EventItemsHelper();

            //Insert server code here
            if (lv.SelectedIndex != -1)
            {
                ItemTypes type2delete = ItemTypeCollection[lv.SelectedIndex];
                client.DeleteEventItemType(u, type2delete);
                ItemTypeCollection.RemoveAt(lv.SelectedIndex);
            }
            client.Close();
        }

        public ObservableCollection<ItemTypes> ItemTypeCollection
        { get { return _Collection; } }

        public List<ItemTypes> GetItemTypeList()
        {
            //List<T> myList = new List<T>(myObservableCollection);
            return (new List<ItemTypes>(ItemTypeCollection));
        }
    }
    //List to observable
    //(TObservable) =new ObservableCollection (TObservable)(); 
    //Convert List items(OldListItems) to collection OldListItems.ForEach(x => TObservable.Add(X));

}
