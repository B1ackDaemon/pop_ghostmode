namespace pop_ghostmode
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.btn_close = new System.Windows.Forms.Button();
            this.label_credits = new System.Windows.Forms.Label();
            this.label_title = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(69, 384);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 5;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label_credits
            // 
            this.label_credits.AutoSize = true;
            this.label_credits.Location = new System.Drawing.Point(45, 206);
            this.label_credits.Name = "label_credits";
            this.label_credits.Size = new System.Drawing.Size(134, 156);
            this.label_credits.TabIndex = 4;
            this.label_credits.Text = "Created by BlackDaemon.\r\n\r\nThanks to:\r\n\r\nCheatEngine devs\r\nx64dbg devs\r\nPCSX2 dev" +
    "s\r\nPPSSPP devs\r\nghidra devs\r\nghidra-emotionengine devs\r\nghidra-allegrex devs\r\nan" +
    "d many others...";
            this.label_credits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_title.Location = new System.Drawing.Point(36, 9);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(150, 16);
            this.label_title.TabIndex = 3;
            this.label_title.Text = "WW/T2T ghostmode";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::pop_ghostmode.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(28, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 160);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 417);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label_credits);
            this.Controls.Add(this.label_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "About";
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label_credits;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}