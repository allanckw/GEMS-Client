using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for frmServiceContactList.xaml
	/// </summary>
	public partial class frmServiceContactList : Window
	{
        User user;
        Event event_;

		public frmServiceContactList(User u, Event e)
		{
			this.InitializeComponent();

            this.user = u;
            this.event_ = e;
			// Insert code required on object creation below this point.
		}

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadServices();
        }

        private void loadServices()
        {
            try
            {
                WCFHelperClient client = new WCFHelperClient();
                List<Service> serviceList = client.ViewService(user).ToList<Service>();


                client.Close();



                lstServiceList.ItemsSource = serviceList
                                                 .OrderBy(x => x.Name)
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