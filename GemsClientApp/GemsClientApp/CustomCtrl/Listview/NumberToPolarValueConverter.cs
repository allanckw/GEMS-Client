using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using evmsService.entities;

namespace Gems.UIWPF.CustomCtrl
{
    [ValueConversion(typeof(object), typeof(int))]
    public class NumberToPolarValueConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            double number = (double)System.Convert.ChangeType(value, typeof(double));

            if (number == -1)
                return -1;//red

            if (number == 1)
                return +1; //green

            return 0;//normal
        }

        public object ConvertBack(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }

    [ValueConversion(typeof(bool), typeof(int))]
    public class BoolToPolarValueConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            bool isBought = (bool)System.Convert.ChangeType(value, typeof(bool));
            System.Diagnostics.Trace.WriteLine("Debug : is bought = " + isBought + "\n\n");

            if (isBought)
                return 1;//green

            return 0;//normal
        }

        public object ConvertBack(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }

    [ValueConversion(typeof(int), typeof(int))]
    public class TaskToPolarValueConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            int taskID = (int)System.Convert.ChangeType(value, typeof(int));
            WCFHelperClient client = new WCFHelperClient();
            Task task = client.GetTask(taskID);
            //System.Diagnostics.Trace.WriteLine("Debug : " + taskID + "\n");
            client.Close();
            DateTime dueDate = task.DueDate;
            double percentageCompleted = task.PercentageCompletion;
            if (percentageCompleted < 100)
            {
                if (dueDate.CompareTo(DateTime.Now) < 0)
                {
                    return -1;
                }
                else
                    return 0;
            }
            //System.Diagnostics.Trace.WriteLine("Debug : DueDate = " + DueDate.ToString("dd MMM yyyy HH:mm") + "\n");
            //System.Diagnostics.Trace.WriteLine("Debug : " + DueDate.CompareTo(DateTime.Now) + "\n");
            //if (DueDate.CompareTo(DateTime.Now) < 0)
            //    return -1;//red meaning overdue date

            return 1;//normal
        }

        public object ConvertBack(
            object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }
}
