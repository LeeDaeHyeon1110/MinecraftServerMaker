﻿namespace MinecraftServerMaker
{
    partial class InstallSpigot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallSpigot));
            this.showLogCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressLbl = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.logBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // showLogCheck
            // 
            this.showLogCheck.AutoSize = true;
            this.showLogCheck.Location = new System.Drawing.Point(394, 12);
            this.showLogCheck.Name = "showLogCheck";
            this.showLogCheck.Size = new System.Drawing.Size(78, 19);
            this.showLogCheck.TabIndex = 13;
            this.showLogCheck.Text = "로그 보기";
            this.showLogCheck.UseVisualStyleBackColor = true;
            this.showLogCheck.CheckedChanged += new System.EventHandler(this.showLogCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.label1.Location = new System.Drawing.Point(121, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 25);
            this.label1.TabIndex = 12;
            this.label1.Text = "잠시 딴짓을 하고 있어도 됨!";
            // 
            // progressLbl
            // 
            this.progressLbl.AutoSize = true;
            this.progressLbl.BackColor = System.Drawing.Color.Transparent;
            this.progressLbl.Location = new System.Drawing.Point(123, 51);
            this.progressLbl.Name = "progressLbl";
            this.progressLbl.Size = new System.Drawing.Size(24, 15);
            this.progressLbl.TabIndex = 11;
            this.progressLbl.Text = "0%";
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(126, 69);
            this.progress.MarqueeAnimationSpeed = 20;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(346, 29);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progress.TabIndex = 10;
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.Color.White;
            this.logBox.Location = new System.Drawing.Point(12, 111);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(460, 273);
            this.logBox.TabIndex = 9;
            this.logBox.WordWrap = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-11, -14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(157, 132);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // InstallSpigot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 111);
            this.Controls.Add(this.showLogCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressLbl);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "InstallSpigot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "스피곳 서버 설치";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InstallSpigot_FormClosing);
            this.Load += new System.EventHandler(this.InstallSpigot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showLogCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label progressLbl;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}