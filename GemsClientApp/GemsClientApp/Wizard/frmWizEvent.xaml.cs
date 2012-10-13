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
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmWizEvent.xaml
    /// </summary>
    public partial class frmWizEvent : GemsWizPage
    {
        Events evnt;
        List<EventDay> days;
        List<List<Program>> programs;
        List<List<Guest>> guests;


        public frmWizEvent(frmWizard c)
        {
            evnt = c._event;
            days = c._days;
            programs = c._programs;
            guests = c._guests;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtEventName.Focus();

            txtEventName.Text = evnt.Name;
            txtDesc.AppendText(evnt.Description);
            txtTag.Text = evnt.Tag;
            txtWebsite.Text = evnt.Website;

            dtpStart.SelectedDateTime = evnt.StartDateTime;
            dtpEnd.SelectedDateTime = evnt.EndDateTime;
        }
        private bool validateInput()
        {
            if (txtEventName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter an Event Name",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (dtpStart.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Invalid Event Start Date",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (dtpEnd.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Invalid Event End Date",
                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void createDays(DateTime EventStartDateTime, DateTime EventEndDatetime)
        {
            days.Clear();
            

            DateTime current_date = EventStartDateTime;
            int day = 1;
            do
            {
                EventDay tempday = new EventDay();
                tempday.DayNumber = day;

                days.Add(tempday);
                //programs.Add(new List<Program>());
                //guests.Add(new List<Guest>());

                day++;

                current_date = current_date.Date;
                current_date = current_date.AddDays(1);
            } while (current_date < EventEndDatetime);



        }

        

        public override bool Save()
        {
            if (!validateInput())
                return false;
            try
            {
                DateTime startTime = dtpStart.SelectedDateTime;
                DateTime endTime = dtpEnd.SelectedDateTime;

                if (startTime.CompareTo(endTime) >= 0)
                {
                    throw new Exception("Invalid Date Entry, End Date Must be at a Later Date Then Start Date");
                    
                }
                
                var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);

                //evnt = new Events();
                evnt.Name = txtEventName.Text;
                evnt.StartDateTime = startTime;
                evnt.EndDateTime = endTime;
                evnt.Description = textRange.Text;
                evnt.Website = txtWebsite.Text;
                evnt.Tag = txtTag.Text;

                
                createDays(evnt.StartDateTime, evnt.EndDateTime);

                //make sure days array match the rest.

                while(programs.Count() != days.Count())
                {
                    if (programs.Count() < days.Count())
                    {
                        programs.Add(new List<Program>());
                    }

                    if (programs.Count() > days.Count())
                    {
                        programs.RemoveAt(programs.Count - 1);
                    }
                }

                while (guests.Count() != guests.Count())
                {
                    if (guests.Count() < days.Count())
                    {
                        guests.Add(new List<Guest>());
                    }

                    if (guests.Count() > days.Count())
                    {
                        guests.RemoveAt(guests.Count - 1);
                    }
                }


                days.Count();



                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
           
        }
    }
}
