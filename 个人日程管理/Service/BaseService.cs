using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Service
{
    class BaseService
    {
        protected DbUtility db = DbUtility.GetInstance();
    }
}
