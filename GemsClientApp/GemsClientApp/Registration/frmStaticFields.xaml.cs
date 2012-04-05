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
    /// Interaction logic for frmStaticFields.xaml
    /// </summary>
    public partial class frmStaticFields : GEMSPage
    {
        User user;
        Event event_;

        public frmStaticFields()
        {
            InitializeComponent();
        }

        public frmStaticFields(User u, Event e)
            : this()
        {
            this.user = u;
            this.event_ = e;

            try
            {
                WCFHelperClient client = new WCFHelperClient();
                List<StaticField> staticFields = client.ViewStaticField().ToList();
                client.Close();
                dgStaticFields.ItemsSource = staticFields;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
