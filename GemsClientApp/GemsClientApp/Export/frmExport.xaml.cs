using System;
using System.Windows;
using System.Windows.Input;
using evmsService.entities;

namespace Gems.UIWPF
{
    public partial class frmExport : Window
    {
        private User user;
        private Events event_;

        public frmExport()
        {
            InitializeComponent();
        }

        public frmExport(User u, Events e)
            : this()
        {
            this.user = u;
            this.event_ = e;

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}

