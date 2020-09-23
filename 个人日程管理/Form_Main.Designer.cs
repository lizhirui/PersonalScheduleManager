namespace 个人日程管理
{
    partial class Form_Main
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
            this.tabControl_Main = new System.Windows.Forms.TabControl();
            this.tabPage_Memo = new System.Windows.Forms.TabPage();
            this.button_Memo_Update = new System.Windows.Forms.Button();
            this.button_Memo_Add = new System.Windows.Forms.Button();
            this.textBox_Memo_Description = new System.Windows.Forms.TextBox();
            this.textBox_Memo_Title = new System.Windows.Forms.TextBox();
            this.listBox_Memo_List = new System.Windows.Forms.ListBox();
            this.tabPage_Task = new System.Windows.Forms.TabPage();
            this.panel_Task_Data = new System.Windows.Forms.Panel();
            this.button_Task_Update = new System.Windows.Forms.Button();
            this.button_Task_Add = new System.Windows.Forms.Button();
            this.textBox_Task_ProgressPercent = new System.Windows.Forms.TextBox();
            this.textBox_Task_Unit = new System.Windows.Forms.TextBox();
            this.textBox_Task_TotalProgress = new System.Windows.Forms.TextBox();
            this.label_Task_Separate = new System.Windows.Forms.Label();
            this.textBox_Task_FinishedProgress = new System.Windows.Forms.TextBox();
            this.label_Task_Progress = new System.Windows.Forms.Label();
            this.textBox_Task_Description = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Task_Title = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listView_Task_Item = new System.Windows.Forms.ListView();
            this.columnHeader_Task_List_Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Task_List_Progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Task_List_ProgressPercent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Task_FinishSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Task_RemainedTaskFinishMinimumSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_LastTaskFinishTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Task_List_CreatedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.treeView_Task_Dir = new System.Windows.Forms.TreeView();
            this.tabPage_Record = new System.Windows.Forms.TabPage();
            this.listView_Record_List = new System.Windows.Forms.ListView();
            this.columnHeader_Record_Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_Record_Search = new System.Windows.Forms.Button();
            this.dateTimePicker_Record_EndTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_Record_StartTime = new System.Windows.Forms.DateTimePicker();
            this.tabPage_Event = new System.Windows.Forms.TabPage();
            this.checkBox_Event_LinkTask = new System.Windows.Forms.CheckBox();
            this.button_Event_ScriptTest = new System.Windows.Forms.Button();
            this.checkBox_Event_Enable = new System.Windows.Forms.CheckBox();
            this.elementHost_Event_RemindFormula = new System.Windows.Forms.Integration.ElementHost();
            this.dateTimePicker_Event_EndTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_Event_StartTime = new System.Windows.Forms.DateTimePicker();
            this.button_Event_SearchAll = new System.Windows.Forms.Button();
            this.button_Event_Search = new System.Windows.Forms.Button();
            this.dateTimePicker_Event_EndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_Event_StartDate = new System.Windows.Forms.DateTimePicker();
            this.button_Event_Update = new System.Windows.Forms.Button();
            this.button_Event_Add = new System.Windows.Forms.Button();
            this.textBox_Event_Description = new System.Windows.Forms.TextBox();
            this.textBox_Event_Title = new System.Windows.Forms.TextBox();
            this.listBox_Event_List = new System.Windows.Forms.ListBox();
            this.tabPage_Schedule = new System.Windows.Forms.TabPage();
            this.button_Schedule_Search = new System.Windows.Forms.Button();
            this.dateTimePicker_Schedule_EndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_Schedule_StartDate = new System.Windows.Forms.DateTimePicker();
            this.checkBox_Schedule_Remind = new System.Windows.Forms.CheckBox();
            this.tabControl_Main.SuspendLayout();
            this.tabPage_Memo.SuspendLayout();
            this.tabPage_Task.SuspendLayout();
            this.panel_Task_Data.SuspendLayout();
            this.tabPage_Record.SuspendLayout();
            this.tabPage_Event.SuspendLayout();
            this.tabPage_Schedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage_Memo);
            this.tabControl_Main.Controls.Add(this.tabPage_Task);
            this.tabControl_Main.Controls.Add(this.tabPage_Record);
            this.tabControl_Main.Controls.Add(this.tabPage_Event);
            this.tabControl_Main.Controls.Add(this.tabPage_Schedule);
            this.tabControl_Main.Location = new System.Drawing.Point(2, 1);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(1260, 450);
            this.tabControl_Main.TabIndex = 0;
            // 
            // tabPage_Memo
            // 
            this.tabPage_Memo.AutoScroll = true;
            this.tabPage_Memo.Controls.Add(this.button_Memo_Update);
            this.tabPage_Memo.Controls.Add(this.button_Memo_Add);
            this.tabPage_Memo.Controls.Add(this.textBox_Memo_Description);
            this.tabPage_Memo.Controls.Add(this.textBox_Memo_Title);
            this.tabPage_Memo.Controls.Add(this.listBox_Memo_List);
            this.tabPage_Memo.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Memo.Name = "tabPage_Memo";
            this.tabPage_Memo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Memo.Size = new System.Drawing.Size(1252, 424);
            this.tabPage_Memo.TabIndex = 0;
            this.tabPage_Memo.Text = "便签";
            this.tabPage_Memo.UseVisualStyleBackColor = true;
            this.tabPage_Memo.SizeChanged += new System.EventHandler(this.tabPage_Memo_SizeChanged);
            // 
            // button_Memo_Update
            // 
            this.button_Memo_Update.Location = new System.Drawing.Point(582, 388);
            this.button_Memo_Update.Name = "button_Memo_Update";
            this.button_Memo_Update.Size = new System.Drawing.Size(75, 23);
            this.button_Memo_Update.TabIndex = 4;
            this.button_Memo_Update.Text = "更新";
            this.button_Memo_Update.UseVisualStyleBackColor = true;
            this.button_Memo_Update.Click += new System.EventHandler(this.button_Memo_Update_Click);
            // 
            // button_Memo_Add
            // 
            this.button_Memo_Add.Location = new System.Drawing.Point(439, 388);
            this.button_Memo_Add.Name = "button_Memo_Add";
            this.button_Memo_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Memo_Add.TabIndex = 3;
            this.button_Memo_Add.Text = "添加";
            this.button_Memo_Add.UseVisualStyleBackColor = true;
            this.button_Memo_Add.Click += new System.EventHandler(this.button_Memo_Add_Click);
            // 
            // textBox_Memo_Description
            // 
            this.textBox_Memo_Description.Location = new System.Drawing.Point(309, 32);
            this.textBox_Memo_Description.Multiline = true;
            this.textBox_Memo_Description.Name = "textBox_Memo_Description";
            this.textBox_Memo_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Memo_Description.Size = new System.Drawing.Size(478, 350);
            this.textBox_Memo_Description.TabIndex = 2;
            this.textBox_Memo_Description.TextChanged += new System.EventHandler(this.textBox_Memo_Description_TextChanged);
            // 
            // textBox_Memo_Title
            // 
            this.textBox_Memo_Title.Location = new System.Drawing.Point(309, 4);
            this.textBox_Memo_Title.Name = "textBox_Memo_Title";
            this.textBox_Memo_Title.Size = new System.Drawing.Size(478, 21);
            this.textBox_Memo_Title.TabIndex = 1;
            this.textBox_Memo_Title.TextChanged += new System.EventHandler(this.textBox_Memo_Title_TextChanged);
            // 
            // listBox_Memo_List
            // 
            this.listBox_Memo_List.FormattingEnabled = true;
            this.listBox_Memo_List.ItemHeight = 12;
            this.listBox_Memo_List.Location = new System.Drawing.Point(7, 4);
            this.listBox_Memo_List.Name = "listBox_Memo_List";
            this.listBox_Memo_List.Size = new System.Drawing.Size(295, 412);
            this.listBox_Memo_List.TabIndex = 0;
            this.listBox_Memo_List.Click += new System.EventHandler(this.listBox_Memo_List_Click);
            this.listBox_Memo_List.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox_Memo_List_KeyDown);
            // 
            // tabPage_Task
            // 
            this.tabPage_Task.Controls.Add(this.panel_Task_Data);
            this.tabPage_Task.Controls.Add(this.listView_Task_Item);
            this.tabPage_Task.Controls.Add(this.treeView_Task_Dir);
            this.tabPage_Task.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Task.Name = "tabPage_Task";
            this.tabPage_Task.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Task.Size = new System.Drawing.Size(1252, 424);
            this.tabPage_Task.TabIndex = 1;
            this.tabPage_Task.Text = "任务";
            this.tabPage_Task.UseVisualStyleBackColor = true;
            this.tabPage_Task.SizeChanged += new System.EventHandler(this.tabPage_Task_SizeChanged);
            // 
            // panel_Task_Data
            // 
            this.panel_Task_Data.Controls.Add(this.button_Task_Update);
            this.panel_Task_Data.Controls.Add(this.button_Task_Add);
            this.panel_Task_Data.Controls.Add(this.textBox_Task_ProgressPercent);
            this.panel_Task_Data.Controls.Add(this.textBox_Task_Unit);
            this.panel_Task_Data.Controls.Add(this.textBox_Task_TotalProgress);
            this.panel_Task_Data.Controls.Add(this.label_Task_Separate);
            this.panel_Task_Data.Controls.Add(this.textBox_Task_FinishedProgress);
            this.panel_Task_Data.Controls.Add(this.label_Task_Progress);
            this.panel_Task_Data.Controls.Add(this.textBox_Task_Description);
            this.panel_Task_Data.Controls.Add(this.label2);
            this.panel_Task_Data.Controls.Add(this.textBox_Task_Title);
            this.panel_Task_Data.Controls.Add(this.label1);
            this.panel_Task_Data.Location = new System.Drawing.Point(230, 94);
            this.panel_Task_Data.Name = "panel_Task_Data";
            this.panel_Task_Data.Size = new System.Drawing.Size(519, 390);
            this.panel_Task_Data.TabIndex = 2;
            this.panel_Task_Data.SizeChanged += new System.EventHandler(this.panel_Task_Data_SizeChanged);
            this.panel_Task_Data.VisibleChanged += new System.EventHandler(this.panel_Task_Data_VisibleChanged);
            // 
            // button_Task_Update
            // 
            this.button_Task_Update.Location = new System.Drawing.Point(294, 351);
            this.button_Task_Update.Name = "button_Task_Update";
            this.button_Task_Update.Size = new System.Drawing.Size(75, 23);
            this.button_Task_Update.TabIndex = 11;
            this.button_Task_Update.Text = "更新";
            this.button_Task_Update.UseVisualStyleBackColor = true;
            this.button_Task_Update.Click += new System.EventHandler(this.button_Task_Update_Click);
            // 
            // button_Task_Add
            // 
            this.button_Task_Add.Location = new System.Drawing.Point(155, 352);
            this.button_Task_Add.Name = "button_Task_Add";
            this.button_Task_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Task_Add.TabIndex = 10;
            this.button_Task_Add.Text = "添加";
            this.button_Task_Add.UseVisualStyleBackColor = true;
            this.button_Task_Add.Click += new System.EventHandler(this.button_Task_Add_Click);
            // 
            // textBox_Task_ProgressPercent
            // 
            this.textBox_Task_ProgressPercent.Location = new System.Drawing.Point(387, 307);
            this.textBox_Task_ProgressPercent.Name = "textBox_Task_ProgressPercent";
            this.textBox_Task_ProgressPercent.ReadOnly = true;
            this.textBox_Task_ProgressPercent.Size = new System.Drawing.Size(100, 21);
            this.textBox_Task_ProgressPercent.TabIndex = 9;
            // 
            // textBox_Task_Unit
            // 
            this.textBox_Task_Unit.Location = new System.Drawing.Point(280, 307);
            this.textBox_Task_Unit.Name = "textBox_Task_Unit";
            this.textBox_Task_Unit.Size = new System.Drawing.Size(100, 21);
            this.textBox_Task_Unit.TabIndex = 8;
            // 
            // textBox_Task_TotalProgress
            // 
            this.textBox_Task_TotalProgress.Location = new System.Drawing.Point(173, 307);
            this.textBox_Task_TotalProgress.Name = "textBox_Task_TotalProgress";
            this.textBox_Task_TotalProgress.Size = new System.Drawing.Size(100, 21);
            this.textBox_Task_TotalProgress.TabIndex = 7;
            this.textBox_Task_TotalProgress.TextChanged += new System.EventHandler(this.textBox_Task_TotalProgress_TextChanged);
            // 
            // label_Task_Separate
            // 
            this.label_Task_Separate.AutoSize = true;
            this.label_Task_Separate.Location = new System.Drawing.Point(155, 310);
            this.label_Task_Separate.Name = "label_Task_Separate";
            this.label_Task_Separate.Size = new System.Drawing.Size(11, 12);
            this.label_Task_Separate.TabIndex = 6;
            this.label_Task_Separate.Text = "/";
            // 
            // textBox_Task_FinishedProgress
            // 
            this.textBox_Task_FinishedProgress.Location = new System.Drawing.Point(48, 307);
            this.textBox_Task_FinishedProgress.Name = "textBox_Task_FinishedProgress";
            this.textBox_Task_FinishedProgress.Size = new System.Drawing.Size(100, 21);
            this.textBox_Task_FinishedProgress.TabIndex = 5;
            this.textBox_Task_FinishedProgress.TextChanged += new System.EventHandler(this.textBox_Task_FinishedProgress_TextChanged);
            // 
            // label_Task_Progress
            // 
            this.label_Task_Progress.AutoSize = true;
            this.label_Task_Progress.Location = new System.Drawing.Point(14, 310);
            this.label_Task_Progress.Name = "label_Task_Progress";
            this.label_Task_Progress.Size = new System.Drawing.Size(41, 12);
            this.label_Task_Progress.TabIndex = 4;
            this.label_Task_Progress.Text = "进度：";
            // 
            // textBox_Task_Description
            // 
            this.textBox_Task_Description.Location = new System.Drawing.Point(48, 38);
            this.textBox_Task_Description.Multiline = true;
            this.textBox_Task_Description.Name = "textBox_Task_Description";
            this.textBox_Task_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Task_Description.Size = new System.Drawing.Size(455, 260);
            this.textBox_Task_Description.TabIndex = 3;
            this.textBox_Task_Description.TextChanged += new System.EventHandler(this.textBox_Task_Description_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "描述：";
            // 
            // textBox_Task_Title
            // 
            this.textBox_Task_Title.Location = new System.Drawing.Point(48, 10);
            this.textBox_Task_Title.Name = "textBox_Task_Title";
            this.textBox_Task_Title.Size = new System.Drawing.Size(455, 21);
            this.textBox_Task_Title.TabIndex = 1;
            this.textBox_Task_Title.TextChanged += new System.EventHandler(this.textBox_Task_Title_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "标题：";
            // 
            // listView_Task_Item
            // 
            this.listView_Task_Item.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Task_List_Title,
            this.columnHeader_Task_List_Progress,
            this.columnHeader_Task_List_ProgressPercent,
            this.columnHeader_Task_FinishSpeed,
            this.columnHeader_Task_RemainedTaskFinishMinimumSpeed,
            this.columnHeader_LastTaskFinishTime,
            this.columnHeader_Task_List_CreatedTime});
            this.listView_Task_Item.FullRowSelect = true;
            this.listView_Task_Item.GridLines = true;
            this.listView_Task_Item.HideSelection = false;
            this.listView_Task_Item.Location = new System.Drawing.Point(230, 7);
            this.listView_Task_Item.Name = "listView_Task_Item";
            this.listView_Task_Item.Size = new System.Drawing.Size(1016, 408);
            this.listView_Task_Item.TabIndex = 1;
            this.listView_Task_Item.UseCompatibleStateImageBehavior = false;
            this.listView_Task_Item.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_Task_List_Title
            // 
            this.columnHeader_Task_List_Title.Text = "任务标题";
            this.columnHeader_Task_List_Title.Width = 200;
            // 
            // columnHeader_Task_List_Progress
            // 
            this.columnHeader_Task_List_Progress.Text = "进度";
            this.columnHeader_Task_List_Progress.Width = 100;
            // 
            // columnHeader_Task_List_ProgressPercent
            // 
            this.columnHeader_Task_List_ProgressPercent.Text = "进度百分比";
            this.columnHeader_Task_List_ProgressPercent.Width = 100;
            // 
            // columnHeader_Task_FinishSpeed
            // 
            this.columnHeader_Task_FinishSpeed.Text = "完成速度";
            this.columnHeader_Task_FinishSpeed.Width = 150;
            // 
            // columnHeader_Task_RemainedTaskFinishMinimumSpeed
            // 
            this.columnHeader_Task_RemainedTaskFinishMinimumSpeed.Text = "剩余任务完成最小速度";
            this.columnHeader_Task_RemainedTaskFinishMinimumSpeed.Width = 150;
            // 
            // columnHeader_LastTaskFinishTime
            // 
            this.columnHeader_LastTaskFinishTime.Text = "最晚任务完成时间";
            this.columnHeader_LastTaskFinishTime.Width = 150;
            // 
            // columnHeader_Task_List_CreatedTime
            // 
            this.columnHeader_Task_List_CreatedTime.Text = "创建时间";
            this.columnHeader_Task_List_CreatedTime.Width = 150;
            // 
            // treeView_Task_Dir
            // 
            this.treeView_Task_Dir.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView_Task_Dir.HideSelection = false;
            this.treeView_Task_Dir.Location = new System.Drawing.Point(7, 7);
            this.treeView_Task_Dir.Name = "treeView_Task_Dir";
            this.treeView_Task_Dir.Size = new System.Drawing.Size(216, 408);
            this.treeView_Task_Dir.TabIndex = 0;
            this.treeView_Task_Dir.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_Task_Dir_DrawNode);
            this.treeView_Task_Dir.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_Task_Dir_BeforeSelect);
            this.treeView_Task_Dir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Task_Dir_AfterSelect);
            this.treeView_Task_Dir.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_Task_Dir_NodeMouseClick);
            this.treeView_Task_Dir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_Task_Dir_KeyDown);
            // 
            // tabPage_Record
            // 
            this.tabPage_Record.Controls.Add(this.listView_Record_List);
            this.tabPage_Record.Controls.Add(this.button_Record_Search);
            this.tabPage_Record.Controls.Add(this.dateTimePicker_Record_EndTime);
            this.tabPage_Record.Controls.Add(this.dateTimePicker_Record_StartTime);
            this.tabPage_Record.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Record.Name = "tabPage_Record";
            this.tabPage_Record.Size = new System.Drawing.Size(1252, 424);
            this.tabPage_Record.TabIndex = 2;
            this.tabPage_Record.Text = "操作记录";
            this.tabPage_Record.UseVisualStyleBackColor = true;
            this.tabPage_Record.SizeChanged += new System.EventHandler(this.tabPage_Record_SizeChanged);
            // 
            // listView_Record_List
            // 
            this.listView_Record_List.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Record_Time,
            this.columnHeader_Description});
            this.listView_Record_List.FullRowSelect = true;
            this.listView_Record_List.GridLines = true;
            this.listView_Record_List.HideSelection = false;
            this.listView_Record_List.Location = new System.Drawing.Point(6, 31);
            this.listView_Record_List.Name = "listView_Record_List";
            this.listView_Record_List.Size = new System.Drawing.Size(1006, 347);
            this.listView_Record_List.TabIndex = 3;
            this.listView_Record_List.UseCompatibleStateImageBehavior = false;
            this.listView_Record_List.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_Record_Time
            // 
            this.columnHeader_Record_Time.Text = "时间";
            this.columnHeader_Record_Time.Width = 150;
            // 
            // columnHeader_Description
            // 
            this.columnHeader_Description.Text = "描述";
            this.columnHeader_Description.Width = 800;
            // 
            // button_Record_Search
            // 
            this.button_Record_Search.Location = new System.Drawing.Point(476, 5);
            this.button_Record_Search.Name = "button_Record_Search";
            this.button_Record_Search.Size = new System.Drawing.Size(75, 23);
            this.button_Record_Search.TabIndex = 2;
            this.button_Record_Search.Text = "查询";
            this.button_Record_Search.UseVisualStyleBackColor = true;
            this.button_Record_Search.Click += new System.EventHandler(this.button_Record_Search_Click);
            // 
            // dateTimePicker_Record_EndTime
            // 
            this.dateTimePicker_Record_EndTime.Location = new System.Drawing.Point(255, 6);
            this.dateTimePicker_Record_EndTime.Name = "dateTimePicker_Record_EndTime";
            this.dateTimePicker_Record_EndTime.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_Record_EndTime.TabIndex = 1;
            // 
            // dateTimePicker_Record_StartTime
            // 
            this.dateTimePicker_Record_StartTime.Location = new System.Drawing.Point(30, 6);
            this.dateTimePicker_Record_StartTime.Name = "dateTimePicker_Record_StartTime";
            this.dateTimePicker_Record_StartTime.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_Record_StartTime.TabIndex = 0;
            // 
            // tabPage_Event
            // 
            this.tabPage_Event.Controls.Add(this.checkBox_Event_LinkTask);
            this.tabPage_Event.Controls.Add(this.button_Event_ScriptTest);
            this.tabPage_Event.Controls.Add(this.checkBox_Event_Enable);
            this.tabPage_Event.Controls.Add(this.elementHost_Event_RemindFormula);
            this.tabPage_Event.Controls.Add(this.dateTimePicker_Event_EndTime);
            this.tabPage_Event.Controls.Add(this.dateTimePicker_Event_StartTime);
            this.tabPage_Event.Controls.Add(this.button_Event_SearchAll);
            this.tabPage_Event.Controls.Add(this.button_Event_Search);
            this.tabPage_Event.Controls.Add(this.dateTimePicker_Event_EndDate);
            this.tabPage_Event.Controls.Add(this.dateTimePicker_Event_StartDate);
            this.tabPage_Event.Controls.Add(this.button_Event_Update);
            this.tabPage_Event.Controls.Add(this.button_Event_Add);
            this.tabPage_Event.Controls.Add(this.textBox_Event_Description);
            this.tabPage_Event.Controls.Add(this.textBox_Event_Title);
            this.tabPage_Event.Controls.Add(this.listBox_Event_List);
            this.tabPage_Event.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Event.Name = "tabPage_Event";
            this.tabPage_Event.Size = new System.Drawing.Size(1252, 424);
            this.tabPage_Event.TabIndex = 3;
            this.tabPage_Event.Text = "事件";
            this.tabPage_Event.UseVisualStyleBackColor = true;
            this.tabPage_Event.SizeChanged += new System.EventHandler(this.tabPage_Event_SizeChanged);
            // 
            // checkBox_Event_LinkTask
            // 
            this.checkBox_Event_LinkTask.AutoSize = true;
            this.checkBox_Event_LinkTask.Location = new System.Drawing.Point(695, 257);
            this.checkBox_Event_LinkTask.Name = "checkBox_Event_LinkTask";
            this.checkBox_Event_LinkTask.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Event_LinkTask.TabIndex = 19;
            this.checkBox_Event_LinkTask.Text = "任务关联";
            this.checkBox_Event_LinkTask.UseVisualStyleBackColor = true;
            this.checkBox_Event_LinkTask.CheckedChanged += new System.EventHandler(this.checkBox_Event_LinkTask_CheckedChanged);
            this.checkBox_Event_LinkTask.Click += new System.EventHandler(this.checkBox_Event_LinkTask_Click);
            // 
            // button_Event_ScriptTest
            // 
            this.button_Event_ScriptTest.Location = new System.Drawing.Point(613, 252);
            this.button_Event_ScriptTest.Name = "button_Event_ScriptTest";
            this.button_Event_ScriptTest.Size = new System.Drawing.Size(75, 23);
            this.button_Event_ScriptTest.TabIndex = 18;
            this.button_Event_ScriptTest.Text = "脚本测试";
            this.button_Event_ScriptTest.UseVisualStyleBackColor = true;
            this.button_Event_ScriptTest.Click += new System.EventHandler(this.button_Event_ScriptTest_Click);
            // 
            // checkBox_Event_Enable
            // 
            this.checkBox_Event_Enable.AutoSize = true;
            this.checkBox_Event_Enable.Checked = true;
            this.checkBox_Event_Enable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Event_Enable.Location = new System.Drawing.Point(557, 257);
            this.checkBox_Event_Enable.Name = "checkBox_Event_Enable";
            this.checkBox_Event_Enable.Size = new System.Drawing.Size(48, 16);
            this.checkBox_Event_Enable.TabIndex = 17;
            this.checkBox_Event_Enable.Text = "启用";
            this.checkBox_Event_Enable.UseVisualStyleBackColor = true;
            this.checkBox_Event_Enable.CheckedChanged += new System.EventHandler(this.checkBox_Event_Enable_CheckedChanged);
            // 
            // elementHost_Event_RemindFormula
            // 
            this.elementHost_Event_RemindFormula.Location = new System.Drawing.Point(308, 288);
            this.elementHost_Event_RemindFormula.Name = "elementHost_Event_RemindFormula";
            this.elementHost_Event_RemindFormula.Size = new System.Drawing.Size(478, 93);
            this.elementHost_Event_RemindFormula.TabIndex = 16;
            this.elementHost_Event_RemindFormula.Text = "elementHost1";
            this.elementHost_Event_RemindFormula.Child = null;
            // 
            // dateTimePicker_Event_EndTime
            // 
            this.dateTimePicker_Event_EndTime.CustomFormat = "HH:mm";
            this.dateTimePicker_Event_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Event_EndTime.Location = new System.Drawing.Point(453, 254);
            this.dateTimePicker_Event_EndTime.Name = "dateTimePicker_Event_EndTime";
            this.dateTimePicker_Event_EndTime.Size = new System.Drawing.Size(74, 21);
            this.dateTimePicker_Event_EndTime.TabIndex = 15;
            this.dateTimePicker_Event_EndTime.ValueChanged += new System.EventHandler(this.dateTimePicker_Event_EndTime_ValueChanged);
            // 
            // dateTimePicker_Event_StartTime
            // 
            this.dateTimePicker_Event_StartTime.CustomFormat = "HH:mm";
            this.dateTimePicker_Event_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Event_StartTime.Location = new System.Drawing.Point(352, 254);
            this.dateTimePicker_Event_StartTime.Name = "dateTimePicker_Event_StartTime";
            this.dateTimePicker_Event_StartTime.Size = new System.Drawing.Size(77, 21);
            this.dateTimePicker_Event_StartTime.TabIndex = 14;
            this.dateTimePicker_Event_StartTime.ValueChanged += new System.EventHandler(this.dateTimePicker_Event_StartTime_ValueChanged);
            // 
            // button_Event_SearchAll
            // 
            this.button_Event_SearchAll.Location = new System.Drawing.Point(571, 6);
            this.button_Event_SearchAll.Name = "button_Event_SearchAll";
            this.button_Event_SearchAll.Size = new System.Drawing.Size(75, 23);
            this.button_Event_SearchAll.TabIndex = 13;
            this.button_Event_SearchAll.Text = "查询全部";
            this.button_Event_SearchAll.UseVisualStyleBackColor = true;
            this.button_Event_SearchAll.Click += new System.EventHandler(this.button_Event_SearchAll_Click);
            // 
            // button_Event_Search
            // 
            this.button_Event_Search.Location = new System.Drawing.Point(471, 6);
            this.button_Event_Search.Name = "button_Event_Search";
            this.button_Event_Search.Size = new System.Drawing.Size(75, 23);
            this.button_Event_Search.TabIndex = 12;
            this.button_Event_Search.Text = "查询";
            this.button_Event_Search.UseVisualStyleBackColor = true;
            this.button_Event_Search.Click += new System.EventHandler(this.button_Event_Search_Click);
            // 
            // dateTimePicker_Event_EndDate
            // 
            this.dateTimePicker_Event_EndDate.Location = new System.Drawing.Point(250, 7);
            this.dateTimePicker_Event_EndDate.Name = "dateTimePicker_Event_EndDate";
            this.dateTimePicker_Event_EndDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_Event_EndDate.TabIndex = 11;
            // 
            // dateTimePicker_Event_StartDate
            // 
            this.dateTimePicker_Event_StartDate.Location = new System.Drawing.Point(25, 7);
            this.dateTimePicker_Event_StartDate.Name = "dateTimePicker_Event_StartDate";
            this.dateTimePicker_Event_StartDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_Event_StartDate.TabIndex = 10;
            // 
            // button_Event_Update
            // 
            this.button_Event_Update.Location = new System.Drawing.Point(581, 387);
            this.button_Event_Update.Name = "button_Event_Update";
            this.button_Event_Update.Size = new System.Drawing.Size(75, 23);
            this.button_Event_Update.TabIndex = 9;
            this.button_Event_Update.Text = "更新";
            this.button_Event_Update.UseVisualStyleBackColor = true;
            this.button_Event_Update.Click += new System.EventHandler(this.button_Event_Update_Click);
            // 
            // button_Event_Add
            // 
            this.button_Event_Add.Location = new System.Drawing.Point(438, 387);
            this.button_Event_Add.Name = "button_Event_Add";
            this.button_Event_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Event_Add.TabIndex = 8;
            this.button_Event_Add.Text = "添加";
            this.button_Event_Add.UseVisualStyleBackColor = true;
            this.button_Event_Add.Click += new System.EventHandler(this.button_Event_Add_Click);
            // 
            // textBox_Event_Description
            // 
            this.textBox_Event_Description.Location = new System.Drawing.Point(308, 66);
            this.textBox_Event_Description.Multiline = true;
            this.textBox_Event_Description.Name = "textBox_Event_Description";
            this.textBox_Event_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Event_Description.Size = new System.Drawing.Size(478, 176);
            this.textBox_Event_Description.TabIndex = 7;
            this.textBox_Event_Description.TextChanged += new System.EventHandler(this.textBox_Event_Description_TextChanged);
            // 
            // textBox_Event_Title
            // 
            this.textBox_Event_Title.Location = new System.Drawing.Point(307, 39);
            this.textBox_Event_Title.Name = "textBox_Event_Title";
            this.textBox_Event_Title.Size = new System.Drawing.Size(478, 21);
            this.textBox_Event_Title.TabIndex = 6;
            this.textBox_Event_Title.TextChanged += new System.EventHandler(this.textBox_Event_Title_TextChanged);
            // 
            // listBox_Event_List
            // 
            this.listBox_Event_List.FormattingEnabled = true;
            this.listBox_Event_List.ItemHeight = 12;
            this.listBox_Event_List.Location = new System.Drawing.Point(6, 39);
            this.listBox_Event_List.Name = "listBox_Event_List";
            this.listBox_Event_List.Size = new System.Drawing.Size(295, 376);
            this.listBox_Event_List.TabIndex = 5;
            this.listBox_Event_List.Click += new System.EventHandler(this.listBox_Event_List_Click);
            this.listBox_Event_List.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox_Event_List_KeyDown);
            // 
            // tabPage_Schedule
            // 
            this.tabPage_Schedule.Controls.Add(this.checkBox_Schedule_Remind);
            this.tabPage_Schedule.Controls.Add(this.button_Schedule_Search);
            this.tabPage_Schedule.Controls.Add(this.dateTimePicker_Schedule_EndDate);
            this.tabPage_Schedule.Controls.Add(this.dateTimePicker_Schedule_StartDate);
            this.tabPage_Schedule.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Schedule.Name = "tabPage_Schedule";
            this.tabPage_Schedule.Size = new System.Drawing.Size(1252, 424);
            this.tabPage_Schedule.TabIndex = 4;
            this.tabPage_Schedule.Text = "日程";
            this.tabPage_Schedule.UseVisualStyleBackColor = true;
            this.tabPage_Schedule.SizeChanged += new System.EventHandler(this.tabPage_Schedule_SizeChanged);
            // 
            // button_Schedule_Search
            // 
            this.button_Schedule_Search.Location = new System.Drawing.Point(463, 11);
            this.button_Schedule_Search.Name = "button_Schedule_Search";
            this.button_Schedule_Search.Size = new System.Drawing.Size(75, 23);
            this.button_Schedule_Search.TabIndex = 15;
            this.button_Schedule_Search.Text = "查询";
            this.button_Schedule_Search.UseVisualStyleBackColor = true;
            this.button_Schedule_Search.Click += new System.EventHandler(this.button_Schedule_Search_Click);
            // 
            // dateTimePicker_Schedule_EndDate
            // 
            this.dateTimePicker_Schedule_EndDate.Location = new System.Drawing.Point(242, 12);
            this.dateTimePicker_Schedule_EndDate.Name = "dateTimePicker_Schedule_EndDate";
            this.dateTimePicker_Schedule_EndDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_Schedule_EndDate.TabIndex = 14;
            // 
            // dateTimePicker_Schedule_StartDate
            // 
            this.dateTimePicker_Schedule_StartDate.Location = new System.Drawing.Point(17, 12);
            this.dateTimePicker_Schedule_StartDate.Name = "dateTimePicker_Schedule_StartDate";
            this.dateTimePicker_Schedule_StartDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker_Schedule_StartDate.TabIndex = 13;
            // 
            // checkBox_Schedule_Remind
            // 
            this.checkBox_Schedule_Remind.AutoSize = true;
            this.checkBox_Schedule_Remind.Checked = true;
            this.checkBox_Schedule_Remind.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Schedule_Remind.Location = new System.Drawing.Point(570, 16);
            this.checkBox_Schedule_Remind.Name = "checkBox_Schedule_Remind";
            this.checkBox_Schedule_Remind.Size = new System.Drawing.Size(48, 16);
            this.checkBox_Schedule_Remind.TabIndex = 16;
            this.checkBox_Schedule_Remind.Text = "提醒";
            this.checkBox_Schedule_Remind.UseVisualStyleBackColor = true;
            this.checkBox_Schedule_Remind.CheckedChanged += new System.EventHandler(this.checkBox_Schedule_Remind_CheckedChanged);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1486, 654);
            this.Controls.Add(this.tabControl_Main);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "个人日程管理 1.0 beta 202009230757 by 李志锐 QQ：859067292";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Main_FormClosed);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.SizeChanged += new System.EventHandler(this.Form_Main_SizeChanged);
            this.tabControl_Main.ResumeLayout(false);
            this.tabPage_Memo.ResumeLayout(false);
            this.tabPage_Memo.PerformLayout();
            this.tabPage_Task.ResumeLayout(false);
            this.panel_Task_Data.ResumeLayout(false);
            this.panel_Task_Data.PerformLayout();
            this.tabPage_Record.ResumeLayout(false);
            this.tabPage_Event.ResumeLayout(false);
            this.tabPage_Event.PerformLayout();
            this.tabPage_Schedule.ResumeLayout(false);
            this.tabPage_Schedule.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.TabPage tabPage_Memo;
        private System.Windows.Forms.TabPage tabPage_Task;
        private System.Windows.Forms.ListBox listBox_Memo_List;
        private System.Windows.Forms.TextBox textBox_Memo_Description;
        private System.Windows.Forms.TextBox textBox_Memo_Title;
        private System.Windows.Forms.Button button_Memo_Add;
        private System.Windows.Forms.Button button_Memo_Update;
        private System.Windows.Forms.ListView listView_Task_Item;
        private System.Windows.Forms.TreeView treeView_Task_Dir;
        private System.Windows.Forms.ColumnHeader columnHeader_Task_List_Title;
        private System.Windows.Forms.ColumnHeader columnHeader_Task_List_Progress;
        private System.Windows.Forms.ColumnHeader columnHeader_Task_List_ProgressPercent;
        private System.Windows.Forms.ColumnHeader columnHeader_Task_List_CreatedTime;
        private System.Windows.Forms.Panel panel_Task_Data;
        private System.Windows.Forms.TextBox textBox_Task_ProgressPercent;
        private System.Windows.Forms.TextBox textBox_Task_Unit;
        private System.Windows.Forms.TextBox textBox_Task_TotalProgress;
        private System.Windows.Forms.Label label_Task_Separate;
        private System.Windows.Forms.TextBox textBox_Task_FinishedProgress;
        private System.Windows.Forms.Label label_Task_Progress;
        private System.Windows.Forms.TextBox textBox_Task_Description;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Task_Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Task_Add;
        private System.Windows.Forms.Button button_Task_Update;
        private System.Windows.Forms.ColumnHeader columnHeader_Task_FinishSpeed;
        private System.Windows.Forms.ColumnHeader columnHeader_Task_RemainedTaskFinishMinimumSpeed;
        private System.Windows.Forms.ColumnHeader columnHeader_LastTaskFinishTime;
        private System.Windows.Forms.TabPage tabPage_Record;
        private System.Windows.Forms.ListView listView_Record_List;
        private System.Windows.Forms.ColumnHeader columnHeader_Record_Time;
        private System.Windows.Forms.ColumnHeader columnHeader_Description;
        private System.Windows.Forms.Button button_Record_Search;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Record_EndTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Record_StartTime;
        private System.Windows.Forms.TabPage tabPage_Event;
        private System.Windows.Forms.Button button_Event_SearchAll;
        private System.Windows.Forms.Button button_Event_Search;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Event_EndDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Event_StartDate;
        private System.Windows.Forms.Button button_Event_Update;
        private System.Windows.Forms.Button button_Event_Add;
        private System.Windows.Forms.TextBox textBox_Event_Description;
        private System.Windows.Forms.TextBox textBox_Event_Title;
        private System.Windows.Forms.ListBox listBox_Event_List;
        private System.Windows.Forms.Integration.ElementHost elementHost_Event_RemindFormula;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Event_EndTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Event_StartTime;
        private System.Windows.Forms.CheckBox checkBox_Event_Enable;
        private System.Windows.Forms.Button button_Event_ScriptTest;
        private System.Windows.Forms.TabPage tabPage_Schedule;
        private System.Windows.Forms.Button button_Schedule_Search;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Schedule_EndDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Schedule_StartDate;
        private System.Windows.Forms.CheckBox checkBox_Event_LinkTask;
        private System.Windows.Forms.CheckBox checkBox_Schedule_Remind;
    }
}