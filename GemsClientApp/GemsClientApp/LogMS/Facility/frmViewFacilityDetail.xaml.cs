using System;
using System.Windows;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmViewFacilityDetail.xaml
    /// </summary>
    public partial class frmViewFacilityDetail : Window
    {
        Facility fac;
        Window mainFrame;

        public frmViewFacilityDetail()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        public frmViewFacilityDetail(Window w, Facility fac)
            : this()
        {
            this.fac = fac;
            this.mainFrame = w;
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainFrame.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtBookingCon.Text = fac.BookingContact;
            txtFaculty.Text = fac.Faculty.ToString().Replace("_", " ");
            txtLocation.Text = fac.Location;
            txtSeatCapacity1.Text = fac.Capacity.ToString();
            txtTechContact.Text = fac.TechContact;
            txtVenue.Text = fac.FacilityID;
       }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}