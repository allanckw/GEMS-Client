using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;


namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for ucLVTime.xaml
	/// </summary>
	public partial class ucLVTime : UserControl
	{
	    
		public ucLVTime()
		{
			this.InitializeComponent();
            SlotGeneration();
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

        public List<string> ProcessTimeSlot()
		{
            List<string> temp = new List<String>();
            for (int h = 8; h <= 22; h++)
            {
                for (int m = 0; m < 60; )
                {
                    string th = string.Format("{0:00}", h);
                    string tm = string.Format("{0:00}", m);
                    string tempStr = th + tm;
                    temp.Add(tempStr);
                    m = m + 30;
                }
            }
            temp.RemoveAt(temp.Count - 1);
            return temp;
		}

        public void SlotGeneration()
        {
			System.Random r=new System.Random();
            ObservableCollection<TimeSlot> _TimeCollection = new ObservableCollection<TimeSlot>();
            List<string> temp = ProcessTimeSlot();
            for (int i = 0; i < temp.Count - 1; i++)
            {
                _TimeCollection.Add(new TimeSlot
                {
                    StartTime = temp[i],
                    EndTime = temp[i + 1],
                    Purpose = "",
                    Balance = r.Next(500)
                });
            }
            setSource(_TimeCollection);
        }

	}
	
    public class TimeSlot
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Purpose { get; set; }
        public int Balance { get; set; }
    }
}