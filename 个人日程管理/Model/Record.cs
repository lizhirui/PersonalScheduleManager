using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Model
{
    class Record : BaseModel
    {
        public int id;
        public DateTime time;
        public string Description;
    }
}
