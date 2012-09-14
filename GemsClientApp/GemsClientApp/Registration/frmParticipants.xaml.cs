
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using evmsService.entities;
using evmsService.Controllers;

namespace Gems.UIWPF
{
    /// <summary>
    /// Interaction logic for frmParticipantList.xaml
    /// </summary>
    public partial class frmParticipants : GEMSPage
    {
        User user;
        Events event_;
        ParticipantWithName[] participants;

        public frmParticipants()
        {
            this.InitializeComponent();
        }

        public frmParticipants(User u, Events e)
            : this()
        {
            this.user = u;
            this.event_ = e;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadParticipants();
        }

        private void loadParticipants()
        {
            try
            {
               RegistrationHelper client = new RegistrationHelper();
              
                participants = client.ViewEventParticipantWithName(user, event_.EventID);
                lstParticipants.ItemsSource = participants;
                if (participants != null && participants.Count<ParticipantWithName>() > 0)
                    lstParticipants.SelectedIndex = 0;

                //ParticipantWithName a;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstParticipants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstParticipants.SelectedIndex == -1)
            {
                btnDelete.IsEnabled = false;
                dgParticipant.ItemsSource = null;
                return;
            }
            try
            {
                RegistrationHelper client = new RegistrationHelper();
                List<FieldAnswer> fieldAnswers = client.GetParticipantFieldAnswer(user, event_.EventID, (int)lstParticipants.SelectedValue).ToList();
                List<Field> fields = client.ViewField(event_.EventID).ToList();
                client.Close();
                List<Tuple<Field, FieldAnswer>> answers = new List<Tuple<Field, FieldAnswer>>();
                foreach (Field field in fields)
                {
                    FieldAnswer fieldAnswer = fieldAnswers.Find(
                        delegate(FieldAnswer fa)
                        {
                            return fa.FieldID == field.FieldID;
                        }
                    );
                    if (fieldAnswer != null)
                    {
                        field.FieldName += ":";
                        answers.Add(Tuple.Create(field, fieldAnswer));
                    }
                }

                Participant p = (Participant)lstParticipants.SelectedItem;


                dgParticipant.ItemsSource = answers;
                
               
                btnDelete.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstParticipants.SelectedIndex == -1)
                return;
            try
            {
                RegistrationHelper client = new RegistrationHelper();
                client.DeleteParticipant(user, event_.EventID, (int)lstParticipants.SelectedValue);
                client.Close();
                loadParticipants();
                MessageBox.Show("Operation succeeded!");
                lstParticipants_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
