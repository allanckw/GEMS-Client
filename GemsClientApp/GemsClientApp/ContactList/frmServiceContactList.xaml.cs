using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using evmsService.entities;
using System.Linq;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmServiceContactList.xaml
    /// </summary>
    public partial class frmServiceContactList : Page
    {
        User user;
        Event event_;
        public frmServiceContactList()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        public frmServiceContactList(User user, Event evnt)
            : this()
        {
            this.user = user;
            this.event_ = evnt;
            loadServices();
        }

        private void loadServices()
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                List<Service> serviceList = client.ViewService(user).ToList<Service>();

                client.Close();

                lstServiceList.ItemsSource = serviceList.OrderBy(x => x.Name)
                                                 .ThenBy(x => x.Notes)
                                                 .ToList<Service>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
