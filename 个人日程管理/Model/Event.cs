using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Model
{
    class Event : BaseModel
    {

        public enum Type
        {
            Task,
            GenericEvent
        }

        public int id;
        public DateTime createdTime;
        public DateTime startTime;
        public DateTime endTime;
        public string remindFormula;
        public Type type;
        public int taskId;
        public string title;
        public string description;
        public bool enabled;

        public override string ToString()
        {
            return title + " - " + createdTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
