using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmProgramList.xaml
    /// </summary>
    public partial class frmProgramManagement : Window
    {

        User user;
        Window mainFrame;
        Event event_;

        public frmProgramManagement()
        {
            this.InitializeComponent();
            CreateDTPData();
        }

        public void CreateDTPData()
        {
            for (int i = 0; i <= 23; i++)
            {
                cboStartHr.Items.Add(string.Format("{0:00}", i));
                cboEndHr.Items.Add(string.Format("{0:00}", i));
            }

            for (int i = 0; i <= 55; i += 10)
            {
                cboStartMin.Items.Add(string.Format("{0:00}", i));
                cboEndMin.Items.Add(string.Format("{0:00}", i));
            }
        }

        public frmProgramManagement(User u, frmMain f, Event e)
            : this()
        {
            this.user = u;
            this.mainFrame = f;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstProgram.SelectedValuePath = "ProgramId";
            loadPrograms();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }

        private void loadPrograms()
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                lstProgram.ItemsSource = client.ViewProgram(user, event_.EventID)
                                                .OrderBy(x => x.StartDateTime)
                                                .ThenBy(x => x.EndDateTime)
                                                .ToList<Program>();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            clearAll();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter program segment name.");
                return;
            }
            if (cboStartHr.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter starting hour of program segment.");
                return;
            }
            if (cboStartMin.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter starting minute of program segment.");
                return;
            }
            if (cboEndHr.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter ending hour of program segment.");
                return;
            }
            if (cboEndMin.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter ending minute of program segment.");
                return;
            }
            DateTime SegmentStartDateTime = event_.StartDateTime.Date;
            DateTime SegmentEndDateTime = event_.EndDateTime.Date;
            SegmentStartDateTime = SegmentStartDateTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
            SegmentStartDateTime = SegmentStartDateTime.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
            SegmentEndDateTime = SegmentEndDateTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
            SegmentEndDateTime = SegmentEndDateTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));
            if (SegmentStartDateTime < event_.StartDateTime)
            {
                MessageBox.Show("Event starts at " + event_.EndDateTime + ", program segment must start after that.");
                return;
            }
            if (SegmentEndDateTime > event_.EndDateTime)
            {
                MessageBox.Show("Event ends at " + event_.EndDateTime + ", program segment must end before that.");
                return;
            }
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                if (lstProgram.SelectedIndex == -1)
                    client.AddProgram(user, txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text, event_.EventID);
                else
                    client.EditProgram(user, ((Program)lstProgram.SelectedItem).ProgramID, txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text);
                client.Close();
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
                btnAdd.Content = "Add";
                return;
            }
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                Program selectedProgram = (Program)lstProgram.SelectedItem;
                txtName.Text = selectedProgram.Name;
                cboStartHr.SelectedIndex = selectedProgram.StartDateTime.Hour;
                cboStartMin.SelectedIndex = selectedProgram.StartDateTime.Minute / 10;
                cboEndHr.SelectedIndex = selectedProgram.EndDateTime.Hour;
                cboEndMin.SelectedIndex = selectedProgram.EndDateTime.Minute / 10;
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
            lstProgram.SelectedIndex = -1;
            txtName.Text = "";
            cboStartHr.SelectedIndex = -1;
            cboStartMin.SelectedIndex = -1;
            cboEndHr.SelectedIndex = -1;
            cboEndMin.SelectedIndex = -1;
            txtDescription.Text = "";
            btnAdd.Content = "Add";
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadPrograms();
        }
    }
}
