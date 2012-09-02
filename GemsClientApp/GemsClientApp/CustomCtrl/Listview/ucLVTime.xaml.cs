using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;
using evmsService.entities;

namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for ucLVTime.xaml
    /// </summary>
    public partial class ucLVTime : UserControl
    {
        private int startHr = 0;
        private int endHr = 24;
        private int BookStartIdx = 0;
        private int BookEndIdx = 0;
        private bool slotclash = false;
        private int clashIdx = -1;

        ObservableCollection<TimeSlot> _TimeCollection;

        public ucLVTime()
        {
            this.InitializeComponent();
            Reset();
        }

        private void RefreshTime()
        {
            lv.ItemsSource = TimeCollection;
        }


        public void SetBookingTimeRange(DateTime bookingStart, DateTime bookingEnd)
        {
            int BookingSHr = bookingStart.Hour;
            int BookingSMin = bookingStart.Minute;
            int BookingEHr = bookingEnd.Hour;
            int BookingEMin = bookingEnd.Minute;
            BookStartIdx = PreprocessTime(BookingSHr, BookingSMin);
            if (bookingStart.Date.CompareTo(bookingEnd.Date) != 0)
            {
                BookingEHr = 24;
                BookingEMin = 0;
            }
            BookEndIdx = PreprocessTime(BookingEHr, BookingEMin) - 1;
        }

        public ObservableCollection<TimeSlot> TimeCollection
        { get { return _TimeCollection; } }

        public void SetSource(List<FacilityBookingConfirmed> temp)
        {
            Reset();
            foreach (FacilityBookingConfirmed item in temp)
            {
                DateTime start = item.RequestStartDateTime;
                DateTime end = item.RequestEndDateTime;

                int shr = start.Hour;
                int smin = start.Minute;
                int ehr = end.Hour;
                int emin = end.Minute;

                string EventName = item.Purpose;
                int sIdx = PreprocessTime(shr, smin);
                if (end.CompareTo(start) != 0)
                {
                    ehr = 24;
                    emin = 0;
                }

                int eIdx = PreprocessTime(ehr, emin);
                for (int i = sIdx; i < eIdx; i++)
                {
                    TimeCollection[i].Purpose = EventName;
                }
            }
            checkClash();

            ScrollToItem(BookEndIdx-1);
        }

        public bool CanApproved()
        {
            if (slotclash)
                ScrollToItem(clashIdx);
            //if slotClash = true, the slot is unavailable so cannot be approved
            return (slotclash == false);
        }

        private void checkClash()
        {
            for (int i = BookStartIdx; i <= BookEndIdx; i++)
            {
                if (TimeCollection[i].Purpose.Trim().Length > 0)
                {
                    clashIdx = i;
                    TimeCollection[i].Balance = -1;
                    slotclash = true;
                }
                else
                    TimeCollection[i].Balance = 1;
            }
            //Code to populate first index to green if start time is 0000
            if (BookStartIdx == 0)
            {
                TimeSlot ts = TimeCollection[0];
                TimeCollection.RemoveAt(0);                
                TimeCollection.Insert(0, ts);
            }

            RefreshTime();

        }

        private int PreprocessTime(int hr, int min)
        {
            int tempidx = hr * 2;
            if (min > 0)
            {
                tempidx += 1;
            }

            return tempidx;
        }

        public void Reset()
        {
            SlotGeneration();
            slotclash = false;
            ScrollToItem(0);
        }

        private void ScrollToItem(int posIdx)
        {
            if (posIdx == -1)
            {
                posIdx = 0;
            }
            var listView = lv; 
            listView.SelectedItem = listView.Items.GetItemAt(posIdx);
            listView.ScrollIntoView(listView.Items[0]);
            listView.ScrollIntoView(listView.SelectedItem);
        }

        private List<string> ProcessTimeSlot()
        {
            List<string> temp = new List<String>();
            for (int h = startHr; h <= endHr; h++)
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

        private void SlotGeneration()
        {
            _TimeCollection = new ObservableCollection<TimeSlot>();
            List<string> temp = ProcessTimeSlot();
            for (int i = 0; i < temp.Count - 1; i++)
            {
                _TimeCollection.Add(new TimeSlot
                {
                    StartTime = temp[i],
                    EndTime = temp[i + 1],
                    Purpose = "",
                    Balance = 0
                });
            }
            RefreshTime();
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