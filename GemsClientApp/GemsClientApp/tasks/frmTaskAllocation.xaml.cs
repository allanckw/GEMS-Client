using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
using evmsService.Controllers;
using System.Windows.Controls;
using System.Data;
using Gems.UIWPF.CustomCtrl;

namespace Gems.UIWPF
{
    public partial class frmTaskAllocation : GEMSPage
    {
        private User user;
        private Events event_;
        //List<Tuple<Role, string>> roleByEvent;
        List<RoleWithUser> roleByEvent;
        
        public frmTaskAllocation()
        {
            this.InitializeComponent();
        }

        public frmTaskAllocation(User u, Events e)
            : this()
        {
            this.user = u;
            this.event_ = e;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshTaskList();
            lstManageTasks.ItemsSource = lstOverviewAllTask.ItemsSource = event_.Tasks;
            LoadRoles();
            
        }

        private void LoadTasks()
        {
            TasksHelper client = new TasksHelper();
            lstManageTasks.ItemsSource = lstOverviewAllTask.ItemsSource = client.GetTasksByEvent(user.UserID, event_.EventID);
            client.Close();
            ClearAll();
        }

        private void LoadRoles()
        {
            RoleHelper client = new RoleHelper();
            try
            {
                cboRole.DisplayMemberPath = "user";
                cboRole.SelectedValuePath = "role.RoleID";
                roleByEvent = client.ViewEventRoles(user, event_).ToList<RoleWithUser>();//client.ViewEventRoles(user, event_).ToList<Tuple<Role, string>>();
                cboRole.ItemsSource = roleByEvent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
        }

        public void refreshTaskList()
        {
            if (cboRole.SelectedIndex == -1)
            {
                lstAllTask.IsEnabled = false;
                return;
            }
            //Example of All Task created for the event
            //Throw in All Task Here
            List<Task> IndividualTask = new List<Task>();
            TasksHelper client = new TasksHelper();
            List<Task> AllTask = client.GetTasksByEvent(user.UserID, event_.EventID).ToList<Task>();

            lstAllTask.IsEnabled = true;
            IndividualTask = client.GetTaskByRole(event_.EventID, int.Parse(cboRole.SelectedValue.ToString())).ToList<Task>();

            client.Close();

            lstAllTask.ItemsSource = clsTaskAllocate.GetTaskNotAssigned(AllTask, IndividualTask);

            lstAssignedTask.ItemsSource = clsTaskAllocate.GetTaskAssigned(IndividualTask, int.Parse(cboRole.SelectedValue.ToString()));
        }

        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //WCFHelperHelper client = new WCFHelperHelper();
            if (cboRole.SelectedIndex == -1)
            {
                lstAllTask.ItemsSource = null;
                lstAssignedTask.ItemsSource = null;
            }

            refreshTaskList();
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
            TasksHelper client = new TasksHelper();
            var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
            try
            {
                client.CreateTask(user, event_.EventID, txtTaskName.Text.Trim(),
                    textRange.Text.Trim(), dtpDueDate.SelectedDateTime);
                int currIdx = cboRole.SelectedIndex;
                cboRole.SelectedIndex = -1;
                cboRole.SelectedIndex = currIdx;
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
            TasksHelper client = new TasksHelper();
            var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
            try
            {
                client.UpdateTask(user, event_.EventID, t.TaskID, txtTaskName.Text.Trim(), textRange.Text.Trim(),
                    dtpDueDate.SelectedDateTime);
                int currIdx = cboRole.SelectedIndex;
                cboRole.SelectedIndex = -1;
                cboRole.SelectedIndex = currIdx;
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
            TasksHelper client = new TasksHelper();
            try
            {
                client.DeleteTask(user, event_.EventID, t.TaskID);
                int currIdx = cboRole.SelectedIndex;
                cboRole.SelectedIndex = -1;
                cboRole.SelectedIndex = currIdx;
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
            lstOverviewAllTask.SelectedIndex = -1;
            ClearOverview();
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

                            TasksHelper client = new TasksHelper();
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
            LoadTasks();
        }


        private void btnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            saveAssignment();
        }

        private void lstOverviewAllTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stkTaskAssignment.Visibility = Visibility.Collapsed;
            if (lstOverviewAllTask.SelectedIndex == -1)
            {
                return;
            }

            txtOverviewDesc.Document.Blocks.Clear();
            Task task = (Task)lstOverviewAllTask.SelectedItem;
            txtOverviewTaskName.Text = task.TaskName;
            txtOverviewDesc.AppendText(task.TaskDesc);
            dtpOverviewDueDate.SelectedDateTime = task.DueDate;
            pBarTaskProgress.Value = task.PercentageCompletion;
            SetGlowVisibility(pBarTaskProgress, Visibility.Collapsed);
            List<TaskAssignmentState> TAS = GetTaskAssignmentState(task.TasksAssignments);
            lvOverViewRoleView.ItemsSource = TAS;
        }

        void SetGlowVisibility(ProgressBar progressBar, Visibility visibility)
        {
            var glow = progressBar.Template.FindName("PART_GlowRect", progressBar) as FrameworkElement;
            if (glow != null) glow.Visibility = visibility;
        }

        private List<TaskAssignmentState> GetTaskAssignmentState(TaskAssignment[] tAssns)
        {
            List<TaskAssignmentState> lstTAS = new List<TaskAssignmentState>();
            foreach (TaskAssignment taskAssignment in tAssns)
            {
                foreach (RoleWithUser item in roleByEvent)
                {
                    if (item.role.RoleID == taskAssignment.AssignedRoleID)
                    {
                        //String name=
                        TaskAssignmentState newTAS = new TaskAssignmentState(taskAssignment, item.user);
                        lstTAS.Add(newTAS);
                    }
                }
            }
            return lstTAS;
        }

        private void chkIsCompleted_Checked(object sender, RoutedEventArgs e)
        {
            stkCompletedDate.Visibility = Visibility.Visible;
        }

        private void chkIsCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            stkCompletedDate.Visibility = Visibility.Collapsed;
        }

        private void lvOverViewRoleView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOverViewRoleView.SelectedIndex == -1)
                return;
            stkTaskAssignment.Visibility = Visibility.Visible;
            TaskAssignmentState tas = (TaskAssignmentState)lvOverViewRoleView.SelectedItem;
            chkIsCompleted.IsChecked = tas.TaskAssignment.IsCompleted;
            txtCompletedDate.Text = tas.TaskAssignment.CompletedDateTime.ToString("dd MMM yyyy HH:mm");
            txtOverviewRemark.Document.Blocks.Clear();
            txtOverviewRemark.AppendText(tas.TaskAssignment.Remarks);
        }

        private void chkIsCompleted_Click(object sender, RoutedEventArgs e)
        {
            int selectedTaskIdx = lstOverviewAllTask.SelectedIndex;
            Task task = (Task)lstOverviewAllTask.SelectedItem;
            TaskAssignmentState tas = (TaskAssignmentState)lvOverViewRoleView.SelectedItem;
            TasksHelper client = new TasksHelper();

            try
            {
                if (MessageBox.Show("Are you sure you want to change the status of completion? ",
               "Confirm Operation...",
               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    chkIsCompleted.IsChecked = tas.TaskAssignment.IsCompleted;
                    return;
                }
                var textRange = new TextRange(txtOverviewRemark.Document.ContentStart, txtOverviewRemark.Document.ContentEnd);

                if (chkIsCompleted.IsChecked == true)
                {
                    client.SetTaskCompleted(task, tas.TaskAssignment.AssignedRoleID, textRange.Text.Trim());
                }
                else
                {
                    client.SetTaskIncomplete(user, task, tas.TaskAssignment.AssignedRoleID, textRange.Text.Trim());
                }

                LoadTasks();
                ClearOverview();
                cboRole.SelectedIndex = -1;
                lstOverviewAllTask.SelectedIndex = selectedTaskIdx;
                MessageBox.Show("Operation Succeeded");
                
                //lvOverViewRoleView.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            client.Close();
        }

        private void ClearOverview()
        {
            txtOverviewTaskName.Text = "";
            txtOverviewRemark.Document.Blocks.Clear();
            txtOverviewDesc.Document.Blocks.Clear();
            lvOverViewRoleView.ItemsSource = null;
            dtpOverviewDueDate.clear();
            pBarTaskProgress.Value = 0;
        }
    }
}


