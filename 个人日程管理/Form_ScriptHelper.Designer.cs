namespace 个人日程管理
{
    partial class Form_ScriptHelper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ScriptHelper));
            this.radioButton_Once = new System.Windows.Forms.RadioButton();
            this.radioButton_MultiTime = new System.Windows.Forms.RadioButton();
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.checkedListBox_Day = new System.Windows.Forms.CheckedListBox();
            this.button_Generate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButton_Once
            // 
            this.radioButton_Once.AutoSize = true;
            this.radioButton_Once.Location = new System.Drawing.Point(34, 21);
            this.radioButton_Once.Name = "radioButton_Once";
            this.radioButton_Once.Size = new System.Drawing.Size(47, 16);
            this.radioButton_Once.TabIndex = 0;
            this.radioButton_Once.TabStop = true;
            this.radioButton_Once.Text = "一次";
            this.radioButton_Once.UseVisualStyleBackColor = true;
            this.radioButton_Once.CheckedChanged += new System.EventHandler(this.radioButton_Once_CheckedChanged);
            // 
            // radioButton_MultiTime
            // 
            this.radioButton_MultiTime.AutoSize = true;
            this.radioButton_MultiTime.Location = new System.Drawing.Point(87, 21);
            this.radioButton_MultiTime.Name = "radioButton_MultiTime";
            this.radioButton_MultiTime.Size = new System.Drawing.Size(47, 16);
            this.radioButton_MultiTime.TabIndex = 1;
            this.radioButton_MultiTime.TabStop = true;
            this.radioButton_MultiTime.Text = "多次";
            this.radioButton_MultiTime.UseVisualStyleBackColor = true;
            this.radioButton_MultiTime.CheckedChanged += new System.EventHandler(this.radioButton_MultiTime_CheckedChanged);
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(27, 47);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker_StartDate.TabIndex = 2;
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(27, 74);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker_EndDate.TabIndex = 3;
            // 
            // checkedListBox_Day
            // 
            this.checkedListBox_Day.CheckOnClick = true;
            this.checkedListBox_Day.FormattingEnabled = true;
            this.checkedListBox_Day.Items.AddRange(new object[] {
            "周日",
            "周一",
            "周二",
            "周三",
            "周四",
            "周五",
            "周六"});
            this.checkedListBox_Day.Location = new System.Drawing.Point(27, 112);
            this.checkedListBox_Day.Name = "checkedListBox_Day";
            this.checkedListBox_Day.Size = new System.Drawing.Size(120, 116);
            this.checkedListBox_Day.TabIndex = 4;
            // 
            // button_Generate
            // 
            this.button_Generate.Location = new System.Drawing.Point(47, 244);
            this.button_Generate.Name = "button_Generate";
            this.button_Generate.Size = new System.Drawing.Size(75, 23);
            this.button_Generate.TabIndex = 5;
            this.button_Generate.Text = "生成";
            this.button_Generate.UseVisualStyleBackColor = true;
            this.button_Generate.Click += new System.EventHandler(this.button_Generate_Click);
            // 
            // Form_ScriptHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 279);
            this.Controls.Add(this.button_Generate);
            this.Controls.Add(this.checkedListBox_Day);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Controls.Add(this.radioButton_MultiTime);
            this.Controls.Add(this.radioButton_Once);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ScriptHelper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "脚本助手";
            this.Load += new System.EventHandler(this.Form_ScriptHelper_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_Once;
        private System.Windows.Forms.RadioButton radioButton_MultiTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.CheckedListBox checkedListBox_Day;
        private System.Windows.Forms.Button button_Generate;
    }
}