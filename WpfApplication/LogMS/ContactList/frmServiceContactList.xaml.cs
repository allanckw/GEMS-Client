﻿using System;
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

namespace Gems.UIWPF
{
	/// <summary>
	/// Interaction logic for frmServiceContactList.xaml
	/// </summary>
	public partial class frmServiceContactList : Window
	{
		public frmServiceContactList()
		{
			this.InitializeComponent();
			
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
	}
}