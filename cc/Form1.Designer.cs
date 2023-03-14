namespace cc
{
    partial class MainClass
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
            if (disposing && (components != null))
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.StartButton = new System.Windows.Forms.Button();
            this.reset_button = new System.Windows.Forms.Button();
            this.stage_count = new System.Windows.Forms.Label();
            this.stage_num = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1324, 0);
            this.panel1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.StartButton);
            this.flowLayoutPanel1.Controls.Add(this.reset_button);
            this.flowLayoutPanel1.Controls.Add(this.stage_count);
            this.flowLayoutPanel1.Controls.Add(this.stage_num);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1324, 42);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(3, 3);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // reset_button
            // 
            this.reset_button.Location = new System.Drawing.Point(84, 3);
            this.reset_button.Name = "reset_button";
            this.reset_button.Size = new System.Drawing.Size(75, 23);
            this.reset_button.TabIndex = 7;
            this.reset_button.Text = "Reset";
            this.reset_button.UseVisualStyleBackColor = true;
            // 
            // stage_count
            // 
            this.stage_count.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.stage_count.AutoSize = true;
            this.stage_count.Location = new System.Drawing.Point(165, 8);
            this.stage_count.Name = "stage_count";
            this.stage_count.Size = new System.Drawing.Size(41, 13);
            this.stage_count.TabIndex = 8;
            this.stage_count.Text = "Stage: ";
            this.stage_count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stage_num
            // 
            this.stage_num.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.stage_num.AutoSize = true;
            this.stage_num.Location = new System.Drawing.Point(212, 8);
            this.stage_num.Name = "stage_num";
            this.stage_num.Size = new System.Drawing.Size(13, 13);
            this.stage_num.TabIndex = 9;
            this.stage_num.Text = "1";
            // 
            // MainClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1324, 659);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "MainClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CyberCode";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainClass_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button reset_button;
        private System.Windows.Forms.Label stage_count;
        private System.Windows.Forms.Label stage_num;
    }
}