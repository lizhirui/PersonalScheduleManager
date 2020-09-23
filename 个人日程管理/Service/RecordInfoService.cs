using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 个人日程管理.Dao;

namespace 个人日程管理.Service
{
    class RecordInfoService : BaseService
    {
        private RecordInfoDao dao;

        public RecordInfoService(DbUtility db = null)
        {
            dao = new RecordInfoDao(db);
        }

        public enum RecordType
        {
            Insert,
            Update,
            Delete
        }

        public void Add(Model.Task task,RecordType recordType)
        {
            var record = new Model.Record();
            record.time = DateTime.Now;

            switch(recordType)
            {
                case RecordType.Insert:
                    record.Description = "任务" + task.title +"被添加，最新进度：" + task.finishedProgress + "/" + task.totalProgress + task.progressUnit + "(" + Math.Round(task.finishedProgress * 100.0d / task.totalProgress,2) + "%)";
                    break;

                case RecordType.Update:
                    record.Description = "任务" + task.title +"被修改，最新进度：" + task.finishedProgress + "/" + task.totalProgress + task.progressUnit + "(" + Math.Round(task.finishedProgress * 100.0d / task.totalProgress,2) + "%)";
                    break;

                case RecordType.Delete:
                    record.Description = "任务" + task.title + "被删除";
                    break;
            }

            dao.Add(record);
        }

        public Model.Record[] GetList(DateTime startTime,DateTime endTime)
        {
            return dao.GetList(startTime,endTime);
        }
    }
}
