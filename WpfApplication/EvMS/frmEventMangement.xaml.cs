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
	/// <summary>
	/// Interaction logic for frmEventMangement.xaml
	/// </summary>
	public partial class frmEventMangement : Window
	{

        frmMain mainFrame;
        User user;
		public frmEventMangement()
		{
			this.InitializeComponent();

            for (int i = 0; i <= 23; i++)
            {
                if (i < 10)
                {
                    cboStartHr.Items.Add("0" + i.ToString());
                    cboEndHr.Items.Add("0" + i.ToString());
                }
                else
                {
                    cboStartHr.Items.Add(i.ToString());
                    cboEndHr.Items.Add(i.ToString());
                }
            }

            for (int i = 0; i <= 55; i += 5)
            {
                if (i < 10)
                {
                    cboStartMin.Items.Add("0" + i.ToString());
                    cboEndMin.Items.Add("0" + i.ToString());
                }
                else
                {
                    cboStartMin.Items.Add(i.ToString());
                    cboEndMin.Items.Add(i.ToString());
                }
            }
		}

        public frmEventMangement(User u, frmMain f)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
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