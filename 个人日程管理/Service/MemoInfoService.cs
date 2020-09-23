using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 个人日程管理.Dao;

namespace 个人日程管理.Service
{
    class MemoInfoService : BaseService
    {
        private MemoInfoDao dao;

        public MemoInfoService(DbUtility db = null)
        {
            dao = new MemoInfoDao(db);
        }

        public void Add(Model.Memo memo)
        {
            dao.Add(memo);
        }

        public void Modify(Model.Memo memo)
        {
            dao.Modify(memo);
        }

        public void Delete(int id)
        {
            dao.Delete(id);
        }

        public void Delete(IEnumerable<int> idList)
        {
            db.BeginTransaction();

            foreach(var id in idList)
            {
                dao.Delete(id);
            }
            
            db.CommitOrRollBackTransaction();
        }

        public Model.Memo GetInfo(int id)
        {
            return dao.GetInfo(id);
        }

        public Model.Memo[] GetAllList()
        {
            return dao.GetAllList();
        }
    }
}
