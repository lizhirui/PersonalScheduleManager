using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using 个人日程管理.Model;
using LiZhiruiToolSet;

namespace 个人日程管理.Dao
{
    class EventInfoDao : BaseDao
    {
        public EventInfoDao(DbUtility db = null) : base("eventinfo",db)
        {
        }

        public void Add(Model.Event info)
        {
            db.Insert(tableName,info);
        }

        public void Modify(Model.Event info)
        {
            db.Update(tableName,info,"Id",info.id);
        }

        public void Delete(int id)
        {
            db.Delete(tableName,"Id",id);
        }

        public void Delete(IEnumerable<int> idList)
        {
            db.BeginTransaction();

            foreach(var id in idList)
            {
                db.Delete(tableName,"Id",id);
            }

            db.CommitOrRollBackTransaction();
        }

        public Model.Event GetInfo(int id)
        {
            var r = db.Select<Model.Event>(tableName,"Id",id);
            return r.Count > 0 ? r[0] : null;
        }

        public Model.Event[] GetAllList(DateTime startTime,DateTime endTime)
        {
            var plist = new List<DbParameter>();

            plist.Add(db.CreateDbParameter("startTime",startTime));
            plist.Add(db.CreateDbParameter("endTime",endTime));
            return db.Select<Model.Event>(tableName,"CreatedTime between @startTime and @endTime order by CreatedTime desc",plist).ToArray();
        }

        public Model.Event[] GetLinkedEventList(int taskId)
        {
            var plist = new List<DbParameter>();

            plist.Add(db.CreateDbParameter("taskId",taskId));
            plist.Add(db.CreateDbParameter("type",Event.Type.Task));
            return db.Select<Model.Event>(tableName,"taskId = @taskId and type = @type and enabled = true",plist).ToArray();
        }
    }
}
