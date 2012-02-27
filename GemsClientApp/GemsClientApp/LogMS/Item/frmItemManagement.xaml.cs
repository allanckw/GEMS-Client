using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for frmItemManagement.xaml
	/// </summary>
	public partial class frmItemManagement : Window
	{
		public frmItemManagement()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        private void btnItemTypeAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lvItemType.AddNewRow())
            {
                lvItem.UpdateItemType(lvItemType.UpdateItemTypeChosen());
            }
        }

        private void btnItemTypeDelete_Click(object sender, RoutedEventArgs e)
        {
            string itemType2Delete = lvItemType.DeleteRow();
            if (lvItem.ItemCollection.Count > 0)
            {
                lvItem.DeleteRowByItemType(itemType2Delete);
                lvItem.UpdateItemType(lvItemType.UpdateItemTypeChosen());
            }
        }

        private void btnItemAdd_Click(object sender, RoutedEventArgs e)
        {
            lvItem.AddNewRow();
        }

        private void btnItemDelete_Click(object sender, RoutedEventArgs e)
        {
            lvItem.DeleteRow();
        }
	}
}