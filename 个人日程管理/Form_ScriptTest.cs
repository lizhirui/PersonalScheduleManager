using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 个人日程管理
{
    public partial class Form_ScriptTest : Form
    {
        private string code;
        private FormulaManager fm = null;

        public Form_ScriptTest(string code)
        {
            this.code = code;
            fm = new FormulaManager(code);
            InitializeComponent();
        }

        private void Form_ScriptTest_Load(object sender,EventArgs e)
        {

        }

        private void button_Test_Click(object sender,EventArgs e)
        {
            try
            {
                if(dateTimePicker_Date.Value < fm.GetStartDate())
                {
                    Global.Error("该日期早于开始日期！");
                }
                else if(dateTimePicker_Date.Value > fm.GetEndDate())
                {
                    Global.Error("该日期晚于结束日期！");
                }
                else if(fm.CheckDate(dateTimePicker_Date.Value))
                {
                    Global.Info("该日期通过检查！");
                }
                else
                {
                    Global.Error("该日期未通过检查！");
                }
            }
            catch
            {
            
            }
        }
    }
}
