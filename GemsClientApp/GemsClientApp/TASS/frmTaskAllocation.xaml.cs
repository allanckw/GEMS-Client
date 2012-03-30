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
using evmsService.entities;
using Gems.UIWPF.CustomCtrl;

namespace Gems.UIWPF
{
	public partial class frmTaskAllocation
	{
		private Window mainFrame;
        private User user;
        private Event event_;
		
		public frmTaskAllocation()
		{
			this.InitializeComponent();
            refreshTaskList();
			// Insert code required on object creation below this point.
		}
		

        public void refreshTaskList(){

            //Example of All Task created for the event
            //Throw in All Task Here
            List<clsTaskAllocate> AllTask = new List<clsTaskAllocate>(new clsTaskAllocate[] {
                    new clsTaskAllocate { Name="Task2", Description="Description 2", Balance=0},
                    new clsTaskAllocate { Name="Task3", Description="Description 3", Balance=0},
                 new clsTaskAllocate { Name="Task5", Description="Description 5", Balance=0},
                 new clsTaskAllocate { Name="Task6", Description="Description 6", Balance=0},
                 new clsTaskAllocate { Name="Task7", Description="Description 7", Balance=0},
                 new clsTaskAllocate { Name="Task9", Description="Description 9",Balance=0}});

            //Example of assigned Task to an individual
            //Get individual task list dependent on cboRole
            
            List<clsTaskAllocate> IndividualTask = new List<clsTaskAllocate>(new clsTaskAllocate[] {
                    new clsTaskAllocate { Name="Task1", Description="Description 1 dasdsasdasdasdasdasdasdasdasd", Balance=0 },
                     new clsTaskAllocate { Name="Task4", Description="Description 4", Balance=0},
                 new clsTaskAllocate { Name="Task8", Description="Description 8",Balance=0},
            });

            //<<Throw in List of Task to me>>
            //<<Throw in another list of Task Assigned to the person>>
            //This is only an example
            //List does not work with drag and drop
            //Need to think how we are going to save the changes or detect the changes
            //if not sure dunno how to do, just pass in that two list i will do it
            lstAllTask.ItemsSource = clsTaskAllocate.GetTaskNotAssigned(AllTask, IndividualTask);

            lstAssignedTask.ItemsSource = clsTaskAllocate.GetTaskAssigned(IndividualTask);
        }

		public frmTaskAllocation(User u, Event e, Window f)
            : this()
        {
            this.user = u;
            this.event_ = e;
            this.mainFrame = f;
        }

        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboRole.SelectedIndex != -1)
            {
            }
        }

        private void btnSaveTask_Click(object sender, RoutedEventArgs e)
        {

        }
	}
}