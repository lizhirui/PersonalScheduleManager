using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Dao
{
    class MemoInfoDao : BaseDao
    {
        public MemoInfoDao(DbUtility db = null) : base("memoinfo",db)
        {
        }

        public void Add(Model.Memo info)
        {
            db.Insert(tableName,info);
        }

        public void Modify(Model.Memo info)
        {
            db.Update(tableName,info,"Id",info.id);
        }

        public void Delete(int id)
        {
            db.Delete(tableName,"Id",id);
        }

        public Model.Memo GetInfo(int id)
        {
            var r = db.Select<Model.Memo>(tableName,"Id",id);
            return r.Count > 0 ? r[0] : null;
        }

        public Model.Memo[] GetAllList()
        {
            return db.Select<Model.Memo>(tableName,"1 = 1 order by CreatedTime desc",new List<DbParameter>()).ToArray();
        }
    }
}
