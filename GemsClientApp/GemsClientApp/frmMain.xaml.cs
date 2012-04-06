using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using evmsService.entities;
using System.Windows.Controls;
using System.Reflection;

namespace Gems.UIWPF
{
    //<summary>
    //Interaction logic for frmMain.xaml
    //</summary>
    public partial class frmMain : Window
    {
        User user;
        frmLogin mainFrame;
        private DispatcherTimer timer, hourlyTimer;
        private Notifier taskbarNotifier;
        private Type currPageType;
        private GEMSPage currPage;
        private int selectedIndex = -1;

        private static Dictionary<Type, Tuple<string, EnumFunctions[]>> pageFunctions = new Dictionary<Type, Tuple<string, EnumFunctions[]>>
        {
            { typeof(frmOverview), Tuple.Create("Overview", new EnumFunctions[] {})},
            { typeof(frmRoleTemplates), Tuple.Create("manage role templates", new EnumFunctions[] {
                EnumFunctions.Add_Role,
                EnumFunctions.Edit_Role,
                EnumFunctions.Delete_Role,
                EnumFunctions.View_Role
            })},
            { typeof(frmRoleList), Tuple.Create("manage roles", new EnumFunctions[] {
                EnumFunctions.Add_Role,
                EnumFunctions.Edit_Role,
                EnumFunctions.Delete_Role,
                EnumFunctions.View_Role
            })},
            { typeof(frmProgramManagement), Tuple.Create("edit programme", new EnumFunctions[] {
                EnumFunctions.Edit_Programmes,
                EnumFunctions.Create_Programmes,
                EnumFunctions.Delete_Programmes
            })},
            { typeof(frmItemManagement), Tuple.Create("add items", new EnumFunctions[] {
                EnumFunctions.Manage_ItemTypes,
                EnumFunctions.Manage_Items,
                EnumFunctions.OptimizeItemList,
            })},
            { typeof(frmBudgetItemList), Tuple.Create("manage budget item list", new EnumFunctions[] {
                EnumFunctions.Manage_Items
            })},
            { typeof(frmBudgetIncome), Tuple.Create("manage budget income", new EnumFunctions[] {
                EnumFunctions.Manage_Income
            })},
            { typeof(frmBudgetReport), Tuple.Create("view budget report", new EnumFunctions[] {
                EnumFunctions.View_Budget_Report
            })},
            { typeof(frmTaskAllocation), Tuple.Create("manage tasks", new EnumFunctions[] {
                EnumFunctions.Add_Task,
                EnumFunctions.Update_Task,
                EnumFunctions.Delete_Task,
                EnumFunctions.Assign_Task
            })},
            { typeof(frmViewTask), Tuple.Create("view task", new EnumFunctions[] {})},
            { typeof(frmGuestList), Tuple.Create("edit guests", new EnumFunctions[] {
                EnumFunctions.Add_Guest,
                EnumFunctions.Edit_Guest,
                EnumFunctions.Delete_Guest
            })},
            { typeof(frmServiceContactList), Tuple.Create("Service Contact List", new EnumFunctions[] {})},
            { typeof(frmFields), Tuple.Create("edit registration form", new EnumFunctions[] {
                EnumFunctions.Manage_Participant
            })},
            { typeof(frmStaticFields), Tuple.Create("view static fields", new EnumFunctions[] {
                EnumFunctions.Manage_Participant
            })},
            { typeof(frmPublishWebsite), Tuple.Create("publish website", new EnumFunctions[] {
                EnumFunctions.Manage_Participant
            })},
            { typeof(frmParticipants), Tuple.Create("view participant registration data", new EnumFunctions[] {
                EnumFunctions.Manage_Participant
            })}
        };

        public frmMain()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Start();

            hourlyTimer = new DispatcherTimer();
            hourlyTimer.Interval = TimeSpan.FromMinutes(59.99);
            hourlyTimer.Tick += new EventHandler(hourlyTimer1_Tick);
            hourlyTimer.Start();
        }

        public frmMain(User u, frmLogin mainFrame)
            : this()
        {
            this.user = u;
            this.mainFrame = mainFrame;
            currPageType = typeof(frmOverview);

            Loaded += new RoutedEventHandler(Window_Loaded);
            taskbarNotifier = new Notifier(user, this);
            taskbarNotifier.OpeningMilliseconds = 2000;
            taskbarNotifier.StayOpenMilliseconds = 4000;
            taskbarNotifier.HidingMilliseconds = 2000;
            this.taskbarNotifier.Show();

            getNewNotifications();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!user.isSystemAdmin)
            {
                this.mnuAdmin.Visibility = Visibility.Collapsed;
                this.mnuGlobalRoleTemplates.Visibility = Visibility.Collapsed;
            }
            if (!user.isFacilityAdmin)
            {
                this.mnuManageFac.Visibility = Visibility.Collapsed;
                this.mnuManageFacBookings.Visibility = Visibility.Collapsed;
            }
            getHourlyNotifications();
            notify();
            loadEvents();
            lstEventList.SelectedIndex = 0;
            Event ev = (Event)lstEventList.SelectedItem;
            frame.Navigate(new frmOverview(user, ev));
        }



        private void DisableAllRight()
        {
            mnuEvent.Visibility = Visibility.Collapsed;
            mnuItemManageEvent.Visibility = Visibility.Collapsed;
            //
            mnuLocation.Visibility = Visibility.Collapsed;
            mnuSearchFac.Visibility = Visibility.Collapsed;
            mnuViewBookings.Visibility = Visibility.Collapsed;
            //
            mnuPrograms.Visibility = Visibility.Collapsed;
            //
            mnuBudget.Visibility = Visibility.Collapsed;
            mnuManageItem.Visibility = Visibility.Collapsed;
            mnuManageBudgetItem.Visibility = Visibility.Collapsed;
            mnuBudgetReport.Visibility = Visibility.Collapsed;
            //Tasks and View Tasks Are Global
            mnuManageTasks.Visibility = Visibility.Collapsed;
            //
            mnuManpower.Visibility = Visibility.Collapsed;
            mnuRoles.Visibility = Visibility.Collapsed;
            mnuRoleTemplates.Visibility = Visibility.Collapsed;
            //
            mnuGuests.Visibility = Visibility.Collapsed;
            //
            mnuPublish.Visibility = Visibility.Collapsed;
        }

        private void EnableAllRight()
        {
            mnuEvent.Visibility = Visibility.Visible;
            mnuItemManageEvent.Visibility = Visibility.Visible;
            //
            mnuLocation.Visibility = Visibility.Visible;
            mnuSearchFac.Visibility = Visibility.Visible;
            mnuViewBookings.Visibility = Visibility.Visible;
            //
            mnuPrograms.Visibility = Visibility.Visible;
            //
            mnuBudget.Visibility = Visibility.Visible;
            mnuManageItem.Visibility = Visibility.Visible;
            mnuManageBudgetItem.Visibility = Visibility.Visible;
            mnuBudgetReport.Visibility = Visibility.Visible;
            //Tasks and View Tasks Are Global
            mnuManageTasks.Visibility = Visibility.Visible;
            //
            mnuManpower.Visibility = Visibility.Visible;
            mnuRoles.Visibility = Visibility.Visible;
            mnuRoleTemplates.Visibility = Visibility.Visible;
            //
            mnuGuests.Visibility = Visibility.Visible;
            //
            mnuPublish.Visibility = Visibility.Visible;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            loadEventsAuto();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void loadEvents()
        {
            if ((dtpFrom.SelectedDate == null && dtpTo.SelectedDate != null) ||
                (dtpFrom.SelectedDate != null && dtpTo.SelectedDate == null))
            {
                MessageBox.Show("Invalid Date Range");
            }
            else
            {
                try
                {
                    WCFHelperClient client = new WCFHelperClient();
                    List<Event> list;
                    if (dtpFrom.SelectedDate == null && dtpTo.SelectedDate == null)
                        list = client.ViewEvent(user).ToList<Event>();

                    else
                        list = client.ViewEventsbyDate(user, dtpFrom.SelectedDate.Value,
                                        dtpTo.SelectedDate.Value).ToList<Event>();

                    lstEventList.ItemsSource = list;
                    client.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void loadEventsAuto()
        {

            bool nthselected = (lstEventList.SelectedIndex == -1);
            //SelectionChanged="lstEventList_SelectionChanged"

            lstEventList.SelectionChanged -= lstEventList_SelectionChanged;
            int Eid = 0;
            if (!nthselected)
            {
                Eid = ((Event)lstEventList.SelectedItem).EventID;
            }
            WCFHelperClient client = new WCFHelperClient();
            List<Event> list;

            //if valid date range.
            if (dtpFrom.SelectedDate != null && dtpTo.SelectedDate != null)
            {
                list = client.ViewEventsbyDate(user, dtpFrom.SelectedDate.Value,
                                dtpTo.SelectedDate.Value).ToList<Event>();
            }
            else
                list = client.ViewEvent(user).ToList<Event>();

            lstEventList.ItemsSource = list;
            client.Close();
            if (!nthselected)
            {
                for (int i = 0; i < lstEventList.Items.Count; i++)
                {
                    if (((Event)lstEventList.Items[i]).EventID == Eid)
                    {
                        lstEventList.SelectedIndex = i;
                        lstEventList.SelectionChanged += lstEventList_SelectionChanged;
                        return;
                    }
                }
            }
            lstEventList.SelectionChanged += lstEventList_SelectionChanged;
            //lstEventList.SelectedIndex = 0;
            //loadEventItems();
        }

        private void SetRight(List<EnumFunctions> ef)
        {
            DisableAllRight();

            if (user.isEventOrganizer || ef.Contains(EnumFunctions.Delete_Event)
                || ef.Contains(EnumFunctions.Edit_Event))
            {
                mnuEvent.Visibility = Visibility.Visible;
            }
            if (user.isEventOrganizer)
            {
                mnuLocation.Visibility = Visibility.Visible;
                mnuSearchFac.Visibility = Visibility.Visible;
                mnuViewBookings.Visibility = Visibility.Visible;
                mnuRoleTemplates.Visibility = Visibility.Visible;
            }
            //
            if (ef.Contains(EnumFunctions.Edit_Programmes) ||
                ef.Contains(EnumFunctions.Create_Programmes) ||
                ef.Contains(EnumFunctions.Delete_Programmes))
            {
                mnuPrograms.Visibility = Visibility.Visible;
            }
            //
            if (ef.Contains(EnumFunctions.Manage_ItemTypes) ||
                ef.Contains(EnumFunctions.Manage_Items) ||
                ef.Contains(EnumFunctions.OptimizeItemList))
            {
                mnuBudget.Visibility = Visibility.Visible;
                mnuManageItem.Visibility = Visibility.Visible;

            }
            if (ef.Contains(EnumFunctions.Manage_Items))
            {
                mnuManageBudgetItem.Visibility = Visibility.Visible;
            }
            if (ef.Contains(EnumFunctions.View_Budget_Report))
            {
                mnuBudgetReport.Visibility = Visibility.Visible;
            }
            //
            if (ef.Contains(EnumFunctions.Add_Task) ||
                ef.Contains(EnumFunctions.Update_Task) ||
                ef.Contains(EnumFunctions.Delete_Task) ||
                ef.Contains(EnumFunctions.Assign_Task))
            { //Tasks and View Tasks Are Global
                mnuManageTasks.Visibility = Visibility.Visible;
            }
            if (ef.Contains(EnumFunctions.Add_Guest) ||
                ef.Contains(EnumFunctions.Edit_Guest) ||
                ef.Contains(EnumFunctions.Delete_Guest))
            {
                mnuGuests.Visibility = Visibility.Visible;
            }
            //
            if (ef.Contains(EnumFunctions.Add_Role) ||
                ef.Contains(EnumFunctions.Edit_Role) ||
                ef.Contains(EnumFunctions.Delete_Role) ||
                ef.Contains(EnumFunctions.View_Role))
            {
                mnuManpower.Visibility = Visibility.Visible;
                mnuRoles.Visibility = Visibility.Visible;
            }
            //
            if (ef.Contains(EnumFunctions.Manage_Participant))
            {
                mnuPublish.Visibility = Visibility.Visible;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.taskbarNotifier != null)
            {
                this.getNewNotifications();
                notify();
            }
        }

        private void notify()
        {
            if (taskbarNotifier.NotifyContent.Count > 0)
                taskbarNotifier.Notify();
        }

        private void hourlyTimer1_Tick(object sender, EventArgs e)
        {
            if (this.taskbarNotifier != null)
            {
                this.getHourlyNotifications();
                notify();
            }
        }

        private void mnuItemAssSR_Click(object sender, RoutedEventArgs e)
        {
            var frmAssn = new frmSearchUsers(user, this);
            frmAssn.ShowDialog();
        }

        private void mnuItemViewUsers_Click(object sender, RoutedEventArgs e)
        {
            var viewUserForm = new frmViewUsers(user, this);
            viewUserForm.ShowDialog();
        }

        private void mnuNotify_Click(object sender, RoutedEventArgs e)
        {
            var frmNotif = new frmNotificationList(user, this);
            frmNotif.ShowDialog();
        }

        private void getNewNotifications()
        {
            WCFHelperClient client = new WCFHelperClient();
            taskbarNotifier.NotifyContent.Clear();
            string sender = client.GetNewMessage(user.userID);

            if (sender.Length > 0)
            {
                NotifyObject n = new NotifyObject();
                n.Message = "You have 1 new message from " + sender;
                taskbarNotifier.NotifyContent.Add(n);
            }
            client.Close();
        }

        private void getHourlyNotifications()
        {
            WCFHelperClient client = new WCFHelperClient();
            taskbarNotifier.NotifyContent.Clear();
            int noOfUnreadMsg = client.GetUnreadMessageCount(user.userID);

            if (noOfUnreadMsg > 0)
            {
                NotifyObject n = new NotifyObject();
                n.Message = "You have " + noOfUnreadMsg + " unread messages";
                taskbarNotifier.NotifyContent.Add(n);
            }
            client.Close();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.taskbarNotifier != null)
                this.taskbarNotifier.Close();
            this.taskbarNotifier = null;

            timer.Stop();
            base.Close();
        }

        private void mnuItemManageEvent_Click(object sender, RoutedEventArgs e)
        {
            var frmManageEvent = new frmEventMangement(user, this);
            frmManageEvent.ShowDialog();
        }

        private void mnuManageFac_Click(object sender, RoutedEventArgs e)
        {
            var frmFacManage = new frmManageFacility(user, this);
            frmFacManage.ShowDialog();
        }

        private void mnuGuests_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmGuestList>();
        }

        private void mnuProgram_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmProgramManagement>();
        }

        private void btnGetEvents_Click(object sender, RoutedEventArgs e)
        {
            loadEvents();
        }

        private void mnuSearchFac_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an event to book facility!", "No Event Selected!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                Event ev = (Event)lstEventList.SelectedItem;
                var frmFacSearch = new frmFacBooking(user, ev, this);
                frmFacSearch.ShowDialog();
            }
        }

        private void mnuManageItem_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmItemManagement>();
        }

        private void mnuManageFacBookings_Click(object sender, RoutedEventArgs e)
        {
            var frmManageBookings = new frmFacBookingAdmin(user, this);
            frmManageBookings.ShowDialog();
        }

        private void mnuRoles_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmRoleList>();
        }

        private void mnuRoleTemplates_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmRoleTemplates>();
        }

        private void mnuGlobalRoleTemplates_Click(object sender, RoutedEventArgs e)
        {
            currPageType = typeof(frmRoleTemplates);
            currPage = new frmRoleTemplates(user, null);
            frame.Navigate(currPage);
        }

        private void mnuViewBookings_Click(object sender, RoutedEventArgs e)
        {
            var frmViewBookings = new frmViewCurrentBooking(user, this);
            frmViewBookings.ShowDialog();
        }

        private bool navigate<T>()
        {
            Type newPageType = typeof(T);
            if (newPageType == typeof(frmServiceContactList))
            {
                //Do nothing
            }
            else
            {
                if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select an event to " + pageFunctions[newPageType].Item1 + "!", "No Event Selected!",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                if (currPage != null && currPage.isChanged())
                {
                    MessageBoxResult answer = MessageBox.Show("There are unsaved changes. Would you like to save your changes now?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if ((answer == MessageBoxResult.Yes && !currPage.saveChanges()) || answer == MessageBoxResult.Cancel)
                        return false;
                }
            }
            currPageType = typeof(T);
            Event ev = (Event)lstEventList.SelectedItem;
            if (pageFunctions[currPageType].Item2.Length > 0 && user.userID != ev.Organizerid && !user.isSystemAdmin)
            {
                try
                {
                    WCFHelperClient client = new WCFHelperClient();
                    foreach (EnumFunctions ef1 in client.GetRights(ev.EventID, user.userID).ToArray<EnumFunctions>())
                        foreach (EnumFunctions ef2 in pageFunctions[currPageType].Item2)
                            if (ef1 == ef2)
                            {
                                frame.Navigate(Activator.CreateInstance(currPageType, user, (Event)lstEventList.SelectedItem));
                                return true;
                            }
                    client.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                currPageType = typeof(frmOverview);
            }
            currPage = (GEMSPage)Activator.CreateInstance(currPageType, user, ev);
            frame.Navigate(currPage);
            return true;
        }

        private void lstEventList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstEventList.SelectedIndex == -1)
            {
                selectedIndex = -1;
                return;
            }
            if (selectedIndex == lstEventList.SelectedIndex)
                return;
            try
            {
                Event ev = (Event)lstEventList.SelectedItem;
                if (!(bool)typeof(frmMain)
                    .GetMethod("navigate", BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(currPageType)
                    .Invoke(this, null))
                {
                    lstEventList.SelectedIndex = selectedIndex;
                    return;
                }

                if (user.userID == ev.Organizerid)
                    EnableAllRight();
                else
                {
                    WCFHelperClient client = new WCFHelperClient();

                    if (user.userID == ev.Organizerid || user.isSystemAdmin)
                    {
                        EnableAllRight();
                    }
                    else if (user.isFacilityAdmin)
                    {
                        DisableAllRight();
                        mnuLocation.Visibility = Visibility.Visible;

                        mnuViewBookings.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SetRight(client.GetRights(ev.EventID, user.userID).ToList<EnumFunctions>());
                    }
                    client.Close();

                }
                selectedIndex = lstEventList.SelectedIndex;
            }
            catch (Exception ex)
            {
                DisableAllRight();
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuOverview_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmOverview>();
        }

        private void mnuPublishWebsite_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmPublishWebsite>();
        }

        private void mnuEditRegForm_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmFields>();
        }

        private void mnuStaticFields_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmStaticFields>();
        }

        private void mnuParticipants_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmParticipants>();
        }

        private void mnuContactList_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmServiceContactList>();
        }

        private void mnuManageBudgetItem_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmBudgetItemList>();
        }

        private void mnuTasks_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmTaskAllocation>();
        }

        private void mnuGST_Click(object sender, RoutedEventArgs e)
        {
            var gstForm = new frmGST(user);
            gstForm.ShowDialog();
        }

        private void mnuManageIncome_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmBudgetIncome>();
        }

        private void mnuViewTasks_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmViewTask>();
        }

        private void mnuBudgetReport_Click(object sender, RoutedEventArgs e)
        {
            navigate<frmBudgetReport>();
        }
    }
}