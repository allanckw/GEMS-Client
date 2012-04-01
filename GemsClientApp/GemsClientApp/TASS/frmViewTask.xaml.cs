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
using System.Windows.Navigation;
using System.Windows.Shapes;
using evmsService.entities;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmViewTask.xaml
    /// </summary>
    public partial class frmViewTask : Page
    {
        private User user;
        private Event event_;

        public frmViewTask()
        {
            this.InitializeComponent();
        }

        public frmViewTask(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;

        }
    }
}
