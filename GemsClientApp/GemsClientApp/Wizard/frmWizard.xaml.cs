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
        public frmWizard()
        {

            
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
                
                
                
               
            //Navigation_MouseClick(listlabel[0], null);
           // navigate<frmWizEvent>();
            //listlabel.Add(
            //InitializeComponent();
        }

        private int HighLight_Navigation(Button lbl)
        {
            int index=0;
            for (int i = 0; i < listlabel.Count; i++)
            {

                if (listlabel[i] == lbl)
                {
                    //listlabel[i].Foreground = Brushes.Green;
                    index = i;
                }
                //else
                   // listlabel[i].Foreground = Brushes.Black;

                
            }
            return index;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            curindex = 0;
        }

        
        private void Navigation_MouseClick(object sender, RoutedEventArgs e)
        {
            curindex = HighLight_Navigation((Button)sender);

            switch (curindex)
            {
                case 0:
                    navigate<frmWizEvent>();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
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
            
                
            frame.Navigate(Activator.CreateInstance(newPageType));
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

      
    }
}
