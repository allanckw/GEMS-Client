using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using WPFTaskbarNotifier;
using evmsService.entities;

namespace Gems.UIWPF
{

    public class NotifyObject
    {
        string message;
        public String Message
        {
            get { return message; }
            set { message = value; }
        }
    }
    /// <summary>
    /// This is a TaskbarNotifier that contains a list of Notificationss to be displayed.
    /// </summary>
    public partial class Notifier : TaskbarNotifier
    {
        User user;
        Window mainFrame;
        public Notifier()
        {
            InitializeComponent();
        }

        public Notifier(User u, frmMain f)
            :this()
        {
            user = u;
            mainFrame = f;
        }

        private ObservableCollection<NotifyObject> notifyContent;
        /// <summary>
        /// A collection of Notificationss that the main window can add to.
        /// </summary>
        public ObservableCollection<NotifyObject> NotifyContent
        {
            get
            {
                if (this.notifyContent == null)
                {
                    // Not yet created.
                    // Create it.
                    this.NotifyContent = new ObservableCollection<NotifyObject>();
                }

                return this.notifyContent;
            }
            set
            {
                this.notifyContent = value;
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;

            if (hyperlink == null)
                return;

            NotifyObject Notifications = hyperlink.Tag as NotifyObject;
            if (Notifications != null)
            {
                var notiForm = new frmNotificationList(user, mainFrame);
                //this.Visibility = Visibility.Collapsed;
                notiForm.ShowDialog();
            }
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            this.ForceHidden();
        }
    }
}