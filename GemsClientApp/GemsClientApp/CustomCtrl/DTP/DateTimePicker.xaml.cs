using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gems.UIWPF.CustomCtrl
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
            CreateDTPData();
            DateTime d = DateTime.Now;
            dtpDate.SelectedDate =
                d.AddHours(-d.Hour).AddMinutes(-d.Minute).AddSeconds(-d.Second);
            dtpDate.SelectedDate = DateTime.Now;
            cboHr.SelectedIndex = 0;
            cboMin.SelectedIndex = 0;
        }

        public static readonly RoutedEvent DateChangedEvent = EventManager.RegisterRoutedEvent(
        "DateChangedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateTimePicker));


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Provide CLR accessors for the event
        public event RoutedEventHandler DateChanged
        {
            add { AddHandler(DateChangedEvent, value); }
            remove { RemoveHandler(DateChangedEvent, value); }
        }

        // This method raises the DateChanged Event 
        void RaiseDateChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(DateTimePicker.DateChangedEvent);
            RaiseEvent(newEventArgs);
        }

        //DTP preprocessing
        public void CreateDTPData()
        {
            for (int i = 0; i <= 24; i++)
            {
                cboHr.Items.Add(string.Format("{0:00}", i));
            }

            for (int i = 0; i <= 55; i += 30)
            {
                cboMin.Items.Add(string.Format("{0:00}", i));
            }
            cboHr.SelectedIndex = cboMin.SelectedIndex = 0;
        }

        public DateTime SelectedDateTime
        {
            get {
                if (dtpDate.SelectedDate == null)
                    return default(DateTime);
                DateTime d = dtpDate.SelectedDate.Value;
                return new DateTime(d.Year, d.Month, d.Day, 
                    int.Parse(cboHr.SelectedItem.ToString()), int.Parse(cboMin.SelectedItem.ToString()), 0);
            }
            set
            {
                this.Date = value;
                this.Hour = value.Hour;
                this.Minute = value.Minute;
            }
        }

        public DateTime Date
        {
            get
            {
                DateTime d = dtpDate.SelectedDate.Value;
                return d.AddHours(-d.Hour).AddMinutes(-d.Minute).AddSeconds(-d.Second);
            }
            set
            {
                dtpDate.SelectedDate = value.AddHours(-value.Hour).AddMinutes(-value.Minute).
                    AddSeconds(-value.Second);
            }
        }

        public int Hour
        {
            get
            {
                return cboHr.SelectedIndex;
            }
            set
            {
                cboHr.SelectedIndex = value;
            }
        }

        public int Minute
        {
            get
            {
                return int.Parse(cboMin.SelectedItem.ToString());
            }
            set
            {
                if (value > 0)
                    cboMin.SelectedIndex = 1;
                else
                    cboMin.SelectedIndex = 0;
            }
        }

        public Boolean isDateEnabled
        {
            get
            {
               return dtpDate.IsEnabled;
            }
            set
            {
                dtpDate.IsEnabled = value;
            }
        }

        private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseDateChangedEvent();
        }



        public void clear()
        {
            dtpDate.SelectedDate = null;
            cboHr.SelectedIndex = 0;
            cboMin.SelectedIndex = 0;
        }

    }
}
