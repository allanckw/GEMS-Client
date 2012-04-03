using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using evmsService.entities;

namespace Gems.UIWPF.CustomCtrl
{
    public class clsTaskAllocate
    {
       
        public static ObservableCollection<Task> GetTaskNotAssigned(List<Task> AllTask, List<Task> individualTask)
        {
            ObservableCollection<Task> _Collection = new ObservableCollection<Task>();
            ObservableCollection<Task> _Collection2Delete = new ObservableCollection<Task>();
            AllTask.ForEach(x => _Collection.Add(x));
            foreach (Task task in individualTask)
	        {
                foreach (Task task2Check in _Collection)
                {
                    if (task.TaskID==task2Check.TaskID)
                    {
                        _Collection2Delete.Add(task2Check);
                        break;
                    }
                }
		 
        	}
            
            foreach (Task task in _Collection2Delete)
            {
                _Collection.Remove(task);
            }

            return _Collection;
        }

        //public static ObservableCollection<Task> GetTaskAssigned(List<Task> individualTask)
        //{
        //    ObservableCollection<Task> _Collection = new ObservableCollection<Task>();

        //    individualTask.ForEach(x => _Collection.Add(x));

        //    return _Collection;
        //}

        public static ObservableCollection<Task> GetTaskAssigned(List<Task> individualTask, int roleID)
        {
            ObservableCollection<Task> _Collection = new ObservableCollection<Task>();

            foreach (Task t in individualTask)
            {
                List<TaskAssignment> taskAssns = t.TasksAssignments.ToList<TaskAssignment>();
                int completedCount = (from tAssn in taskAssns 
                                  where tAssn.IsCompleted==true 
                                  && tAssn.AssignedRoleID == roleID
                                  select tAssn).ToList<TaskAssignment>().Count;
                if (completedCount == 0)
                {
                    _Collection.Add(t);
                }
            }

            //individualTask.ForEach(x => _Collection.Add(x));
            return _Collection;
        }
    }
}
