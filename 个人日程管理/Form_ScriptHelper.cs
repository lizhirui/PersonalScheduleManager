using ICSharpCode.AvalonEdit;
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
    public partial class Form_ScriptHelper : Form
    {
        private TextEditor formulaEditor;

        public Form_ScriptHelper(TextEditor formulaEditor)
        {
            this.formulaEditor = formulaEditor;
            InitializeComponent();
            radioButton_Once.Checked = false;
            radioButton_MultiTime.Checked = true;
        }

        private void Form_ScriptHelper_Load(object sender,EventArgs e)
        {

        }

        private void radioButton_Once_CheckedChanged(object sender,EventArgs e)
        {
            if(radioButton_Once.Checked)
            {
                dateTimePicker_EndDate.Enabled = false;
                checkedListBox_Day.Enabled = false;
            }
        }

        private void radioButton_MultiTime_CheckedChanged(object sender,EventArgs e)
        {
            if(radioButton_MultiTime.Checked)
            {
                dateTimePicker_EndDate.Enabled = true;
                checkedListBox_Day.Enabled = true;
            }
        }

        private void button_Generate_Click(object sender,EventArgs e)
        {
            var fourBlank = "    ";
            var stb = new StringBuilder();

            stb.AppendLine("function checkDate(curDate)");
            stb.AppendLine("{");

            if(radioButton_Once.Checked)
            {
                stb.Append(fourBlank);
                stb.AppendLine("return true;");
            }
            else
            {
                if(checkedListBox_Day.CheckedIndices.Count == 0)
                {
                    stb.Append(fourBlank);
                    stb.AppendLine("return false;");
                }
                else if(checkedListBox_Day.CheckedIndices.Count == 7)
                {
                    stb.Append(fourBlank);
                    stb.AppendLine("return true;");
                }
                else if(checkedListBox_Day.CheckedIndices.Count <= 3)
                {
                    stb.Append(fourBlank);
                    stb.AppendLine("curDate = getDate(curDate);");
                    var sep = "";
                    stb.Append(fourBlank);
                    stb.Append("return ");

                    for(var i = 0;i < checkedListBox_Day.CheckedIndices.Count;i++)
                    {
                        stb.Append(sep);
                        stb.Append("curDate.getDay() == " + checkedListBox_Day.CheckedIndices[i]);
                        sep = " || ";
                    }

                    stb.AppendLine(";");
                }
                else
                {
                    var set = new HashSet<int>();
                    
                    for(var i = 0;i < checkedListBox_Day.CheckedIndices.Count;i++)
                    {
                        set.Add(checkedListBox_Day.CheckedIndices[i]);
                    }

                    stb.Append(fourBlank);
                    stb.AppendLine("curDate = getDate(curDate);");
                    var sep = "";
                    stb.Append(fourBlank);
                    stb.Append("return ");

                    for(var i = 0;i < 7;i++)
                    {
                        if(!set.Contains(i))
                        {
                            stb.Append(sep);
                            stb.Append("curDate.getDay() != " + i);
                            sep = " && ";
                        }
                    }

                    stb.AppendLine(";");
                }
            }

            stb.AppendLine("}");
            stb.AppendLine();
            stb.AppendLine("function getStartDate(curDate)");
            stb.AppendLine("{");
            stb.Append(fourBlank);
            stb.AppendLine("return returnDate(new Date(" + dateTimePicker_StartDate.Value.Year + "," + (dateTimePicker_StartDate.Value.Month - 1) + "," + dateTimePicker_StartDate.Value.Day + "));");
            stb.AppendLine("}");
            stb.AppendLine();
            stb.AppendLine("function getEndDate(curDate)");
            stb.AppendLine("{");
            stb.Append(fourBlank);
            
            if(radioButton_Once.Checked)
            {
                stb.AppendLine("return returnDate(new Date(" + dateTimePicker_StartDate.Value.Year + "," + (dateTimePicker_StartDate.Value.Month - 1) + "," + dateTimePicker_StartDate.Value.Day + "));");
            }
            else
            {
                stb.AppendLine("return returnDate(new Date(" + dateTimePicker_EndDate.Value.Year + "," + (dateTimePicker_EndDate.Value.Month - 1) + "," + dateTimePicker_EndDate.Value.Day + "));");
            }
            
            stb.Append("}");
            formulaEditor.Text = stb.ToString();
            Close();
        }
    }
}
