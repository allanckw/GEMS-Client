using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using evmsService.entities;

namespace Gems.UIWPF
{
    class TaskAssignmentState
    {
        TaskAssignment taskAssignment;

        public TaskAssignment TaskAssignment
        {
            get { return taskAssignment; }
            set { taskAssignment = value; }
        }

        String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        String position;

        public String Position
        {
            get { return position; }
            set { position = value; }
        }

        public TaskAssignmentState(TaskAssignment tAssn, String name)
        {
            this.taskAssignment = tAssn;

            string temp = name.Replace(Environment.NewLine, "\\");
            string[] temp2 = temp.Split('\\');
            //Name
            this.name = temp2[0];
            //Position
            string[] temp3 = temp.Split(':');
            this.position = temp3[1].Trim();
        }

    }
}
