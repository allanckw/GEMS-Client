using System;
using System.Windows;
using System.Windows.Input;
using evmsService.entities;
using System.Windows.Threading;

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

        void dispatcherOp_Completed(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        private void btnEventExport_Click(object sender, RoutedEventArgs e)
        {


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
                            MessageBox.Show("Please wait while we process Export your data...");

                            ExportClient c = new ExportClient();
                            try
                            {

                                c.Open();
                                ExportData data = c.GetExportData(user, event_.EventID, cboFacilities.IsChecked.Value,
                                    cboProgrammes.IsChecked.Value, cboIncome.IsChecked.Value, cboOptItem.IsChecked.Value, cboTask.IsChecked.Value, cboGuest.IsChecked.Value, cboParticipant.IsChecked.Value);

                                Exporter.PrintExcel(data, cboFacilities.IsChecked.Value,
                                    cboProgrammes.IsChecked.Value, cboIncome.IsChecked.Value, cboOptItem.IsChecked.Value, cboTask.IsChecked.Value, cboGuest.IsChecked.Value, cboParticipant.IsChecked.Value);


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                //this.Close();
                            }
                            c.Close();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error have occured: " + ex.Message, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                ));

                    dispatcherOp.Completed += new EventHandler(dispatcherOp_Completed);
                }
            ));

            thread.Start();



            //try{
            //    ExportClient c = new ExportClient();
            //    c.Open();
            //    ExportData data = c.GetExportData(user, event_.EventID, cboFacilities.IsChecked.Value,
            //        cboProgrammes.IsChecked.Value, cboIncome.IsChecked.Value, cboOptItem.IsChecked.Value, cboTask.IsChecked.Value, cboGuest.IsChecked.Value, cboParticipant.IsChecked.Value);

            //    Exporter.PrintExcel(data, cboFacilities.IsChecked.Value,
            //        cboProgrammes.IsChecked.Value, cboIncome.IsChecked.Value, cboOptItem.IsChecked.Value, cboTask.IsChecked.Value, cboGuest.IsChecked.Value, cboParticipant.IsChecked.Value);

            //    c.Close();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    //this.Close();
            //}
        }
    }
}

