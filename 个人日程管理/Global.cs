using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 个人日程管理
{
    class Global
    {
        public static readonly string connectionString = "Server=localhost;Database=schedulemanager;Uid=schedulemanager;Pwd=schedulemanagerlzr";

        static Global()
        {
            var mcstb = new MySqlConnectionStringBuilder();
            mcstb.Keepalive = 60;
            mcstb.UserID = "schedulemanager";
            mcstb.Password = "schedulemanagerlzr";
            mcstb.Database = "schedulemanager";
            mcstb.Server = "localhost";
            connectionString = mcstb.ConnectionString;
        }

        public static void Error(string text)
        {
            MessageBox.Show(text,"错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        public static bool Confirm(string text)
        {
            return MessageBox.Show(text,"确认",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static void Info(string text)
        {
            MessageBox.Show(text,"提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public static string GetRightSubString(string str,int length)
        {
            return str.Substring(str.Length - length,length);
        }
    }
}
