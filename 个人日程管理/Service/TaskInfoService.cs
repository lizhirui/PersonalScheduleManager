using LiZhiruiToolSet;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 个人日程管理.Dao;

namespace 个人日程管理.Service
{
    class TaskInfoService : BaseService
    {
        private TaskInfoDao dao;

        public TaskInfoService(DbUtility db = null)
        {
            dao = new TaskInfoDao(db);
        }

        public void Add(Model.Task info)
        {
            db.BeginTransaction();
            dao.Add(info);
            new RecordInfoService().Add(info,RecordInfoService.RecordType.Insert);
            db.CommitOrRollBackTransaction();
        }

        public void Modify(Model.Task info)
        {
            db.BeginTransaction();
            dao.Modify(info);
            new RecordInfoService().Add(info,RecordInfoService.RecordType.Update);
            db.CommitOrRollBackTransaction();
        }

        public void Delete(int id)
        {
            db.BeginTransaction();
            var info = dao.GetInfo(id);
            dao.Delete(id);
            new RecordInfoService().Add(info,RecordInfoService.RecordType.Delete);
            db.CommitOrRollBackTransaction();
        }

        public void Delete(IEnumerable<int> idList)
        {
            db.BeginTransaction();

            foreach(var id in idList)
            {
                var info = dao.GetInfo(id);
                dao.Delete(id);
                new RecordInfoService().Add(info,RecordInfoService.RecordType.Delete);
            }
            
            db.CommitOrRollBackTransaction();
        }

        public Model.Task GetInfo(int id)
        {
            var hasChild = false;
            return GetInfo(id,out hasChild);
        }

        private void FillTaskAdvancedFields(Model.Task task)
        {
            var eventDao = new EventInfoDao(dao.usingDbUtility);
            var eventList = eventDao.GetLinkedEventList(task.id);

            if(eventList.Length == 0)
            {
                task._hasLinkedEvent = false;
                task._firstStartTime = DateTime.MaxValue;
                task._lastEndTime = DateTime.MinValue;
            }
            else
            {
                task._hasLinkedEvent = true;
                task._firstStartTime = DateTime.MaxValue;
                task._lastEndTime = DateTime.MinValue;

                foreach(var item in eventList)
                {
                    var fm = new FormulaManager(item.remindFormula);
                    var startTime = fm.GetStartDate();
                    startTime = new DateTime(startTime.Year,startTime.Month,startTime.Day,item.startTime.Hour,item.startTime.Minute,item.startTime.Second);
                    var endTime = fm.GetEndDate();
                    endTime = new DateTime(endTime.Year,endTime.Month,endTime.Day,item.endTime.Hour,item.endTime.Minute,item.endTime.Second);
                    task._firstStartTime = startTime < task._firstStartTime ? startTime : task._firstStartTime;
                    task._lastEndTime = endTime > task._lastEndTime ? endTime : task._lastEndTime;
                }
            }
        }

        public Model.Task GetInfo(int id,out bool hasChild)
        {
            hasChild = false;
            var item = dao.GetInfo(id);
            item._hasChild = false;

            if(item == null)
            {
                return null;
            }

            FillTaskAdvancedFields(item);
            var tlist = GetAllChildList(item.id);

            if(tlist.Length > 0)
            {
                hasChild = true;
                item._hasChild = true;
                item.finishedProgress = 0;
                item.totalProgress = 0;

                foreach(var titem in tlist)
                {
                    item.finishedProgress += (int)(titem.finishedProgress * 10000.0d / titem.totalProgress);
                    item.totalProgress += 10000;

                    if(titem._hasLinkedEvent)
                    {
                        item._hasLinkedEvent = true;
                        item._firstStartTime = titem._firstStartTime < item._firstStartTime ? titem._firstStartTime : item._firstStartTime;
                        item._lastEndTime = titem._lastEndTime > item._lastEndTime ? titem._lastEndTime : item._lastEndTime;
                    }
                }
            }

            return item;
        }

        public Model.Task[] GetAllList()
        {
            return dao.GetAllList();
        }

        public Model.Task[] GetAllDirectChildList(int parentId)
        {
            //return dao.GetAllDirectChildList(parentId);
            return (from x in GetAllChildList(parentId) where x.parentId == parentId select x).ToArray();
        }

        public Model.Task[] GetAllChildList(int parentId)
        {
            var rlist = dao.GetAllDirectChildList(parentId).ToList();
            var rtlist = new List<Model.Task>();

            foreach(var item in rlist)
            {
                FillTaskAdvancedFields(item);
                item._hasChild = false;
                var tlist = GetAllChildList(item.id);

                if(tlist.Length > 0)
                {
                    item._hasChild = true;
                    rtlist.AddRange(tlist);
                    item.finishedProgress = 0;
                    item.totalProgress = 0;

                    foreach(var titem in tlist)
                    {
                        FillTaskAdvancedFields(titem);
                        item.finishedProgress += (int)(titem.finishedProgress * 10000.0d / titem.totalProgress);
                        item.totalProgress += 10000;

                        if(titem._hasLinkedEvent)
                        {
                            item._hasLinkedEvent = true;
                            item._firstStartTime = titem._firstStartTime < item._firstStartTime ? titem._firstStartTime : item._firstStartTime;
                            item._lastEndTime = titem._lastEndTime > item._lastEndTime ? titem._lastEndTime : item._lastEndTime;
                        }
                    }
                }
            }

            rlist.AddRange(rtlist);
            return rlist.ToArray();
        }
    }
}
