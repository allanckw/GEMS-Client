using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System;

namespace Gems.UIWPF
{


    /// <summary>
    /// Interaction logic for frmAssign.xaml
    /// </summary>
    public partial class frmGST : Window
    {
        User user;
        public frmGST()
        {
            InitializeComponent();
        }

        public frmGST(User u)
            : this()
        {
            this.user = u;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int gst;
                if (int.TryParse(txtGST.Text.Trim(), out gst))
                {
                    WCFHelperClient client = new WCFHelperClient();
                    client.UpdateGST(user, gst);
                    client.Close();

                    MessageBox.Show("GST have been added/updated", "Updated GST",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
       
                }
                else
                {
                    MessageBox.Show("GST Percentage must be numeric", "Invalid input", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WCFHelperClient client = new WCFHelperClient();
            this.txtGST.Text = client.GetGST().ToString();
            client.Close();
        }
    }
}
