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

namespace Gems.UIWPF
{
    public enum EnumRoles
    {
        SystemAdmin = 0,
        LocationAdmin = 1,
        EventExco = 2,
        Nil = 3

    }

    /// <summary>
    /// Interaction logic for frmAssign.xaml
    /// </summary>
    public partial class frmAssign : Window
    {
        User user;
        
        string idToAssign;
        EnumRoles action;

        public frmAssign()
        {
            InitializeComponent();
        }

        public frmAssign(User u, string uid, EnumRoles x):this()
        {
            this.user = u;
            this.idToAssign = uid;
            this.action = x;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {


        }
    }
}
