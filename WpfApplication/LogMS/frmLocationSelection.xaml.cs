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
	/// Interaction logic for frmLocationSelection.xaml
	/// </summary>
	public partial class frmLocationSelection : Window
	{
		public frmLocationSelection()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        public void PriorityItemAdd(int TotalLoc)
        {
            List<int> test = new List<int>();
            for (int i = 0; i < TotalLoc; i++)
            {
                test.Add(i + 1);

            }
            cboPriority.ItemsSource = test;

            //To grab cbo selected value in datagrid
            //DataGridRow row = <<DataGridName>>.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
            //ComboBox cbo = <<DataGridName>>.Columns[0].GetCellContent(row) as ComboBox;

        }
	}
}