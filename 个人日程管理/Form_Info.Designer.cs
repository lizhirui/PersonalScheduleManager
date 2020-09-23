namespace 个人日程管理
{
    partial class Form_Info
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
            this.textBox_Info = new System.Windows.Forms.TextBox();
            this.button_Left = new System.Windows.Forms.Button();
            this.button_Right = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_Info
            // 
            this.textBox_Info.Location = new System.Drawing.Point(12, 12);
            this.textBox_Info.Multiline = true;
            this.textBox_Info.Name = "textBox_Info";
            this.textBox_Info.ReadOnly = true;
            this.textBox_Info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Info.Size = new System.Drawing.Size(776, 390);
            this.textBox_Info.TabIndex = 0;
            // 
            // button_Left
            // 
            this.button_Left.Location = new System.Drawing.Point(232, 415);
            this.button_Left.Name = "button_Left";
            this.button_Left.Size = new System.Drawing.Size(98, 23);
            this.button_Left.TabIndex = 1;
            this.button_Left.Text = "button1";
            this.button_Left.UseVisualStyleBackColor = true;
            // 
            // button_Right
            // 
            this.button_Right.Location = new System.Drawing.Point(430, 415);
            this.button_Right.Name = "button_Right";
            this.button_Right.Size = new System.Drawing.Size(98, 23);
            this.button_Right.TabIndex = 2;
            this.button_Right.Text = "button2";
            this.button_Right.UseVisualStyleBackColor = true;
            // 
            // Form_Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_Right);
            this.Controls.Add(this.button_Left);
            this.Controls.Add(this.textBox_Info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Info";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Info;
        private System.Windows.Forms.Button button_Left;
        private System.Windows.Forms.Button button_Right;
    }
}