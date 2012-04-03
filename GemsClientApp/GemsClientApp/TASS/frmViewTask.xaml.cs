using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;
using evmsService.entities;
namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmViewTask.xaml
    /// </summary>
    public partial class frmViewTask : Page
    {
        private User user;
        private Event event_;

        public frmViewTask()
        {
            this.InitializeComponent();
        }

        public frmViewTask(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadUserRoles();
        }

        private void loadUserRoles()
        {
            WCFHelperClient client = new WCFHelperClient();
            cboRole.SelectedValuePath = "RoleID";
            cboRole.DisplayMemberPath = "Post";
            cboRole.ItemsSource = client.ViewUserEventRoles(user.userID, event_.EventID);
            client.Close();

            if (cboRole.Items.Count == 0)
            {
                //MessageBox.Show("You are currently not assigned any role in the system", "No Roles Assigned",
                //    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                cboRole.SelectedIndex = 0;
            }

            ClearAll();
        }

        private void loadTasks()
        {
            WCFHelperClient client = new WCFHelperClient();

            lstTask.ItemsSource = client.GetTaskByRole(event_.EventID,
                int.Parse(cboRole.SelectedValue.ToString()));

            client.Close();
            ClearAll();
        }

        private void lstTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstTask.SelectedIndex == -1)
            {
                return;
            }
            txtDesc.Document.Blocks.Clear();
            txtRemark.Document.Blocks.Clear();

            Task task = (Task)lstTask.SelectedItem;

            txtTaskName.Text = task.TaskName;
            txtDesc.AppendText(task.TaskDesc);
            dtpDueDate.SelectedDateTime = task.DueDate;

            WCFHelperClient client = new WCFHelperClient();

            TaskAssignment assn = client.GetTaskAssignment(task.TaskID, event_.EventID,
                int.Parse(cboRole.SelectedValue.ToString()));

            txtRemark.AppendText(assn.Remarks);
             chkIsCompleted.IsChecked = this.txtRemark.IsReadOnly = assn.IsCompleted;

            chkIsCompleted.IsEnabled = !assn.IsCompleted;

            if (!assn.IsRead)
            {
                client.SetTaskRead(task, int.Parse(cboRole.SelectedValue.ToString()));
            }
            client.Close();
        }

        private void ClearAll()
        {
            txtDesc.Document.Blocks.Clear();
            txtTaskName.Text = "";
            dtpDueDate.clear();
            txtRemark.Document.Blocks.Clear();
            chkIsCompleted.IsChecked = false;
            lstTask.SelectedIndex = -1;
        }


        private void chkIsCompleted_Click(object sender, RoutedEventArgs e)
        {

            if (chkIsCompleted.IsChecked == true)
            {
                if (MessageBox.Show("Are you sure this task is completed? It cannot be undone! ",
                "Confirm task completion...",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    WCFHelperClient client = new WCFHelperClient();
                    try
                    {
                        var textRange = new TextRange(txtDesc.Document.ContentStart, txtDesc.Document.ContentEnd);
                        Task task = (Task)lstTask.SelectedItem;

                        TaskAssignment assn = client.GetTaskAssignment(task.TaskID, event_.EventID,
                            int.Parse(cboRole.SelectedValue.ToString()));

                        client.SetTaskCompleted(task, assn.AssignedRoleID, textRange.Text.Trim());

                        MessageBox.Show("Operation Succeeded");
                        chkIsCompleted.IsEnabled = false;
                        txtRemark.IsReadOnly = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An Error have occured: " + ex.Message, "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    client.Close();
                }
                else
                {
                    chkIsCompleted.IsChecked = false;
                }
            }
        }

        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboRole.SelectedIndex == -1)
                MessageBox.Show("Please select a role to view your tasks", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                loadTasks();
        }

    }
}
