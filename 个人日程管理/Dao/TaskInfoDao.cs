using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Dao
{
    class TaskInfoDao : BaseDao
    {
        public TaskInfoDao(DbUtility db = null) : base("taskinfo",db)
        {
        }

        public void Add(Model.Task info)
        {
            db.Insert(tableName,info);
        }

        public void Modify(Model.Task info)
        {
            db.Update(tableName,info,"Id",info.id);
        }

        public void Delete(int id)
        {
            db.Delete(tableName,"Id",id);
        }

        public Model.Task GetInfo(int id)
        {
            var r = db.Select<Model.Task>(tableName,"Id",id);
            return r.Count > 0 ? r[0] : null;
        }

        public Model.Task[] GetAllList()
        {
            return db.Select<Model.Task>(tableName).ToArray();
        }

        public Model.Task[] GetAllDirectChildList(int parentId)
        {
            return db.Select<Model.Task>(tableName,"parentId",parentId).ToArray();
        }
    }
}
