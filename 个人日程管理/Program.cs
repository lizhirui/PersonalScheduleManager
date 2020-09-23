using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 个人日程管理
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var db = DbUtility.GetInstance();
                db.Init(Global.connectionString,DbProviderType.MySql);
                db.Open();
            }
            catch
            {
                Global.Error("数据库连接失败！");
                Environment.Exit(-1);
            }

            Application.Run(new Form_Main());
        }
    }
}
