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
using System.Windows.Shapes;
using evmsService.entities;
using System.Windows.Threading;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        User user;
        frmLogin mainFrame;
        private DispatcherTimer timer;
        private Notifier taskbarNotifier;

        public frmMain()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(Window_Loaded);
            taskbarNotifier = new Notifier();
            taskbarNotifier.OpeningMilliseconds = 2000;
            taskbarNotifier.StayOpenMilliseconds = 4000;
            taskbarNotifier.HidingMilliseconds = 2000;
            this.taskbarNotifier.Show();
        }

        public frmMain(User u, frmLogin mainFrame)
            : this()
        {
            this.user = u;
            this.mainFrame = mainFrame;
            getAllNotifications();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Start();

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
                this.mnuAdmin.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.taskbarNotifier != null)
            {
                this.taskbarNotifier.Notify();
            }

        }

        private void mnuItemAssSR_Click(object sender, RoutedEventArgs e)
        {
            var admForm = new frmSysAdmin(user, this);
            this.Visibility = Visibility.Collapsed;
            admForm.Show();
        }

        private void getAllNotifications()
        {
            string title = "Test Title";
            string message = "Testing message :(";

            if ((title != string.Empty) && (message != string.Empty))
            {
                // The title and message are both not empty.

                // Add the new title and message to the TaskbarNotifier's content.
                this.taskbarNotifier.NotifyContent.Add(new NotifyObject(message, title));

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.taskbarNotifier != null)
                this.taskbarNotifier.Close();
            this.taskbarNotifier = null;

            timer.Stop();
            base.Close();
           
        }
    }
}