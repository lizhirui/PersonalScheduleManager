using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Model
{
    class Task : BaseModel
    {
        public int id;
        public DateTime createdTime;
        public string title;
        public string description;
        public int parentId;
        public int finishedProgress;
        public int totalProgress;
        public string progressUnit;
        public bool hasChild;
        public bool hasLinkedEvent;
        public DateTime firstStartTime;
        public DateTime lastEndTime;
    }
}
