using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 个人日程管理.Dao;

namespace 个人日程管理.Service
{
    class EventInfoService : BaseService
    {
        private EventInfoDao dao;

        public EventInfoService(DbUtility db = null)
        {
            dao = new EventInfoDao(db);
        }

        public void Add(Model.Event info)
        {
            dao.Add(info);    
        }

        public void Modify(Model.Event info)
        {
            dao.Modify(info);
        }

        public void Delete(int id)
        {
            dao.Delete(id);
        }

        public Model.Event GetInfo(int id)
        {
            return dao.GetInfo(id);
        }
        
        public Model.Event[] GetAllList(DateTime startTime,DateTime endTime)
        {
            return dao.GetAllList(startTime,endTime);
        }
    }
}
