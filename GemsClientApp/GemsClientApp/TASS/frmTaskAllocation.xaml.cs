using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;
using evmsService.entities;
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
            List<clsTaskAllocate> AllTask = new List<clsTaskAllocate>(new clsTaskAllocate[] {
                    new clsTaskAllocate { Name="Task2", Description="Description 2", Balance=0},
                    new clsTaskAllocate { Name="Task3", Description="Description 3", Balance=0},
                 new clsTaskAllocate { Name="Task5", Description="Description 5", Balance=0},
                 new clsTaskAllocate { Name="Task6", Description="Description 6", Balance=0},
                 new clsTaskAllocate { Name="Task7", Description="Description 7", Balance=0},
                 new clsTaskAllocate { Name="Task9", Description="Description 9",Balance=0}});

            //Example of assigned Task to an individual
            //Get individual task list dependent on cboRole

            List<clsTaskAllocate> IndividualTask = new List<clsTaskAllocate>(new clsTaskAllocate[] {
                    new clsTaskAllocate { Name="Task1", Description="Description 1 dasdsasdasdasdasdasdasdasdasd", Balance=0 },
                     new clsTaskAllocate { Name="Task4", Description="Description 4", Balance=0},
                 new clsTaskAllocate { Name="Task8", Description="Description 8",Balance=0},
            });

            //<<Throw in List of Task to me>>
            //<<Throw in another list of Task Assigned to the person>>
            //This is only an example
            //List does not work with drag and drop
            //Need to think how we are going to save the changes or detect the changes
            //if not sure dunno how to do, just pass in that two list i will do it
            lstManageTasks.ItemsSource = clsTaskAllocate.GetTaskNotAssigned(AllTask, IndividualTask);

            lstAssignedTask.ItemsSource = clsTaskAllocate.GetTaskAssigned(IndividualTask);
        }

        //The 2 listbox should display the same things as what lstManageTask, i cant set ItemTemplate cos
        //There is already 1 in the usercontrol..
        //All Tasks = client.GetTasksByEvent(event_.EventID); Already done in load task
        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            if (cboRole.SelectedIndex != -1)
            {
                lstAssignedTask.ItemsSource = client.GetTaskByRole(event_.EventID, int.Parse(cboRole.SelectedValue.ToString()));
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
                //if (cboRoleCreate.SelectedIndex == -1)
                    client.CreateTask(user, event_.EventID, txtTaskName.Text.Trim(),
                        textRange.Text.Trim(), dtpDueDate.SelectedDateTime);
                //else
                //    client.CreateTask(user, event_.EventID, int.Parse(cboRoleCreate.SelectedValue.ToString()),
                //        txtTaskName.Text.Trim(), textRange.Text, dtpDueDate.SelectedDateTime);

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

    }
}