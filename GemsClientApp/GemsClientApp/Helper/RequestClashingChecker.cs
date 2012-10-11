using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gems.UIWPF.Helper
{

    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsTaken { get; set; }

        public TimeSlot(DateTime start, DateTime end)
        {
            this.StartTime = start;
            this.EndTime = end;
            this.IsTaken = false;
        }
    }

    public class RequestClashingChecker
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        List<TimeSlot> timeCollection;

        public RequestClashingChecker(DateTime date)
        {
            this.date = date.AddHours(-date.Hour).AddMinutes(-date.Minute);
            SlotGeneration();
        }


        public void SetTimeSlotTaken(DateTime start, DateTime end)
        {
            int startIndex = PreprocessTime(start.Hour, start.Minute);
            int endIndex = 0;

            if (end.Date != start.Date)
                endIndex = 47;

            else
                endIndex = PreprocessTime(end.Hour, end.Minute);

            for (int i = startIndex; i < endIndex; i++)
            {
                timeCollection[i].IsTaken = true;
            }
        }

        public bool HaveClash(DateTime testStart, DateTime testEnd)
        {
            int startIndex = PreprocessTime(testStart.Hour, testStart.Minute);
            int endIndex = 0;
            if (testStart.Date != testEnd.Date)
                endIndex = 47;
            else
                endIndex = PreprocessTime(testEnd.Hour, testEnd.Minute);

            for (int i = startIndex; i < endIndex; i++)
            {
                if (timeCollection[i].IsTaken)
                    return true;
            }

            return false;
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

        private void SlotGeneration()
        {
            timeCollection = new List<TimeSlot>();

            for (int i = 0; i < 48; i++)
            {
                TimeSpan duration = new TimeSpan(0, 30, 0);
                TimeSlot t = new TimeSlot(date, date.Add(duration));
                date = date.Add(duration);
                timeCollection.Add(t);
            }
        }

        // Tests if two given periods overlap each other.
        // return true if the periods overlap, false other wise
        public static bool checkIsOverlap(DateTime basePeriodStart, DateTime basePeriodEnd,
            DateTime testPeriodStart, DateTime testPeriodEnd)
        {
            return (
                (testPeriodStart >= basePeriodStart && testPeriodStart < basePeriodEnd) ||
                (testPeriodEnd <= basePeriodEnd && testPeriodEnd > basePeriodStart) ||
                (testPeriodStart <= basePeriodStart && testPeriodEnd >= basePeriodEnd)
            );

        }

    }

}