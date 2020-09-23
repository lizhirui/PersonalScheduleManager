using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Model
{
    class Memo : BaseModel
    {
        public int id;
        public DateTime createdTime;
        public string title;
        public string description;
        public int parentId;

        public override string ToString()
        {
            return title + " - " + createdTime.ToString("yyyy/MM/dd hh:mm:ss");
        }
    }
}
