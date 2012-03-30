using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using evmsService.entities;
using System.Windows;


namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for ucLVSelectedItem.xaml
    /// </summary>
    public partial class ucLVSelectedItem : UserControl
    {
        private ObservableCollection<BudgetItems> _Collection;
        public ucLVSelectedItem()
        {
            InitializeComponent();
            preprocess();
        }

        private void preprocess()
        {
            _Collection = new ObservableCollection<BudgetItems>();
            refresh();
        }

        private void refresh()
        {
            lv.ItemsSource = ItemCollection;
        }

        public void SetExistingSource(List<BudgetItems> lstItemType)
        {
            _Collection = new ObservableCollection<BudgetItems>();
            lstItemType.ForEach(x => _Collection.Add(x));
            refresh();
        }

        public ObservableCollection<BudgetItems> ItemCollection
        { get { return _Collection; } }

        public BudgetItems GetEditItem()
        {
            if (lv.SelectedIndex == -1)
            {
                return null;
            }
            return ItemCollection[lv.SelectedIndex];
        }
    }
}
