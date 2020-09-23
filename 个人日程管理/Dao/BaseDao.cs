using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Dao
{
    abstract class BaseDao
    {
        protected DbUtility db = DbUtility.GetInstance();
        protected string _tableName = "";

        public string tableName
        {
            get
            {
                return _tableName;
            }
        }

        public DbUtility usingDbUtility
        {
            get
            {
                return db;
            }
        }

        public BaseDao(string _tableName,DbUtility db = null)
        {
            this._tableName = _tableName;

            if(db != null)
            {
                this.db = db;
            }
        }
    }
}
