using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Records;

namespace 个人日程管理.Dao
{
    class RecordInfoDao : BaseDao
    {
        public RecordInfoDao(DbUtility db = null) : base("recordinfo",db)
        {
        }

        public void Add(Model.Record record)
        {
            db.Insert(tableName,record);
        }

        public Model.Record[] GetList(DateTime startTime,DateTime endTime)
        {
            var plist = new List<DbParameter>();

            plist.Add(db.CreateDbParameter("startTime",startTime));
            plist.Add(db.CreateDbParameter("endTime",endTime));
            return db.Select<Model.Record>(tableName,"time between @startTime and @endTime order by time desc",plist).ToArray();
        }
    }
}
