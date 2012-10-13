using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;


namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmWizard.xaml
    /// </summary>
    public partial class frmWizard : Window
    {
        List<Button> listlabel;
        int curindex;

        public Events _event;
        public List<EventDay> _days;
        public List<List<Program>> _programs;
        public List<Items> _items;
        public List<ItemTypes> _itemTypes;
        public List<List<Guest>> _guests;
        public Publish _publish;
        public User _user;
        public List<Task> _task;
        GemsWizPage Curpage;

        
        public frmWizard(User u)
        {

            //_event = null;
            //_days = null;
            //_programs = null;
            //_items = null;
            //_itemTypes = null;
            //_guests = null;
            //_publish = null;
            _user = u;
            _event = new Events();
            _event.StartDateTime = DateTime.Now;
            _event.EndDateTime = DateTime.Now;
            _days = new List<EventDay>();
            _programs = new List<List<Program>>();
            _items = new List<Items>();
            _itemTypes = new List<ItemTypes>();
            _guests = new List<List<Guest>>();
            _publish = new Publish();
            _task = new List<Task>();

 
            this.InitializeComponent();

            listlabel = new List<Button>();
            
            //btnevent
            //btnevent
            listlabel.Add(btnevent);
            listlabel.Add(btnprogramme);
            listlabel.Add(btnitem);
            listlabel.Add(btntask);
            listlabel.Add(btnguest);
            listlabel.Add(btnpublish);
                
            
                
               
            Navigation_MouseClick(listlabel[0], null);
           // navigate<frmWizEvent>();
            //listlabel.Add(
            //InitializeComponent();
        }

        public int HighLight_Navigation(Button lbl)
        {
            int index=0;
            for (int i = 0; i < listlabel.Count; i++)
            {

                if (listlabel[i] == lbl)
                {
                    listlabel[i].Foreground = Brushes.Green;
                    //listlabel[i].Background = Brushes.Transparent;
                    //listlabel[i].BorderBrush = Brushes.Transparent;
                    
                    index = i;
                }
                else
                    listlabel[i].Foreground = Brushes.Black;

                
            }
            return index;
        }

        public int HighLight_Navigation(int index)
        {
            
            for (int i = 0; i < listlabel.Count; i++)
            {

                if (i == index)
                {
                    listlabel[i].Foreground = Brushes.Green;
                    //listlabel[i].Background = Brushes.Transparent;
                    //listlabel[i].BorderBrush = Brushes.Transparent;

                    index = i;
                }
                else
                    listlabel[i].Foreground = Brushes.Black;


            }
            return index;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            curindex = 0;
        }

        
        private void Navigation_MouseClick(object sender, RoutedEventArgs e)
        {
            if (Curpage == null)
            {
                curindex = HighLight_Navigation((Button)sender);
                NavigateFrame(curindex);


            }
            else if (Curpage.Save())
            {
                curindex = HighLight_Navigation((Button)sender);
                //curindex = HighLight_Navigation(curindex + 1);
                NavigateFrame(curindex);
            }
            
            //curindex = HighLight_Navigation((Button)sender);


            //NavigateFrame(curindex);
        }

        public void NavigateFrame(int index)
        {
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            btnSkip.IsEnabled = true;
            btnFinish.IsEnabled = true;

            switch (curindex)
            {
                case 0:
                    navigate<frmWizEvent>();
                    btnPrevious.IsEnabled = false;
                    break;
                case 1:
                    navigate<frmWizProgramme>();
                    break;
                case 2:
                    navigate<frmWizItem>();
                    break;
                case 3:
                    navigate<frmWizTask>();
                    break;
                case 4:
                    navigate<frmWizGuest>();
                    break;
                case 5:
                    navigate<frmWizPublish>();
                    btnSkip.IsEnabled = false;
                    btnNext.IsEnabled = false;
                    break;

            }
        }

        

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private bool navigate<T>()
        {
            Type newPageType = typeof(T);

            Curpage = (GemsWizPage)Activator.CreateInstance(newPageType, this);
            frame.Navigate(Curpage);
            //currPageType = typeof(T);
            //Events ev = (Events)cboEventList.SelectedItem;
            //if (pageFunctions[currPageType].Item2.Length > 0 && user.UserID != ev.Organizerid && !user.isSystemAdmin)
            //{
            //    try
            //    {
            //        RoleHelper client = new RoleHelper();
            //        foreach (EnumFunctions ef1 in client.GetRights(ev.EventID, user.UserID).ToArray<EnumFunctions>())
            //            foreach (EnumFunctions ef2 in pageFunctions[currPageType].Item2)
            //                if (ef1 == ef2)
            //                {
                                
            //                    return true;
            //                }
            //        client.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    currPageType = typeof(frmOverview);
            //}
            //currPage = (GEMSPage)Activator.CreateInstance(currPageType, user, ev);
            //frame.Navigate(currPage);
            return true;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Curpage.Save())
            {
                curindex = HighLight_Navigation(curindex + 1);
                NavigateFrame(curindex);
            }
        }

        

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Curpage.Save())
            {
                curindex = HighLight_Navigation(curindex - 1);
                NavigateFrame(curindex);
            }
            
            //NavigateFrame(curindex);
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            curindex = HighLight_Navigation(curindex + 1);
            NavigateFrame(curindex);
        }

        private T[][] ToArray<T>(List<List<T>> list)
        {
            List<T[]> temp = new List<T[]>();
            foreach(List<T> pro in list)
            {
                temp.Add(pro.ToArray());
            }
            return temp.ToArray();
        }
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (!Curpage.Save())
                return;
            System.Threading.Thread thread = new System.Threading.Thread(
                new System.Threading.ThreadStart(
                delegate()
                {
                    System.Windows.Threading.DispatcherOperation
                    dispatcherOp = this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(delegate()
                    {
                        try
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            MessageBox.Show("Please wait while we process your request...");

                            WizardClient client = new WizardClient();


                            client.WizardAddEvent(_user, _event, ToArray<Program>(_programs), ToArray<Guest>(_guests), _itemTypes.ToArray(), _items.ToArray(), _publish, _task.ToArray());
                            client.Close();

                            MessageBox.Show("Operation Completed",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            //this.Close();
                        }
                    }
                ));

                    dispatcherOp.Completed += new EventHandler(Completed);
                }
            ));

            thread.Start();

            
        }

        void Completed(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;

            
        }
      
    }
}
