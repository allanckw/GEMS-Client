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
    public partial class frmProgramManagement : Page
    {

        User user;
        Event event_;

        public frmProgramManagement()
        {
            InitializeComponent();
            
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
                lstProgram.ItemsSource = progList
                                                .OrderBy(x => x.StartDateTime)
                                                .ThenBy(x => x.EndDateTime)
                                                .ToList<Program>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter programme segment name.");
                return;
            }
            if (cboStartHr.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter starting hour of programme segment.");
                return;
            }
            if (cboStartMin.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter starting minute of programme segment.");
                return;
            }
            if (cboEndHr.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter ending hour of programme segment.");
                return;
            }
            if (cboEndMin.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter ending minute of programme segment.");
                return;
            }

            //DateTime SegmentStartDateTime = event_.StartDateTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
            //DateTime SegmentEndDateTime = event_.EndDateTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
            //SegmentEndDateTime = SegmentEndDateTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
            //SegmentEndDateTime = SegmentEndDateTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));

            DateTime SegmentStartDateTime = event_.StartDateTime.Date;

            DateTime SegmentEndDateTime = event_.EndDateTime.Date;

            SegmentStartDateTime = SegmentStartDateTime.AddHours(int.Parse(cboStartHr.SelectedValue.ToString()));
            SegmentEndDateTime = SegmentEndDateTime.AddHours(int.Parse(cboEndHr.SelectedValue.ToString()));
            SegmentStartDateTime = SegmentStartDateTime.AddMinutes(int.Parse(cboStartMin.SelectedValue.ToString()));
            SegmentEndDateTime = SegmentEndDateTime.AddMinutes(int.Parse(cboEndMin.SelectedValue.ToString()));

            if (SegmentStartDateTime < event_.StartDateTime)
            {
                MessageBox.Show("Event starts at " + event_.EndDateTime + ", programme segment must start after that.");
                return;
            }
            if (SegmentEndDateTime > event_.EndDateTime)
            {
                MessageBox.Show("Event ends at " + event_.EndDateTime + ", programme segment must end before that.");
                return;
            }
            if (SegmentEndDateTime <= SegmentStartDateTime)
            {
                MessageBox.Show("Programme segment's end time must be after its start time.");
                return;
            }
            //chk for overlap
            Program prog = (Program)lstProgram.SelectedItem;
            for (int i = 0; i < lstProgram.Items.Count; i++)
            {
                Program p= (Program)lstProgram.Items[i];
                if (
                    (SegmentStartDateTime >= p.StartDateTime && SegmentStartDateTime <= p.EndDateTime)
                    && (SegmentEndDateTime >= p.StartDateTime && SegmentEndDateTime <= p.EndDateTime)
                    )
                {
                    if (p != prog) //if they are the same 
                    {
                        MessageBox.Show("Programmes cannot overlap!");
                        return;
                    }
                    else
                    {
                        try
                        {
                            WCFHelperClient client = new WCFHelperClient();
                            if (lstProgram.SelectedIndex == -1)
                                client.AddProgram(user, txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text, event_.EventID);
                            else
                                client.EditProgram(user, prog.ProgramID, txtName.Text, SegmentStartDateTime, SegmentEndDateTime, txtDescription.Text);
                            client.Close();
                            MessageBox.Show("Operation succeeded!");
                            clearAll();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        loadPrograms();
                    }
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
    }
}
