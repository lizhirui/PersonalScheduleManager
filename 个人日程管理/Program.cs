using LiZhiruiToolSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            var isAppYetNotRunning = false;
            var mutex = new Mutex(true,"$Lizhirui$PersonalScheduleManager$000001$",out isAppYetNotRunning);

            if(!isAppYetNotRunning)
            {
                Global.Error("本程序只能打开单个实例！");
                Environment.Exit(1);
            }

            try
            {
                var db = DbUtility.GetInstance();
                db.Init(Global.connectionString,DbProviderType.MySql);
                db.Open();
                db.Close();
            }
            catch(Exception e)
            {
                Global.Error("数据库连接失败！\r\n" + e.Message + "\r\n" + e.StackTrace);
                Environment.Exit(-1);
            }
            
            Application.Run(new Form_Main());
        }
    }
}
