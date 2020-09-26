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

        public bool _hasChild;
        public bool _hasLinkedEvent;
        public DateTime _firstStartTime;
        public DateTime _lastEndTime;
        public bool _hasSameUnit;
    }
}
