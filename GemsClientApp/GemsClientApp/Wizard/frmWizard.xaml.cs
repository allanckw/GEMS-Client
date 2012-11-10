using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;


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

        private AxAgentObjects.AxAgent axAgent1;
        private AgentObjects.IAgentCtlCharacter speaker;


        public frmWizard(User u)
        {
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
            listlabel.Add(btnevent);
            listlabel.Add(btnprogramme);
            listlabel.Add(btnitem);
            listlabel.Add(btntask);
            listlabel.Add(btnguest);
            listlabel.Add(btnpublish);
            listlabel.Add(btnsummary);

            Navigation_MouseClick(listlabel[0], null);

        }

        public int HighLight_Navigation(Button lbl)
        {
            int index = 0;
            for (int i = 0; i < listlabel.Count; i++)
            {

                if (listlabel[i] == lbl)
                {
                    listlabel[i].Foreground = Brushes.GreenYellow;
                    listlabel[i].Focusable = false;
                    listlabel[i].IsEnabled = false;
                    //listlabel[i].Background = Brushes.Transparent;
                    //listlabel[i].BorderBrush = Brushes.Transparent;

                    index = i;
                }
                else
                {
                    listlabel[i].Foreground = Brushes.White;
                    listlabel[i].Focusable = true;
                    listlabel[i].IsEnabled = true;
                }

            }
            return index;
        }

        public int HighLight_Navigation(int index)
        {

            for (int i = 0; i < listlabel.Count; i++)
            {

                if (i == index)
                {
                    listlabel[i].Foreground = Brushes.GreenYellow;
                    listlabel[i].Focusable = false;
                    listlabel[i].IsEnabled = false;
                    //listlabel[i].Background = Brushes.Transparent;
                    //listlabel[i].BorderBrush = Brushes.Transparent;

                    index = i;
                }
                else
                {
                    listlabel[i].Foreground = Brushes.White;
                    listlabel[i].Focusable = true;
                    listlabel[i].IsEnabled = true;
                }


            }
            return index;
        }

        public void WizHelpTalk(string talk)
        {
            try
            {

                this.speaker.Speak(talk, null);
            }
            catch { }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            curindex = 0;


            axAgent1 = null;
            try
            {
                System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
                // Create an object of your User control.
                axAgent1 = new AxAgentObjects.AxAgent();
                // Assign MyWebcam control as the host control's child.
                host.Child = axAgent1;
                // Add the interop host control to the Grid
                // control's collection of child controls.
                this.gridwiz.Children.Add(host);

                this.axAgent1.Characters.Load("merlin", "merlin.acs");    //load the character  in the axAgent1 object  -- axAgent1 can load more than one character
                //this.speaker.SoundEffectsOn = false;
                //speaker.SoundEffectsOn = false;
                this.speaker = this.axAgent1.Characters["merlin"];     //give the speaker object the character to show it
                this.speaker.Show(0);



                cbwiz.IsChecked = true;
                cbwizsound.IsChecked = true;

                WizHelpTalk("Welcome to the GEMS Wizard. Please enter the information for your new event. " +
                    "After which, you can click \"Next\" or \"Skip\" button to proceed.");
            }
            //catch (FileNotFoundException)   //if the charater not found  // using IO 
            //{
            //    MessageBox.Show("Invalid charater location");
            //}
            catch
            {
                cbwiz.Visibility = Visibility.Collapsed;
                cbwizsound.Visibility = Visibility.Collapsed;
                //dont load wiz stuff
            }


        }

        private void Navigation_MouseClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (Curpage == null)
            {
                curindex = HighLight_Navigation(btn);
                NavigateFrame(curindex);
            }
            else if (Curpage.Save())
            {
                curindex = HighLight_Navigation(btn);
                //curindex = HighLight_Navigation(curindex + 1);
                NavigateFrame(curindex);
                UpdateSideEventInfo();
            }

        }

        public void NavigateFrame(int index)
        {
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            btnSkip.IsEnabled = true;
            btnFinish.IsEnabled = false;

            switch (curindex)
            {
                case 0:
                    navigate<frmWizEvent>();
                    btnPrevious.IsEnabled = false;
                    btnSkip.IsEnabled = false;
                    this.Title = "Quick Start Wizard - Step 1 of 7";

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("Enter your new event information.");
                    }

                    break;
                case 1:
                    navigate<frmWizProgramme>();
                    this.Title = "Quick Start Wizard - Step 2 of 7";

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("Enter your planned programmes details for the respective event days.");
                    }

                    break;
                case 2:
                    navigate<frmWizItem>();
                    this.Title = "Quick Start Wizard - Step 3 of 7";

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("Enter the items needed for your event.");
                    }

                    break;
                case 3:
                    navigate<frmWizTask>();
                    this.Title = "Quick Start Wizard - Step 4 of 7";

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("Enter the tasks to be completed for your event.");
                    }

                    break;
                case 4:
                    navigate<frmWizGuest>();
                    this.Title = "Quick Start Wizard - Step 5 of 7";

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("Enter the guests who are attending on the respective event days.");
                    }

                    break;
                case 5:
                    navigate<frmWizPublish>();
                    this.Title = "Quick Start Wizard - Step 6 of 7";

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("Enter the publish information for your event.");
                    }
                    //btnSkip.IsEnabled = false;
                    //btnNext.IsEnabled = false;
                    break;
                case 6:
                    navigate<frmWizSummary>();
                    this.Title = "Quick Start Wizard - Step 7 of 7";
                    btnSkip.IsEnabled = false;
                    btnNext.IsEnabled = false;
                    btnFinish.IsEnabled = true;

                    if (cbwizsound.IsChecked == true)
                    {
                        WizHelpTalk("This page summarises all the information that you have added so far. " +
                            "Do look through them before clicking the Finish button. If you have any changes to make, " +
                            "either click the \"Previous\" button or the left menu bar.");
                    }
                    break;
            }
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

            return true;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Curpage.Save())
            {
                curindex = HighLight_Navigation(curindex + 1);
                NavigateFrame(curindex);
                UpdateSideEventInfo();
            }
        }



        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Curpage.Save())
            {
                curindex = HighLight_Navigation(curindex - 1);
                NavigateFrame(curindex);
            }
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            curindex = HighLight_Navigation(curindex + 1);
            NavigateFrame(curindex);
            UpdateSideEventInfo();
        }

        private T[][] ToArray<T>(List<List<T>> list)
        {
            List<T[]> temp = new List<T[]>();
            foreach (List<T> pro in list)
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

                            WizardHelper client = new WizardHelper();


                            client.WizardAddEvent(_user, _event, ToArray<Program>(_programs), ToArray<Guest>(_guests), _itemTypes.ToArray(), _items.ToArray(), _publish, _task.ToArray());
                            client.Close();

                            MessageBox.Show("Operation completed!",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
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

        private void cbwiz_Checked(object sender, RoutedEventArgs e)
        {
            //this.axAgent1.Characters.Load("merlin", "merlin.acs");    //load the character  in the axAgent1 object  -- axAgent1 can load more than one character

            //    this.speaker = this.axAgent1.Characters["merlin"];     //give the speaker object the character to show it
            //    this.speaker.Show(0);

            cbwizsound.Visibility = Visibility.Visible;
            cbwiz.IsChecked = true;
            try
            {
                this.axAgent1.Characters.Load("merlin", "merlin.acs");    //load the character  in the axAgent1 object  -- axAgent1 can load more than one character

                this.speaker = this.axAgent1.Characters["merlin"];     //give the speaker object the character to show it
                this.speaker.Show(0);

                speaker.SoundEffectsOn = false;

                //this.speaker.Speak("ahhhhhhh", null);


            }
            catch { }


        }

        private void cbwizsound_Checked(object sender, RoutedEventArgs e)
        {


        }

        private void cbwiz_Unchecked(object sender, RoutedEventArgs e)
        {
            cbwizsound.Visibility = Visibility.Collapsed;
            try
            {
                axAgent1.Characters.Unload("Merlin");

                this.speaker.StopAll(0);
            }
            catch { }
        }

        private void cbwizsound_Unchecked(object sender, RoutedEventArgs e)
        {
            speaker.SoundEffectsOn = false;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                axAgent1.Characters.Unload("Merlin");

                this.speaker.StopAll(0);
            }
            catch { }
        }

        private void UpdateSideEventInfo()
        {
            if (curindex == 6)
                gbEvtDetails.Visibility = Visibility.Hidden;
            else
            {
                gbEvtDetails.Visibility = Visibility.Visible;
                lblEvtName.Content = "Event Name: \n" + _event.Name;
                lblEvtStartDate.Content = "Event Start Date: \n" + _event.StartDateTime.ToString();
                lblEvtEndDate.Content = "Event End Date: \n" + _event.EndDateTime.ToString();
            }
            //gbEvtDetails.Visibility = Visibility.Visible;
            //lblEvtName.Content = "Event Name: \n" + _event.Name;
            //lblEvtStartDate.Content = "Event Start Date: \n" + _event.StartDateTime.ToString();
            //lblEvtEndDate.Content = "Event End Date: \n" + _event.EndDateTime.ToString();
        }

    }
}
