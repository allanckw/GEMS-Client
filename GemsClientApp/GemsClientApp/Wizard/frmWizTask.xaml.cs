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
    /// Interaction logic for frmWizTask.xaml
    /// </summary>
    public partial class frmWizTask : GemsWizPage
    {
        private User user;
        private Events event_;
        private List<Task> tasks;
        public frmWizTask(frmWizard c)
        {
            user = c._user;
            event_ = c._event;
            tasks = c._task;
            InitializeComponent();
            loadExisting();
            txtTaskName.Focus();
        }

        private void loadExisting()
        {
            for (int i = 0; i < tasks.Count(); i++)
            {
                Task t = tasks[i];
                lstManageTasks.Items.Add(t);
            }

        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (txtTaskName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the task Name", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (dtpDueDate.SelectedDateTime == default(DateTime))
            {
                MessageBox.Show("Please select task's due date", "Invalid input",
                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //TasksHelper client = new TasksHelper();
            var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
            try
            {
                //client.CreateTask(user, event_.EventID, txtTaskName.Text.Trim(),
                    //textRange.Text.Trim(), dtpDueDate.SelectedDateTime);
                //int currIdx = cboRole.SelectedIndex;
                //cboRole.SelectedIndex = -1;
                //cboRole.SelectedIndex = currIdx;
                Task t = new Task();
                t.TaskName = txtTaskName.Text.Trim();
                t.TaskDesc = textRange.Text.Trim();
                t.DueDate = dtpDueDate.SelectedDateTime;
                lstManageTasks.Items.Add(t);
                tasks.Add(t);

                MessageBox.Show("Operation Succeeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //client.Close();
            LoadTasks();
        }

        private void btnUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (lstManageTasks.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a task to update!", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            Task t = (Task)lstManageTasks.SelectedItem;
            //TasksHelper client = new TasksHelper();
            var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
            try
            {
                //client.UpdateTask(user, event_.EventID, t.TaskID, txtTaskName.Text.Trim(), textRange.Text.Trim(),
                    //dtpDueDate.SelectedDateTime);
                //int currIdx = cboRole.SelectedIndex;
                //cboRole.SelectedIndex = -1;
                //cboRole.SelectedIndex = currIdx;
                t.TaskName = txtTaskName.Text;
                t.TaskDesc = textRange.Text;
                t.DueDate = dtpDueDate.SelectedDateTime;

                // to refresh the listview
                lstManageTasks.BeginInit();
                lstManageTasks.EndInit();

                MessageBox.Show("Operation Succeeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //client.Close();
            LoadTasks();
        }

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lstManageTasks.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a task to delete!", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //Task t = (Task)lstManageTasks.SelectedItem;
            //TasksHelper client = new TasksHelper();
            try
            {
                //client.DeleteTask(user, event_.EventID, t.TaskID);
                //int currIdx = cboRole.SelectedIndex;
                //cboRole.SelectedIndex = -1;
                //cboRole.SelectedIndex = currIdx;
                lstManageTasks.Items.RemoveAt(lstManageTasks.SelectedIndex);
                Save();
                MessageBox.Show("Operation Succeeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //client.Close();
            LoadTasks();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void lstManageTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstManageTasks.SelectedIndex == -1)
            {
                return;
            }
            txtDesc.Document.Blocks.Clear();
            Task task = (Task)lstManageTasks.SelectedItem;
            txtTaskName.Text = task.TaskName;
            txtDesc.AppendText(task.TaskDesc);
            dtpDueDate.SelectedDateTime = task.DueDate;
        }

        private void LoadTasks()
        {
            //TasksHelper client = new TasksHelper();
            //lstManageTasks.ItemsSource = lstOverviewAllTask.ItemsSource = client.GetTasksByEvent(event_.EventID);
            //client.Close();
            ClearAll();
        }

        private void ClearAll()
        {
            lstManageTasks.SelectedIndex = -1;
            //lstOverviewAllTask.SelectedIndex = -1;
            //ClearOverview();
            txtTaskName.Text = "";
            txtDesc.Document.Blocks.Clear();
            dtpDueDate.clear();
        }

        public override bool Save()
        {
            tasks.Clear();
            for (int i = 0; i < lstManageTasks.Items.Count; i++)
            {
                Task t = (Task)lstManageTasks.Items[i];
                tasks.Add(t);
            }

            return true;
        }
    }
}
