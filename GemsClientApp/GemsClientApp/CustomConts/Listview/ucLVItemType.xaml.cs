using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for ucLVItemType.xaml
	/// </summary>
	public partial class ucLVItemType : UserControl
	{
        private ObservableCollection<clsItemType> _ItemTypeCollection =
        new ObservableCollection<clsItemType>();
        private ObservableCollection<string> _AvailableItemType =
          new ObservableCollection<string>();
        private ObservableCollection<string> _AvailableImptChoice =
         new ObservableCollection<string>();

		public ucLVItemType()
		{
            preprocessing();
			this.InitializeComponent();
		}

        public void preprocessing()
        {
            _AvailableItemType.Add("ItemType1");
            _AvailableItemType.Add("ItemType2");
            _AvailableItemType.Add("ItemType3");
            _AvailableItemType.Add("ItemType4");
            _AvailableItemType.Add("Others");
            _AvailableImptChoice.Add("Primary");
            _AvailableImptChoice.Add("Secondary");
        }

        public ObservableCollection<clsItemType> ItemTypeCollection
        { get { return _ItemTypeCollection; } }

        public ObservableCollection<string> ImportantChoice
        { get { return _AvailableImptChoice; } }

        public ObservableCollection<string> AvailableItemType
        { get { return _AvailableItemType; } }

        public Boolean AddNewRow()
        {
            if (ItemTypeCollection.Count > 0)
            {
                if (!Cont2Add())
                    return false;
            }
				

            ItemTypeCollection.Add(new clsItemType
            {
                ItemType = "<<Select>>",
                Others = "Nil",
                Important = "<<Select>>"
            });
            ScrollToItem(ItemTypeCollection.Count - 1);
            return true;
        }

        public ObservableCollection<string> UpdateItemTypeChosen()
        {
            ObservableCollection<string> _ItemTypeChosen =
            new ObservableCollection<string>();
            foreach (var item in ItemTypeCollection)
        	{
                String toAdd = item.ItemType;
                if(!toAdd.Contains("<<"))
                    _ItemTypeChosen.Add(toAdd);
            }
            return _ItemTypeChosen;
        }

        private Boolean Cont2Add()
        {
            clsItemType temp = ItemTypeCollection[ItemTypeCollection.Count - 1];
            if (temp.ItemType.Contains("<<"))
                return false;
            if (temp.Important.Contains("<<"))
                return false;

            return ExistInItemType();
        }

        private Boolean ExistInItemType()
        {
            if (ItemTypeCollection.Count > 1)
            {
                clsItemType LastItem = ItemTypeCollection[ItemTypeCollection.Count - 1];
                String toCompare1 = LastItem.ItemType;

                for (int i = 0; i < ItemTypeCollection.Count - 2; i++)
                {
                    clsItemType item = ItemTypeCollection[i];
                    String toCompare2 = item.ItemType;

                    if (toCompare1.ToUpper().CompareTo(toCompare2.ToUpper()) == 0)
                    {
                        MessageBox.Show("Item Type Already Exist!");
                        return false;
                    }
                }
            }
    
            return true;
        }
        public void ScrollToItem(int posIdx)
        {
            var listView = lv; ;
            listView.SelectedItem = listView.Items.GetItemAt(posIdx);
            listView.ScrollIntoView(listView.Items[0]);
            listView.ScrollIntoView(listView.SelectedItem);
        }

        public string DeleteRow()
        {
            if (lv.SelectedIndex != -1)
            {
                int idx = lv.SelectedIndex;
                string temp = _ItemTypeCollection[idx].ItemType;
                ItemTypeCollection.RemoveAt(idx);
                return temp;
            }
            return "";
        }
	}

    public class clsItemType : DependencyObject
    {
        public static readonly DependencyProperty ItemTypeProperty =
          DependencyProperty.Register("ItemType", typeof(string),
          typeof(clsItemType), new UIPropertyMetadata(null));

        public string ItemType
        {
            get {
                string temp=(string)GetValue(ItemTypeProperty);
                if (temp.Contains("Other"))
                {
                    return Others;
                }
                return (string)GetValue(ItemTypeProperty);
            }
            set { SetValue(ItemTypeProperty, value); }
        }

        public static readonly DependencyProperty OthersProperty =
          DependencyProperty.Register("Others", typeof(string),
          typeof(clsItemType), new UIPropertyMetadata(null));

        public string Others
        {
            get { return (string)GetValue(OthersProperty); }
            set { SetValue(OthersProperty, value); }
        }

        public static readonly DependencyProperty ImportantProperty =
            DependencyProperty.Register("Important", typeof(string),
            typeof(clsItemType), new UIPropertyMetadata(null));

        public string Important
        {
            get { return (string)GetValue(ImportantProperty); }
            set { SetValue(ImportantProperty, value); }
        }
    }
}