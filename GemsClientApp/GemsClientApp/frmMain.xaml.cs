using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using evmsService.entities;

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

            Loaded += new RoutedEventHandler(Window_Loaded);
            taskbarNotifier = new Notifier(user, this);
            taskbarNotifier.OpeningMilliseconds = 2000;
            taskbarNotifier.StayOpenMilliseconds = 4000;
            taskbarNotifier.HidingMilliseconds = 2000;
            this.taskbarNotifier.Show();

            getNewNotifications();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!user.isSystemAdmin)
            {
                this.mnuAdmin.Visibility = Visibility.Collapsed;
            }
            if (!user.isFacilityAdmin)
            {
                this.mnuManageFac.Visibility = Visibility.Collapsed;
                this.mnuManageFacBookings.Visibility = Visibility.Collapsed;
            }
            DateTime d = DateTime.Now;

            dtpFrom.SelectedDate = dtpTo.SelectedDate = 
                d.AddHours(-d.Hour).AddMinutes(-d.Minute).AddSeconds(-d.Second);

            getHourlyNotifications();
            notify();
            loadEvents();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            loadEvents();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void loadEvents()
        {
            WCFHelperClient client = new WCFHelperClient();
            List<Event> list = client.ViewEvent(user).ToList<Event>();
            lstEventList.ItemsSource = list;
            client.Close();
            lstEventList.SelectedIndex = 0;
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
            this.Visibility = Visibility.Collapsed;
            frmAssn.ShowDialog();
        }

        private void mnuItemViewUsers_Click(object sender, RoutedEventArgs e)
        {
            var viewUserForm = new frmViewUsers(user, this);
            this.Visibility = Visibility.Collapsed;
            viewUserForm.ShowDialog();
        }

        private void mnuNotify_Click(object sender, RoutedEventArgs e)
        {
            var frmNotif = new frmNotificationList(user, this);
            this.Visibility = Visibility.Collapsed;
            frmNotif.ShowDialog();
        }

        private void getNewNotifications()
        {
            WCFHelperClient client = new WCFHelperClient();
            taskbarNotifier.NotifyContent.Clear();
            string sender = client.getNewMessage(user.userID);

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
            int noOfUnreadMsg = client.getUnreadMessageCount(user.userID);

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
            this.Visibility = Visibility.Collapsed;
            frmManageEvent.ShowDialog();
        }

        private void mnuManageFac_Click(object sender, RoutedEventArgs e)
        {
            var frmFacManage = new frmManageFacility(user, this);
            this.Visibility = Visibility.Collapsed;
            frmFacManage.ShowDialog();
        }

        private void mnuGuests_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an event to edit guests!", "No Event Selected!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Event ev = (Event)lstEventList.SelectedItem;
                var frmGuestList = new frmGuestList(user, this, ev);
                this.Visibility = Visibility.Collapsed;
                frmGuestList.ShowDialog();
            }
        }


        private void mnuProgram_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an event to edit program!", "No Event Selected!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Event ev = (Event)lstEventList.SelectedItem;
                var frmProgramManagement = new frmProgramManagement(user, this, ev);
                this.Visibility = Visibility.Collapsed;
                frmProgramManagement.ShowDialog();
            }
        }

        private void btnGetEvents_Click(object sender, RoutedEventArgs e)
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
                        list = client.viewEventsbyDate(user, dtpFrom.SelectedDate.Value,
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
                this.Visibility = Visibility.Collapsed;
                frmFacSearch.ShowDialog();
            }
        }

        private void mnuManageItem_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an event to add items!", "No Event Selected!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Event ev = (Event)lstEventList.SelectedItem;
                var frmManageItem = new frmItemManagement(user, ev, this);
                this.Visibility = Visibility.Collapsed;
                frmManageItem.ShowDialog();
            }
        }

        private void mnuManageFacBookings_Click(object sender, RoutedEventArgs e)
        {
            var frmManageBookings = new frmFacBookingAdmin(user, this);
            this.Visibility = Visibility.Collapsed;
            frmManageBookings.ShowDialog();
        }

        private void mnuRoles_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an event to manage roles!", "No Event Selected!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Event ev = (Event)lstEventList.SelectedItem;
                var frmRoleList = new frmRoleList(user, this, ev);
                this.Visibility = Visibility.Collapsed; 
                frmRoleList.ShowDialog();
            }
        }

        private void mnuViewBookings_Click(object sender, RoutedEventArgs e)
        {
            var frmViewBookings = new frmViewCurrentBooking(user, this);
            this.Visibility = Visibility.Collapsed;
            frmViewBookings.ShowDialog();
        }

    }
}