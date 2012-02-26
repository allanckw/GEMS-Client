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
	/// Interaction logic for frmBookingFacilityPriority.xaml
	/// </summary>
	public partial class frmBookingFacilityPriority : Window
	{
		public frmBookingFacilityPriority()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
			CreateDTPData();
		}
		//DTP preprocessing
		public void CreateDTPData()
		{
			for (int i = 0; i <= 23; i++)
            {
				cboStartHr.Items.Add(string.Format("{0:00}",i));
				cboEndHr.Items.Add(string.Format("{0:00}",i));
            }

            for (int i = 0; i <= 55; i += 30)
            {
				cboStartMin.Items.Add(string.Format("{0:00}",i));
				cboEndMin.Items.Add(string.Format("{0:00}",i));
            }
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