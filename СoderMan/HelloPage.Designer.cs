
namespace СoderMan
{
    partial class HelloPage
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelloPage));
            this.StartPageText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartPageText
            // 
            this.StartPageText.BackColor = System.Drawing.Color.Transparent;
            this.StartPageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartPageText.Font = new System.Drawing.Font("Microsoft YaHei UI", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartPageText.ForeColor = System.Drawing.SystemColors.InfoText;
            this.StartPageText.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.StartPageText.Location = new System.Drawing.Point(0, 0);
            this.StartPageText.MinimumSize = new System.Drawing.Size(800, 450);
            this.StartPageText.Name = "StartPageText";
            this.StartPageText.Size = new System.Drawing.Size(800, 450);
            this.StartPageText.TabIndex = 0;
            this.StartPageText.Text = "CoderMan";
            this.StartPageText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StartPageText.Click += new System.EventHandler(this.StartPageText_Click);
            // 
            // HelloPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StartPageText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelloPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CoderMan";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label StartPageText;
    }
}

