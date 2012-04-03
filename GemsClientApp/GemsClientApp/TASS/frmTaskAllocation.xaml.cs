using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
using System.Windows.Controls;
using System.Data;
using Gems.UIWPF.CustomCtrl;

namespace Gems.UIWPF
{
    public partial class frmTaskAllocation : Page
    {
        private User user;
        private Event event_;

        public frmTaskAllocation()
        {
            this.InitializeComponent();
        }

        public frmTaskAllocation(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshTaskList();
            lstAllTask.ItemsSource = lstManageTasks.ItemsSource = event_.Tasks;
            LoadRoles();

        }

        private void LoadTasks()
        {
            WCFHelperClient client = new WCFHelperClient();
            lstAllTask.ItemsSource = lstManageTasks.ItemsSource = client.GetTasksByEvent(event_.EventID);
            client.Close();
            ClearAll();
        }

        private void LoadRoles()
        {
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                cboRole.DisplayMemberPath = "m_Item2";
                cboRole.SelectedValuePath = "m_Item1.RoleID";
                cboRole.ItemsSource =
                    client.ViewEventRoles(user, event_).ToList<TupleOfRolestringRsiwEt5l>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        public void refreshTaskList()
        {

            //Example of All Task created for the event
            //Throw in All Task Here
            List<Task> IndividualTask = new List<Task>();
            WCFHelperClient client = new WCFHelperClient();
            List<Task> AllTask = client.GetTasksByEvent(event_.EventID).ToList<Task>();
            if (cboRole.SelectedIndex != -1)
            {
                lstAllTask.IsEnabled = true;
                IndividualTask = client.GetTaskByRole(event_.EventID, int.Parse(cboRole.SelectedValue.ToString())).ToList<Task>();
            }
            else
            {
                lstAllTask.IsEnabled = false;
            }
            client.Close();

            lstAllTask.ItemsSource = clsTaskAllocate.GetTaskNotAssigned(AllTask, IndividualTask);

            lstAssignedTask.ItemsSource = clsTaskAllocate.GetTaskAssigned(IndividualTask);
        }

        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            if (cboRole.SelectedIndex != -1)
            {
                refreshTaskList();
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
            if (dtpDueDate.SelectedDateTime == null)
            {
                MessageBox.Show("Please select task's due date", "Invalid input",
                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            WCFHelperClient client = new WCFHelperClient();
            var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
            try
            {
                client.CreateTask(user, event_.EventID, txtTaskName.Text.Trim(),
                    textRange.Text.Trim(), dtpDueDate.SelectedDateTime);
                MessageBox.Show("Operation Succeeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            client.Close();
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
            WCFHelperClient client = new WCFHelperClient();
            var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
            try
            {
                client.UpdateTask(user, event_.EventID, t.TaskID, txtTaskName.Text.Trim(), textRange.Text.Trim(),
                    dtpDueDate.SelectedDateTime);
                MessageBox.Show("Operation Succeeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
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
            Task t = (Task)lstManageTasks.SelectedItem;
            WCFHelperClient client = new WCFHelperClient();
            try
            {
                client.DeleteTask(user, event_.EventID, t.TaskID);
                MessageBox.Show("Operation Succeeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
            client.Close();
            LoadTasks();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            lstManageTasks.SelectedIndex = -1;
            txtDesc.Document.Blocks.Clear();
            txtTaskName.Text = "";
            dtpDueDate.clear();
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

        private void saveAssignment()
        {
            //Use threading to stop system from "Hanging" as it may take a long time to save
            //as a list of objects are sent over via SOAP

            System.Threading.Thread thread = new System.Threading.Thread(
                new System.Threading.ThreadStart(
                delegate()
                {
                    System.Windows.Threading.DispatcherOperation
                    dispatcherOp = this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(delegate()
                    {
                        try
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            MessageBox.Show("Please wait while we process your request...");

                            WCFHelperClient client = new WCFHelperClient();
                            try
                            {
                                Task[] taskList = this.lstAssignedTask.ItemsSource.Cast<Task>().ToArray();
                                //MessageBox.Show(taskList[0].TaskName);
                                client.AssignTask(user, event_.EventID, int.Parse(cboRole.SelectedValue.ToString()), taskList);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error have occured: " + ex.Message, "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            client.Close();
                                                       
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error have occured: " + ex.Message, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                ));

                    dispatcherOp.Completed += new EventHandler(dispatcherOp_Completed);
                }
            ));

            thread.Start();
        }

        void dispatcherOp_Completed(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;

            MessageBox.Show("Your task assignment have been updated!",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            refreshTaskList();
        }

        
        private void btnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            saveAssignment();
        }

    }
}