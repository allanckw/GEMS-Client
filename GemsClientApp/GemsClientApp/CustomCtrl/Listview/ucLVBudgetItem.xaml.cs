using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using evmsService.entities;
using System.Windows;


namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for ucLVBudgetItem.xaml
    /// </summary>
    public partial class ucLVBudgetItem : UserControl
    {
        private ObservableCollection<OptimizedBudgetItemsDetails> _Collection;
        public ucLVBudgetItem()
        {
            InitializeComponent();
            preprocess();
        }

        private void preprocess()
        {
            _Collection = new ObservableCollection<OptimizedBudgetItemsDetails>();
            refresh();
        }

        private void refresh()
        {
            lv.ItemsSource = ItemCollection;
        }

        public void SetExistingSource(List<OptimizedBudgetItemsDetails> lstItemType)
        {
            _Collection = new ObservableCollection<OptimizedBudgetItemsDetails>();
            lstItemType.ForEach(x => _Collection.Add(x));
            refresh();
        }

        public ObservableCollection<OptimizedBudgetItemsDetails> ItemCollection
        { get { return _Collection; } }

        public OptimizedBudgetItemsDetails GetEditItem()
        {
            if (lv.SelectedIndex == -1)
            {
                return null;
            }
            
            return ItemCollection[lv.SelectedIndex];
        }
    }
}
