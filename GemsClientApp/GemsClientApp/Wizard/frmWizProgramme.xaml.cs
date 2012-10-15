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
using System.Windows.Controls.Primitives;
using Gems.UIWPF.Helper;


namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmWizProgramme.xaml
    /// </summary>
    /// 

    
    /// 


    public partial class frmWizProgramme : GemsWizPage
    {

        
        // Events event_;
        List<EventDay> _day;
        List<List<Program>> _programs;

        List<List<Program>> _programs_Original;
        Events _event;
        frmWizard parent;



        private List<List<Program>> clone(List<List<Program>> old, List<List<Program>> newp)
        {
            newp.Clear();
            for (int i = 0; i < old.Count(); i++)
            {
                List<Program> ps = old[i];
                List<Program> newps = new List<Program>();
                for (int x = 0; x < ps.Count(); x++)
                {
                    Program p = new Program();
                    p.Name = ps[x].Name;
                    p.Location = ps[x].Location;
                    p.StartDateTime = ps[x].StartDateTime;
                    p.EndDateTime = ps[x].EndDateTime;
                    p.Description = ps[x].Description;
                    newps.Add(p);
                }

                newp.Add(newps);

            }

            return newp;
        }

        public frmWizProgramme(frmWizard c)
        {
            _day = c._days;
            _event = c._event;
            _programs_Original = c._programs;
            parent = c;
            _programs = new List<List<Program>>();
            clone(_programs_Original, _programs);

            InitializeComponent();

            lstProgram.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(lstProgram_PreviewMouseLeftButtonDown);
            lstProgram.Drop += new DragEventHandler(lstProgram_Drop);

            lstProgram.DragOver += new DragEventHandler(lstProgram_DragOver);
        }


        private DateTime GetStartDateTime(EventDay d)
        {
            if(d.DayNumber == 1)
                return _event.StartDateTime;
            DateTime dt = _event.StartDateTime.AddDays(d.DayNumber - 1).Date;
            return dt;
        }

        private DateTime GetEndDateTime(EventDay d)
        {
            if (d.DayNumber == _day.Count)
                return _event.EndDateTime;
            DateTime dt = _event.StartDateTime.AddDays(d.DayNumber).Date;
            return dt;
        }

        public void CreateDTPData()
        {
            for (int i = 0; i <= 24; i++)
            {
                cboStartHr.Items.Add(string.Format("{0:00}", i));
                cboEndHr.Items.Add(string.Format("{0:00}", i));
            }

            for (int i = 0; i <= 55; i += 30)
            {
                cboStartMin.Items.Add(string.Format("{0:00}", i));
                cboEndMin.Items.Add(string.Format("{0:00}", i));
            }
        }
        private void loadPrograms(EventDay d)
        {
            try
            {



                DateTime curr = GetStartDateTime(d);
                DateTime end = GetEndDateTime(d);

                List<Program> progList = _programs[d.DayNumber - 1];
                List<Program> newprogList = new List<Program>();
                while (curr.CompareTo(end) < 0)
                {
                    for (int i = 0; i < progList.Count; i++)
                    {
                        if (progList[i].StartDateTime.CompareTo(curr) == 0)
                        {
                            newprogList.Add(progList[i]);
                            curr = progList[i].EndDateTime;



                            goto next;
                        }
                    }

                    Program p = new Program();
                    p.Name = "";
                    p.StartDateTime = curr;
                    p.EndDateTime = curr.AddMinutes(30);
                    newprogList.Add(p);
                    curr = curr.AddMinutes(30);

                next:
                    continue;

                }
                //lstProgram.SelectedIndex = -1;
                lstProgram.ItemsSource = newprogList.OrderBy(x => x.StartDateTime)
                                                 .ThenBy(x => x.EndDateTime).ToList<Program>();
                lstProgram_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadPrograms()
        {

            cboDay.Items.Clear();

            for (int i = 0; i < _day.Count; i++)
            {
                
                cboDay.Items.Add(_day[i].DayNumber);
                //cboDay.Items[cboDay.Items.Count-1]
            }
            cboDay.SelectedIndex = 0;

        }

        private bool validateInputs()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter programme segment name.");
                return false;
            }
            if (cboStartHr.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter starting hour of programme segment.");
                return false;
            }
            if (cboStartMin.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter starting minute of programme segment.");
                return false;
            }
            if (cboEndHr.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter ending hour of programme segment.");
                return false;
            }
            if (cboEndMin.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter ending minute of programme segment.");
                return false;
            }

            return true;
        }

        private bool ChkClash(List<Program> p, DateTime segmentStart, DateTime segmentEnd)
        {
            RequestClashingChecker checker = new RequestClashingChecker(segmentStart);


            foreach (Program prog in p)
            {
                
                checker.SetTimeSlotTaken(prog.StartDateTime, prog.EndDateTime);
            }

            return checker.HaveClash(segmentStart, segmentEnd);
            
        }

        private List<Program> copylist(List<Program> p)
        {
            List<Program> newp = new List<Program>();

            foreach(Program prog in p)
            {
                newp.Add(prog);
            }

            return newp;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (validateInputs())
            {
                EventDay d = _day[cboDay.SelectedIndex];
                List<Program> p = _programs[cboDay.SelectedIndex];
              
                DateTime SegmentStartDateTime = GetStartDateTime(d);
                DateTime SegmentEndDateTime = GetEndDateTime(d);


                DateTime startpro = SegmentStartDateTime.Date;
                DateTime endpro = SegmentStartDateTime.Date;


                startpro = startpro
                    .AddHours(int.Parse(cboStartHr.SelectedValue.ToString()))
                    .AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));

                int idx = cboBookDuration.SelectedIndex + 1;
                //int duration = idx * 30;

                endpro = startpro.AddMinutes(idx * 30);

                if (SegmentStartDateTime > startpro || SegmentEndDateTime < endpro)
                    throw new Exception("Error, Invalid time");
                if (startpro >= endpro)
                    throw new Exception("Error, Invalid time");

                try
                {


                    if (lstProgram.SelectedIndex != -1 && ((Program)lstProgram.SelectedItem).Name.Trim() != "")
                    {
                        

                        
                        //edit
                        List<Program> temp_program = copylist(p);
                        temp_program.Remove(((Program)lstProgram.SelectedItem));
                        if (ChkClash(temp_program, startpro, endpro))
                        {
                            throw new Exception("Error, clashed!");
                        }
                        ((Program)lstProgram.SelectedItem).StartDateTime = startpro;
                        ((Program)lstProgram.SelectedItem).EndDateTime = endpro;

                        ((Program)lstProgram.SelectedItem).Name = txtName.Text;
                        ((Program)lstProgram.SelectedItem).Location = txtLocation.Text;
                        ((Program)lstProgram.SelectedItem).Description = txtDescription.Text;


                    }
                    else
                    {
                        //asa
                        //    //add temp
                        Program tempp = new Program();
                        tempp.StartDateTime = startpro;
                        tempp.EndDateTime = endpro;
                        tempp.Name = txtName.Text;
                        tempp.Description = txtDescription.Text;
                        tempp.Location = txtLocation.Text;

                        //List<Program> temp_program = copylist(p);
                        //temp_program.Add(tempp);
                        if (ChkClash(p, startpro, endpro))
                        {
                            throw new Exception("Error, clashed!");
                        }
                        //chk clash

                        if (SegmentStartDateTime > startpro || SegmentEndDateTime < endpro)
                            throw new Exception("Error, Invalid time");
                        //add
                        
                        p.Add(tempp);

                        //txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text
                    }


                    MessageBox.Show("Operation succeeded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    clearAll();
                    loadPrograms(_day[cboDay.SelectedIndex]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                
            }
        }
        private void lstProgram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstProgram.SelectedIndex == -1)
            {
                clearAll();
                btnAdd.Content = "Add";
                return;
            }
            try
            {
                //ProgrammeHelper client = new ProgrammeHelper();
                Program selectedProgram = (Program)lstProgram.SelectedItem;
                txtName.Text = selectedProgram.Name;
                txtLocation.Text = selectedProgram.Location;
                cboStartHr.SelectedIndex = selectedProgram.StartDateTime.Hour;
                cboStartMin.SelectedIndex = selectedProgram.StartDateTime.Minute / 30;
                if (selectedProgram.EndDateTime.Date != selectedProgram.StartDateTime.Date)
                    cboEndHr.SelectedIndex = cboEndHr.Items.Count - 1;
                else
                    cboEndHr.SelectedIndex = selectedProgram.EndDateTime.Hour;
                cboEndMin.SelectedIndex = selectedProgram.EndDateTime.Minute / 30;
                txtDescription.Text = selectedProgram.Description;
                txtLocation.Text = selectedProgram.Location;
                cboBookDuration.SelectedIndex = computeDurationIdx(selectedProgram) - 1;
                btnAdd.Content = "Save";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int computeDurationIdx(Program p)
        {
            TimeSpan ts = p.EndDateTime.Subtract(p.StartDateTime);
            return (int)(ts.TotalMinutes / 30);
        }

        private bool Check_OverWrite(List<Program> programList)
        {
            
            EventDay d = _day[cboDay.SelectedIndex];

            for (int i = 0; i < programList.Count; i++)
            {
                if (programList[i].StartDateTime.CompareTo(GetStartDateTime(d)) < 0)
                    return true;
                if (programList[i].EndDateTime.CompareTo(GetEndDateTime(d)) > 0)
                    return true;
                for (int j = 0; j < programList.Count; j++)
                {
                    if (i == j)
                        continue;

                    if ((programList[i].StartDateTime.CompareTo(programList[j].StartDateTime) >= 0
                        && programList[i].StartDateTime.CompareTo(programList[j].EndDateTime) < 0)
                        ||
                        (programList[i].EndDateTime.CompareTo(programList[j].StartDateTime) > 0
                        && programList[i].EndDateTime.CompareTo(programList[j].EndDateTime) <= 0)
                        )
                        return true;
                }
            }
            return false;
        }

        private void Program_Swap(Program p1, Program p2)
        {
            EventDay d = _day[cboDay.SelectedIndex];
            DateTime SegmentStartDateTime = GetStartDateTime(d);
            DateTime SegmentEndDateTime = GetEndDateTime(d);

            List<Program> p = _programs[cboDay.SelectedIndex];
            List<Program> temp_program = copylist(p);

            temp_program.Remove(p1);
            temp_program.Remove(p2);

            DateTime tempstart = p1.StartDateTime;

            TimeSpan p1ts = p1.EndDateTime - p1.StartDateTime;
            TimeSpan p2ts = p2.EndDateTime - p2.StartDateTime;

            Program first = new Program(), second = new Program();


            first.StartDateTime = p2.StartDateTime;
            first.EndDateTime = p2.StartDateTime.AddMinutes(p1ts.TotalMinutes);
            first.Name = p1.Name;
            first.Location = p1.Location;
            first.Description = p1.Description;


            second.StartDateTime = tempstart;
            second.EndDateTime = tempstart.AddMinutes(p2ts.TotalMinutes);
            second.Name = p2.Name;
            second.Location = p2.Location;
            second.Description = p2.Description;

            if(first.Name.Trim() != "")
            {
            if(ChkClash(temp_program, first.StartDateTime, first.EndDateTime))
                throw new Exception("Error!, time clash");

            temp_program.Add(first);
            }

            if(second.Name.Trim() != "")
            {
            if(ChkClash(temp_program, second.StartDateTime, second.EndDateTime))
                throw new Exception("Error!, time clash");
            temp_program.Add(second);
            }

            if (SegmentStartDateTime > first.StartDateTime || SegmentEndDateTime < first.EndDateTime)
                throw new Exception("Error, Invalid time");
            if (first.StartDateTime >= first.EndDateTime)
                throw new Exception("Error, Invalid time");

            if (SegmentStartDateTime > second.StartDateTime || SegmentEndDateTime < second.EndDateTime)
                throw new Exception("Error, Invalid time");
            if (second.StartDateTime >= second.EndDateTime)
                throw new Exception("Error, Invalid time");

            ////

            //List<Program> temp = new List<Program>();

            //if (p1.Name.Trim() != "")
            //    temp.Add(p1);

            //if (p2.Name.Trim() != "")
            //    temp.Add(p2);

            //for (int i = 0; i < lstProgram.Items.Count; i++)
            //{
            //    if (((Program)lstProgram.Items[i]).ProgramID != 0 &&
            //        ((Program)lstProgram.Items[i]).ProgramID != p1.ProgramID &&
            //        ((Program)lstProgram.Items[i]).ProgramID != p2.ProgramID)
            //        temp.Add((Program)lstProgram.Items[i]);
            //}

            p.Remove(p1);
            p.Remove(p2);
            

            if(first.Name != "")
            p.Add(first);
            if (second.Name != "")
            p.Add(second);




            if (Check_OverWrite(temp_program))
            {
                MessageBox.Show("OverLap or is over the event time boundary");
                return;
            }

            //try
            //{
               
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            loadPrograms(_day[cboDay.SelectedIndex]);
        }

       

        private void clearAll()
        {
            lstProgram.SelectedIndex = -1;
            txtLocation.Text = "";
            txtName.Text = "";
            cboStartHr.SelectedIndex = 0;
            cboStartMin.SelectedIndex = 0;
            cboEndHr.SelectedIndex = 0;
            cboEndMin.SelectedIndex = 0;
            txtDescription.Text = "";
            cboBookDuration.SelectedIndex = 0;
            btnAdd.Content = "Add";
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {

            lstProgram.SelectedIndex = -1;
            clearAll();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstProgram.SelectedIndex == -1)
                return;
            try
            {
                Program pDel = ((Program)lstProgram.SelectedItem);
                List<Program> p = _programs[cboDay.SelectedIndex];
                for (int i = 0; i < p.Count; i++)
                {
                    if (p[i].StartDateTime == pDel.StartDateTime && p[i].EndDateTime == pDel.EndDateTime)
                    {
                        p.RemoveAt(i);

                    }
                }
                loadPrograms(_day[cboDay.SelectedIndex]);
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadPrograms(_day[cboDay.SelectedIndex]);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            loadPrograms(_day[cboDay.SelectedIndex]);
        }
        int oldIndex = -1;

        ListViewItem GetListViewItem(int index)
        {
            if (lstProgram.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return lstProgram.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }
        bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        {
            if (target == null)
                return false;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }
        int GetCurrentIndex(GetPositionDelegate getPosition)
        {

            int index = -1;
            for (int i = 0; i < this.lstProgram.Items.Count; ++i)
            {
                ListViewItem item = GetListViewItem(i);
                if (this.IsMouseOverTarget(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void lstProgram_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                double i = TranslatePoint(new Point(0, 0), lstProgram).Y;
                i = Math.Abs(i);


                if (e.GetPosition(this).Y <= i + 10)// && e.GetPosition(this).Y >= i)
                {
                    int aaa = this.GetCurrentIndex(e.GetPosition);
                    if (this.GetCurrentIndex(e.GetPosition) != 0)
                    {
                        lstProgram.ScrollIntoView(lstProgram.Items[this.GetCurrentIndex(e.GetPosition) - 1]);

                    }
                }

                else if (e.GetPosition(this).Y >= i - 10)// && e.GetPosition(this).Y <= i)
                {
                    double zz = e.GetPosition(this).Y;
                    if (this.GetCurrentIndex(e.GetPosition) != lstProgram.Items.Count - 1)
                    {
                        lstProgram.ScrollIntoView(lstProgram.Items[this.GetCurrentIndex(e.GetPosition) + 1]);

                    }
                }
            }
            catch { }
        }

        private void lstProgram_Drop(object sender, DragEventArgs e)
        {
            if (oldIndex < 0)
                return;

            int index = this.GetCurrentIndex(e.GetPosition);

            if (index < 0)
                return;

            if (index == oldIndex)
                return;

            Program_Swap((Program)lstProgram.Items[oldIndex], (Program)lstProgram.Items[index]);

            loadPrograms(_day[cboDay.SelectedIndex]);
        }

        private void lstProgram_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            oldIndex = this.GetCurrentIndex(e.GetPosition);

            if (oldIndex < 0)
                return;

            lstProgram.SelectedIndex = oldIndex;
            Program selectedItem = this.lstProgram.Items[oldIndex] as Program;

            if (selectedItem == null)
                return;

            // this will create the drag "rectangle"
            DragDropEffects allowedEffects = DragDropEffects.Move;
            if (DragDrop.DoDragDrop(this.lstProgram, selectedItem, allowedEffects) != DragDropEffects.None)
            {
                // The item was dropped into a new location,
                // so make it the new selected item.
                this.lstProgram.SelectedItem = selectedItem;
            }
        }

        private void cboStartHr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboStartHr.SelectedValue.ToString() == "24")
            {
                cboStartMin.SelectedIndex = 0;
                cboStartMin.IsEnabled = false;
            }
            else
                cboStartMin.IsEnabled = true;
            loadDuration();
        }

        private void cboEndHr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboEndHr.SelectedValue.ToString() == "24")
            {
                cboEndMin.SelectedIndex = 0;
                cboEndMin.IsEnabled = false;
            }
            else
                cboEndMin.IsEnabled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDTPData();
            lstProgram.SelectedValuePath = "ProgramId";
            loadPrograms();
            cboDay.Focus();
            cboStartHr.SelectedIndex = 0;
            cboEndHr.SelectedIndex = 0;
            cboStartMin.SelectedIndex = 0;
            cboEndMin.SelectedIndex = 0;
        }

        private void cboDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboDay.SelectedIndex == -1)
            {
                lbldaydate.Content = "";
            }
            else
            {
                lbldaydate.Content = _event.StartDateTime.AddDays(cboDay.SelectedIndex).ToShortDateString();
                loadPrograms(_day[cboDay.SelectedIndex]);
            }
        }

        public override bool Save()
        {
             clone(_programs,_programs_Original);


            return true;
        }

        private void cboBookDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime starttime = _event.StartDateTime.AddDays(cboDay.SelectedIndex).Date.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()))
                .AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
            DateTime endtime = getEndDateTime(starttime);
            txtEndTime.Text = endtime.ToString("dd MMM yyyy HH:mm");
        }

        private DateTime getEndDateTime(DateTime startTime)
        {
            int i = cboBookDuration.SelectedIndex + 1;
            TimeSpan duration = new TimeSpan(0, i * 30, 0);
            DateTime bookingEndDateTime = startTime.Add(duration);
            return bookingEndDateTime;
        }

        private void loadDuration()
        {
            int pidx = cboBookDuration.SelectedIndex;
            cboBookDuration.Items.Clear();
            if (cboStartHr.SelectedIndex == -1 || cboStartMin.SelectedIndex == -1)
            {
                cboBookDuration.IsEnabled = false;
                return;
            }
            cboBookDuration.IsEnabled = true;
            int hr;
            int.TryParse(cboStartHr.SelectedValue.ToString(), out hr);
            int min;
            int.TryParse(cboStartMin.SelectedValue.ToString(), out min);
            int maxIdx = (24 - hr) * 2;
            if (min > 0)
            {
                maxIdx -= 1;
            }


            //TimeSpan duration = new TimeSpan(0,0,0);

            for (int i = 0; i <= maxIdx; i++)
            {
                TimeSpan duration = new TimeSpan(0, i * 30, 0);
                if (i == 48)
                    cboBookDuration.Items.Add(string.Format("{0:00}", 24) + " H " + string.Format("{0:00}", 0) + " Min");
                else
                    cboBookDuration.Items.Add(string.Format("{0:00}", duration.Hours) + " H " + string.Format("{0:00}", duration.Minutes) + " Min");

                //duration.Add(new TimeSpan(0, 30, 0));
            }
            cboBookDuration.Items.RemoveAt(0);
            cboBookDuration.SelectedIndex = 0;
            if (pidx != -1)
                cboBookDuration.SelectedIndex = pidx;
            cboBookDuration.IsEnabled = true;
        }

        private void cboStartMin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadDuration();
        }
    }
}
