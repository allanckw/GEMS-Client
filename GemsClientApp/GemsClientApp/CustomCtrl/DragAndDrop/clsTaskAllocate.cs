using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Gems.UIWPF.CustomCtrl
{
    public class clsTaskAllocate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Balance { get; set; }

        public static ObservableCollection<clsTaskAllocate> GetTaskNotAssigned(List<clsTaskAllocate> AllTask, List<clsTaskAllocate> individualTask)
        {
            ObservableCollection<clsTaskAllocate> _Collection = new ObservableCollection<clsTaskAllocate>();
            AllTask.ForEach(x => _Collection.Add(x));
            individualTask.ForEach(x => _Collection.Remove(x));

            return _Collection;
        }

        public static ObservableCollection<clsTaskAllocate> GetTaskAssigned(List<clsTaskAllocate> individualTask)
        {
            ObservableCollection<clsTaskAllocate> _Collection = new ObservableCollection<clsTaskAllocate>();
            individualTask.ForEach(x => _Collection.Add(x));
            return _Collection;
        }
    }
}
