namespace 个人日程管理
{
    partial class Form_TaskLinker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TaskLinker));
            this.treeView_Task_Dir = new System.Windows.Forms.TreeView();
            this.button_Link = new System.Windows.Forms.Button();
            this.button_Unlink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView_Task_Dir
            // 
            this.treeView_Task_Dir.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView_Task_Dir.HideSelection = false;
            this.treeView_Task_Dir.Location = new System.Drawing.Point(12, 12);
            this.treeView_Task_Dir.Name = "treeView_Task_Dir";
            this.treeView_Task_Dir.Size = new System.Drawing.Size(216, 408);
            this.treeView_Task_Dir.TabIndex = 1;
            this.treeView_Task_Dir.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_Task_Dir_DrawNode);
            // 
            // button_Link
            // 
            this.button_Link.Location = new System.Drawing.Point(30, 438);
            this.button_Link.Name = "button_Link";
            this.button_Link.Size = new System.Drawing.Size(75, 23);
            this.button_Link.TabIndex = 2;
            this.button_Link.Text = "关联";
            this.button_Link.UseVisualStyleBackColor = true;
            this.button_Link.Click += new System.EventHandler(this.button_Link_Click);
            // 
            // button_Unlink
            // 
            this.button_Unlink.Location = new System.Drawing.Point(127, 438);
            this.button_Unlink.Name = "button_Unlink";
            this.button_Unlink.Size = new System.Drawing.Size(75, 23);
            this.button_Unlink.TabIndex = 3;
            this.button_Unlink.Text = "取消关联";
            this.button_Unlink.UseVisualStyleBackColor = true;
            this.button_Unlink.Click += new System.EventHandler(this.button_Unlink_Click);
            // 
            // Form_TaskLinker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 479);
            this.Controls.Add(this.button_Unlink);
            this.Controls.Add(this.button_Link);
            this.Controls.Add(this.treeView_Task_Dir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TaskLinker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "任务关联器";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_Task_Dir;
        private System.Windows.Forms.Button button_Link;
        private System.Windows.Forms.Button button_Unlink;
    }
}