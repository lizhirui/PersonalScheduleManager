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
    public partial class Form_Info : Form
    {
        public Form_Info(string text,bool topMost,bool buttonLeftEnabled,string buttonLeftText,EventHandler buttonLeftClicked,bool buttonRightEnabled,string buttonRightText,EventHandler buttonRightClicked)
        {
            InitializeComponent();
            TopMost = topMost;
            Global.SetWindowPos(Handle,new IntPtr(-1),Left,Top,Width,Height,0);
            textBox_Info.Text = text;
            textBox_Info.SelectionStart = 0;
            textBox_Info.SelectionLength = 0;
            
            if(buttonLeftEnabled)
            {
                button_Left.Enabled = true;
                button_Left.Text = buttonLeftText;
                button_Left.Click += buttonLeftClicked;
            }
            else
            {
                button_Left.Enabled = false;
                button_Left.Text = "该按钮被禁用";
            }

            if(buttonRightEnabled)
            {
                button_Right.Enabled = true;
                button_Right.Text = buttonRightText;
                button_Right.Click += buttonRightClicked;
            }
            else
            {
                button_Right.Enabled = false;
                button_Right.Text = "该按钮被禁用";
            }
        }
    }
}
