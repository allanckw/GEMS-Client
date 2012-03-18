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
using System.Collections;

namespace Gems.UIWPF.BASS
{
    /// <summary>
    /// Interaction logic for frmAllocateBudget.xaml
    /// </summary>
    public partial class frmAllocateBudget : Window
    {
        private Window mainFrame;
        private User user;
        private Event event_;
        private List<Items> itemList;
        private List<ItemTypes> typeList;

        public frmAllocateBudget()
        {
            InitializeComponent();
        }

        public frmAllocateBudget(Window f, User u, Event e, List<ItemTypes> typeList, List<Items> itemList)
            : this()
        {
            this.mainFrame = f;
            this.user = u;
            this.event_ = e;
            this.typeList = typeList;
            this.itemList = itemList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //perform algorithm here

            //TODO:
            //Return a list of optimized algorithm.. 
            //A slider as mentioned
            //How you would like to display is up to your discretion
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
