using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;
using dragonz.actb.core;
using dragonz.actb.provider;
using System.Windows.Media;

namespace Gems.UIWPF
{
    public class UserDataProvider : IAutoCompleteDataProvider
    {
        public IEnumerable<string> GetItems(string textPattern)
        {
            textPattern = textPattern.Trim();
            if (textPattern.Length < 2)
            {
                return null;
            }
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                List<User> users = client.searchUser(textPattern, "").ToList<User>();
                client.Close();
                List<String> results = new List<string>();
                foreach (User user in users)
                    results.Add(user.Name.ToString() + " (" + user.userID.ToString() + ")");
                return results;
            }
            catch
            {
                return null;
            }
        }
    }
    /// <summary>
    /// Interaction logic for frmRoleList.xaml
    /// </summary>
    public partial class frmRoleList : Window
    {

        User user;
        Window mainFrame;
        Event event_;
        Dictionary<string, List<CheckBox>> checkBoxes;

        public frmRoleList()
        {
            this.InitializeComponent();
        }

        public frmRoleList(User u, frmMain f, Event e)
            : this()
        {
            this.user = u;
            this.mainFrame = f;
            this.event_ = e;
            this.checkBoxes = new Dictionary<string, List<CheckBox>>();
            accbUsers.AutoCompleteManager.DataProvider = new UserDataProvider();
            accbUsers.AutoCompleteManager.AutoAppend = true;

            try
            {
                WCFHelperClient client = new WCFHelperClient();
                List<Function> functions = client.ViewFunction().ToList<Function>();
                client.Close();
                TreeViewItem root = new TreeViewItem() { Header = "All Rights", IsExpanded = true };
                tvFunctions.Items.Add(root);
                string currGroup = "";
                TreeViewItem parent = null;
                foreach (Function function in functions)
                {
                    if (function.Grouping != currGroup)
                    {
                        currGroup = function.Grouping;
                        parent = new TreeViewItem() { Header = function.Grouping, IsExpanded = true };
                        root.Items.Add(parent);
                    }
                    parent.Items.Add(new TreeViewItem() { Header = function.Description, Tag = function.RightsEnum }); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstRole.SelectedValuePath = "RoleId";
            loadRoles();
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem root = (TreeViewItem)tvFunctions.Items[0];
            CheckBox chkRoot = (CheckBox)GetChildControl(root, "chk");
            chkRoot.IsThreeState = true;
            chkRoot.Tag = root.Header.ToString();
            checkBoxes.Add("", new List<CheckBox>() { chkRoot });
            foreach (TreeViewItem parent in root.Items)
            {
                CheckBox chkParent = (CheckBox)GetChildControl(parent, "chk");
                chkParent.IsThreeState = true;
                chkParent.Tag = parent.Header.ToString();
                List<CheckBox> list = new List<CheckBox>() { chkParent };
                checkBoxes.Add(parent.Header.ToString(), list);
                foreach (TreeViewItem child in parent.Items)
                {
                    CheckBox chkChild = (CheckBox)GetChildControl(child, "chk");
                    chkChild.Tag = Tuple.Create((EnumFunctions)child.Tag, parent.Header.ToString());
                    list.Add(chkChild);
                }
            }
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


        private UIElement GetChildControl(DependencyObject parentObject, string childName)
        {

            UIElement element = null;
            if (parentObject != null)
            {
                int totalChild = VisualTreeHelper.GetChildrenCount(parentObject);
                for (int i = 0; i < totalChild; i++)
                {
                    DependencyObject childObject = VisualTreeHelper.GetChild(parentObject, i);
                    if (childObject is FrameworkElement && ((FrameworkElement)childObject).Name == childName)
                    {
                        element = childObject as UIElement;
                        break;
                    }
                    element = GetChildControl(childObject, childName);
                    if (element != null) break;
                }
            }
            return element;
        }

        private void chkBoxClicked(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if (!chkBox.IsChecked.HasValue)
                chkBox.IsChecked = false;
        }

        private void chkBoxToggled(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            var par = chkBox.Parent;
            var par2 = VisualTreeHelper.GetParent(chkBox);
            bool? isChecked = chkBox.IsChecked;
            if (isChecked == null) return;
            if (chkBox.Tag is string)
            {
                if (chkBox.Tag.ToString().Equals("All Rights"))
                    foreach (var pair in checkBoxes)
                        foreach (CheckBox chkChild in pair.Value)
                            chkChild.IsChecked = isChecked;

                else
                    foreach (CheckBox chkChild in checkBoxes[(string)chkBox.Tag])
                        chkChild.IsChecked = isChecked;
            }
            else
            {
                List<CheckBox> chkBoxes = checkBoxes[((Tuple<EnumFunctions, string>)chkBox.Tag).Item2];
                CheckBox parent = chkBoxes[0];
                int checkedCount = 0;
                for (int i = 1; i < chkBoxes.Count; i++)
                    if (chkBoxes[i].IsChecked == true)
                        checkedCount++;
                if (checkedCount == 0)
                    parent.IsChecked = false;
                else if (checkedCount == chkBoxes.Count - 1)
                    parent.IsChecked = true;
                else
                    parent.IsChecked = null;
                checkedCount = 0;
                CheckBox root = checkBoxes[""][0];
                foreach (var pair in checkBoxes)
                    if (pair.Key != "")
                        if (pair.Value[0].IsChecked == true)
                            checkedCount += 666;
                        else if (pair.Value[0].IsChecked == null)
                            checkedCount -= 6;
                if (checkedCount == 0)
                    root.IsChecked = false;
                else if (checkedCount == (checkBoxes.Count - 1) * 666)
                    root.IsChecked = true;
                else
                    root.IsChecked = null;
            }
        }

        private void loadRoles()
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                lstRole.ItemsSource = client.ViewRoleUser(user, event_).ToList <Tuple<Role, string>>();
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
            if (txtPost.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Post name.");
                return;
            }
            if (accbUsers.Text.Trim() == "")
            {
                MessageBox.Show("Please select user.");
                return;
            }
            List<EnumFunctions> selectedFunctions = new List<EnumFunctions>();
            foreach (var pair in checkBoxes)
            {
                List<CheckBox> chkBoxes = pair.Value;
                for (int i = 1; i < pair.Value.Count; i++)
                    if (chkBoxes[i].IsChecked == true)
                       selectedFunctions.Add(((Tuple<EnumFunctions, string>)chkBoxes[i].Tag).Item1);
            }
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                if (lstRole.SelectedIndex == -1)
                    client.AddRole(user, accbUsers.Text.Substring(accbUsers.Text.LastIndexOf('(') + 1).TrimEnd(')'),
                        event_.EventID,txtPost.Text.Trim(), txtDescription.Text.Trim(), selectedFunctions.ToArray());
                else
                    client.EditRights(user, accbUsers.Text.Substring(accbUsers.Text.LastIndexOf('(') + 1).TrimEnd(')'),
                        (((Tuple<Role, string>)lstRole.SelectedItem)).Item1.RoleID, txtPost.Text.Trim(),
                        txtDescription.Text.Trim(), selectedFunctions.ToArray());
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadRoles();
        }

        private void lstRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRole.SelectedIndex == -1)
            {
                btnAdd.Content = "Add";
                return;
            }
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                Tuple<Role, string> selectedItem = ((Tuple<Role, string>)lstRole.SelectedItem);
                Role selectedRole = selectedItem.Item1;
                txtPost.Text = selectedRole.Post;
                txtDescription.Text = selectedRole.Description;
                accbUsers.AutoCompleteManager.UpdateText(selectedItem.Item2 + " (" + selectedRole.UserID + ")", false);
                btnAdd.Content = "Save";
                List<EnumFunctions> rights = client.GetRights(event_.EventID, selectedRole.UserID).ToList();
                foreach (var pair in checkBoxes)
                    foreach (CheckBox chkBox in pair.Value)
                        if (chkBox.Tag is Tuple<EnumFunctions, string>)
                            chkBox.IsChecked = rights.Contains(((Tuple<EnumFunctions, string>)chkBox.Tag).Item1);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearAll()
        {
            cbRoleTemplate.SelectedIndex = -1;
            lstRole.SelectedIndex = -1;
            txtPost.Text = "";
            txtDescription.Text = "";
            btnAdd.Content = "Add";
            accbUsers.Text = "";
            foreach (var pair in checkBoxes)
                foreach (CheckBox chkBox in pair.Value)
                    chkBox.IsChecked = false;
        }

        private void clearAll(object sender, RoutedEventArgs e)
        {
            clearAll();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstRole.SelectedIndex == -1)
                return;
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                client.DeleteRole(user, (((Tuple<Role, string>)lstRole.SelectedItem)).Item1.RoleID);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadRoles();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            loadRoles();
        }
    }
}
