using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for ucLVCurrentBooking.xaml
	/// </summary>
	public partial class ucLVCurrentBooking : UserControl
	{
		public ucLVCurrentBooking()
		{
			this.InitializeComponent();
		}
		
		public void setSource(ObservableCollection<TimeSlot> temp)
        {
            lv.ItemsSource = temp;
        }
		
		public void ScrollToItem(int posIdx)
        {
            var listView = lv; ;
            listView.SelectedItem = listView.Items.GetItemAt(posIdx);
            listView.ScrollIntoView(listView.Items[0]);
            listView.ScrollIntoView(listView.SelectedItem);
        }
	}
	
	public class BookingDetail
    {
        public string Pirority { get; set; }
        public string Facility { get; set; }
        public string location { get; set; }
        public int balance { get; set; }
    }
}