using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmProgramList.xaml
    /// </summary>

    delegate Point GetPositionDelegate(IInputElement element);
    /// 

    public partial class frmProgramManagement : GEMSPage
    {

        User user;
        Event event_;

        public frmProgramManagement()
        {
            InitializeComponent();

            lstProgram.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(lstProgram_PreviewMouseLeftButtonDown);
            lstProgram.Drop += new DragEventHandler(lstProgram_Drop);

            lstProgram.DragOver += new DragEventHandler(lstProgram_DragOver);
        }

        public void CreateDTPData()
        {
            for (int i = 0; i <= 23; i++)
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

        public frmProgramManagement(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDTPData();
            txtDate.Content = event_.StartDateTime.ToString("dd MMM yyyy");
            lstProgram.SelectedValuePath = "ProgramId";
            loadPrograms();
        }

        private void loadPrograms()
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                List<Program> progList = client.ViewProgram(event_.EventID).ToList<Program>();

                client.Close();
                DateTime curr = event_.StartDateTime;
                DateTime end = event_.EndDateTime;

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

                lstProgram.ItemsSource = newprogList.OrderBy(x => x.StartDateTime)
                                                 .ThenBy(x => x.EndDateTime).ToList<Program>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (validateInputs())
            {
                DateTime SegmentStartDateTime = event_.StartDateTime.Date;
                DateTime SegmentEndDateTime = event_.StartDateTime.Date;

                SegmentStartDateTime = SegmentStartDateTime
                    .AddHours(int.Parse(cboStartHr.SelectedValue.ToString()))
                    .AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));

                SegmentEndDateTime = SegmentEndDateTime
                    .AddHours(int.Parse(cboEndHr.SelectedValue.ToString()))
                    .AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));

                try
                {
                    WCFHelperClient client = new WCFHelperClient();
                    bool clashed = client.ValidateProgramTime(event_.EventID, SegmentStartDateTime, SegmentEndDateTime);

                    if (clashed)
                    {
                        MessageBox.Show("Program cannot be overlapped!",
                        "Overlapping Program Detected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    else
                    {
                        if (lstProgram.SelectedIndex != -1 && ((Program)lstProgram.SelectedItem).ProgramID != 0)
                            client.EditProgram(user, ((Program)lstProgram.SelectedItem).ProgramID, txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text);

                        else
                            client.AddProgram(user, txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text, event_.EventID);
                    }

                    client.Close();

                    MessageBox.Show("Operation succeeded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    clearAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                loadPrograms();
            }
        }


        private bool Check_OverWrite(List<Program> programList)
        {

            for (int i = 0; i < programList.Count; i++)
            {
                if (programList[i].StartDateTime.CompareTo(event_.StartDateTime) < 0)
                    return true;
                if (programList[i].EndDateTime.CompareTo(event_.EndDateTime) > 0)
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

            DateTime tempstart;
            tempstart = p1.StartDateTime;

            TimeSpan p1ts = p1.EndDateTime - p1.StartDateTime;
            TimeSpan p2ts = p2.EndDateTime - p2.StartDateTime;

            p1.StartDateTime = p2.StartDateTime;
            p1.EndDateTime = p2.StartDateTime.AddMinutes(p1ts.TotalMinutes);

            p2.StartDateTime = tempstart;
            p2.EndDateTime = tempstart.AddMinutes(p2ts.TotalMinutes);

            List<Program> temp = new List<Program>();

            if (p1.ProgramID != 0)
                temp.Add(p1);

            if (p2.ProgramID != 0)
                temp.Add(p2);

            for (int i = 0; i < lstProgram.Items.Count; i++)
            {
                if (((Program)lstProgram.Items[i]).ProgramID != 0 &&
                    ((Program)lstProgram.Items[i]).ProgramID != p1.ProgramID &&
                    ((Program)lstProgram.Items[i]).ProgramID != p2.ProgramID)
                    temp.Add((Program)lstProgram.Items[i]);
            }

            if (Check_OverWrite(temp))
            {
                MessageBox.Show("OverLap or is over the event time boundary");
                return;
            }

            try
            {
                WCFHelperClient client = new WCFHelperClient();
                if (p1.ProgramID != 0)
                    client.EditProgram(user, p1.ProgramID, p1.Name, p1.StartDateTime, p1.EndDateTime, p1.Description);

                if (p2.ProgramID != 0)
                    client.EditProgram(user, p2.ProgramID, p2.Name, p2.StartDateTime, p2.EndDateTime, p2.Description);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            loadPrograms();
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
                WCFHelperClient client = new WCFHelperClient();
                Program selectedProgram = (Program)lstProgram.SelectedItem;
                txtName.Text = selectedProgram.Name;
                cboStartHr.SelectedIndex = selectedProgram.StartDateTime.Hour;
                cboStartMin.SelectedIndex = selectedProgram.StartDateTime.Minute / 30;
                cboEndHr.SelectedIndex = selectedProgram.EndDateTime.Hour;
                cboEndMin.SelectedIndex = selectedProgram.EndDateTime.Minute / 30;
                txtDescription.Text = selectedProgram.Description;
                btnAdd.Content = "Save";
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearAll()
        {

            txtName.Text = "";
            cboStartHr.SelectedIndex = 0;
            cboStartMin.SelectedIndex = 0;
            cboEndHr.SelectedIndex = 0;
            cboEndMin.SelectedIndex = 0;
            txtDescription.Text = "";
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
                WCFHelperClient client = new WCFHelperClient();
                client.DeleteProgram(user, ((Program)lstProgram.SelectedItem).ProgramID);
                client.Close();
                MessageBox.Show("Operation succeeded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadPrograms();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            loadPrograms();
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

            loadPrograms();
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
    }
}
