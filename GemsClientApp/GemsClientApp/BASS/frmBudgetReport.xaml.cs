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
using evmsService.entities;

namespace Gems.UIWPF
{
	public partial class frmBudgetReport
	{
        User user;
        Event event_;

        public frmBudgetReport()
        {
            InitializeComponent();
        }

        public frmBudgetReport(User user, Event e)
            : this()
        {
            this.user = user;
            this.event_ = e;
        }
	}
}