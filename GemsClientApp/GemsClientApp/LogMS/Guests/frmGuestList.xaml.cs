using System.Windows;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmGuestList.xaml
    /// </summary>
    public partial class frmGuestList : Window
    {

        User user;
        Window mainFrame;
        Event event_;

        public frmGuestList()
        {
            this.InitializeComponent();
        }

        public frmGuestList(User u, frmMain f, Event e)
            : this()
        {
            this.user = u;
            this.mainFrame = f;
            this.event_ = e;
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
    }
}
