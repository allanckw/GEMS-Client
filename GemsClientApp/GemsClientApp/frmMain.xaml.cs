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
            hourlyTimer.Interval = TimeSpan.FromMinutes(59.99); //remember to change to 60
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!user.isSystemAdmin)
            {
                this.mnuAdmin.Visibility =Visibility.Collapsed;
            }
            if (!user.isLocationAdmin)
            {
                this.mnuLocAdmin.Visibility = Visibility.Collapsed;
            }

            getHourlyNotifications();
            notify();
            loadEvents();
        }

        private void loadEvents()
        {
            WCFHelperClient client = new WCFHelperClient();
            List<Event> list = client.ViewEvent(user).ToList<Event>();
            lstEventList.ItemsSource = list;
            client.Close();
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
            var admForm = new frmSearchUsers(user, this);
            this.Visibility = Visibility.Collapsed;
            admForm.ShowDialog();
        }

        private void mnuItemViewUsers_Click(object sender, RoutedEventArgs e)
        {
            var viewUserForm = new frmViewUsers(user, this);
            this.Visibility = Visibility.Collapsed;
            viewUserForm.ShowDialog();
        }

        private void mnuNotify_Click(object sender, RoutedEventArgs e)
        {
            var notiForm = new frmNotificationList(user, this);
            this.Visibility = Visibility.Collapsed;
            notiForm.ShowDialog();
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

        private void mnuItemAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var addEventForm = new frmEventAdd(user, this);
            this.Visibility = Visibility.Collapsed;
            addEventForm.ShowDialog();
        }

        private void mnuItemManageEvent_Click(object sender, RoutedEventArgs e)
        {
            var manageEventForm = new  frmEventMangement(user, this);
            this.Visibility = Visibility.Collapsed;
            manageEventForm.ShowDialog();
        }

        private void mnuAddFac_Click(object sender, RoutedEventArgs e)
        {
            var facAdd = new frmAddFacility(user, this);
            this.Visibility = Visibility.Collapsed;
            facAdd.ShowDialog();
        }

        private void mnuManageFac_Click(object sender, RoutedEventArgs e)
        {
            var facManage = new frmManageFacility(user, this);
            this.Visibility = Visibility.Collapsed;
            facManage.ShowDialog();
        }

        private void mnuGuests_Click(object sender, RoutedEventArgs e)
        {
            if (lstEventList.Items.Count < 0 || lstEventList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an event to add guest!", "No Event Selected!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Event ev = (Event) lstEventList.SelectedItem;
                var frmGuestList= new frmGuestList(user, this, ev);
                this.Visibility = Visibility.Collapsed;
                frmGuestList.ShowDialog();
            }
        }

        private void btnGetEvents_Click(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            List<Event> list = client.viewEventsbyDate(user, dtpFrom.SelectedDate.Value,
                                dtpTo.SelectedDate.Value).ToList<Event>(); ;
            lstEventList.ItemsSource = list;
            client.Close();
        }

        private void mnuSearchFac_Click(object sender, RoutedEventArgs e)
        {
            var facSearch = new frmBookingFacility(user, this);
            this.Visibility = Visibility.Collapsed;
            facSearch.ShowDialog();
        }
    }
}