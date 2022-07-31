namespace MinecraftServerMaker
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.getSpigotBtn = new System.Windows.Forms.Button();
            this.getForgeBtn = new System.Windows.Forms.Button();
            this.jarList = new System.Windows.Forms.ListBox();
            this.downloadJarBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // getSpigotBtn
            // 
            this.getSpigotBtn.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.getSpigotBtn.Location = new System.Drawing.Point(12, 12);
            this.getSpigotBtn.Name = "getSpigotBtn";
            this.getSpigotBtn.Size = new System.Drawing.Size(187, 54);
            this.getSpigotBtn.TabIndex = 1;
            this.getSpigotBtn.Text = "스피곳 서버 만들기\r\n(플러그인 서버 만들기)";
            this.getSpigotBtn.UseVisualStyleBackColor = true;
            this.getSpigotBtn.Click += new System.EventHandler(this.getSpigotBtn_Click);
            // 
            // getForgeBtn
            // 
            this.getForgeBtn.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.getForgeBtn.Location = new System.Drawing.Point(205, 12);
            this.getForgeBtn.Name = "getForgeBtn";
            this.getForgeBtn.Size = new System.Drawing.Size(156, 54);
            this.getForgeBtn.TabIndex = 2;
            this.getForgeBtn.Text = "포지 서버 만들기\r\n(모드 서버 만들기)";
            this.getForgeBtn.UseVisualStyleBackColor = true;
            this.getForgeBtn.Click += new System.EventHandler(this.getForgeBtn_Click);
            // 
            // jarList
            // 
            this.jarList.FormattingEnabled = true;
            this.jarList.ItemHeight = 15;
            this.jarList.Location = new System.Drawing.Point(13, 88);
            this.jarList.Name = "jarList";
            this.jarList.Size = new System.Drawing.Size(348, 409);
            this.jarList.TabIndex = 3;
            this.jarList.SelectedIndexChanged += new System.EventHandler(this.jarList_SelectedIndexChanged);
            // 
            // downloadJarBtn
            // 
            this.downloadJarBtn.Enabled = false;
            this.downloadJarBtn.Location = new System.Drawing.Point(13, 503);
            this.downloadJarBtn.Name = "downloadJarBtn";
            this.downloadJarBtn.Size = new System.Drawing.Size(348, 23);
            this.downloadJarBtn.TabIndex = 4;
            this.downloadJarBtn.Text = "다운로드";
            this.downloadJarBtn.UseVisualStyleBackColor = true;
            this.downloadJarBtn.Click += new System.EventHandler(this.downloadJarBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "버전 리스트";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(372, 537);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downloadJarBtn);
            this.Controls.Add(this.jarList);
            this.Controls.Add(this.getForgeBtn);
            this.Controls.Add(this.getSpigotBtn);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "마인크래프트 서버 만들기 프로그램";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button getSpigotBtn;
        private System.Windows.Forms.Button getForgeBtn;
        private System.Windows.Forms.ListBox jarList;
        private System.Windows.Forms.Button downloadJarBtn;
        private System.Windows.Forms.Label label1;
    }
}

