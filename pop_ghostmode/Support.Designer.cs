namespace pop_ghostmode
{
    partial class Support
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Support));
            this.label_header = new System.Windows.Forms.Label();
            this.label_pc = new System.Windows.Forms.Label();
            this.label_ps2 = new System.Windows.Forms.Label();
            this.label_psp = new System.Windows.Forms.Label();
            this.label_notes = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_header
            // 
            this.label_header.AutoSize = true;
            this.label_header.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_header.Location = new System.Drawing.Point(160, 9);
            this.label_header.Name = "label_header";
            this.label_header.Size = new System.Drawing.Size(187, 25);
            this.label_header.TabIndex = 0;
            this.label_header.Text = "Supported games:";
            // 
            // label_pc
            // 
            this.label_pc.AutoSize = true;
            this.label_pc.Location = new System.Drawing.Point(12, 71);
            this.label_pc.Name = "label_pc";
            this.label_pc.Size = new System.Drawing.Size(124, 78);
            this.label_pc.TabIndex = 1;
            this.label_pc.Text = "PC*\r\n\r\nWW (05/11/04 release) \r\nWW (29/09/04 demo) \r\nWW (20/01/05 demo)\r\nT2T (14/1" +
    "1/05 release)";
            this.label_pc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_ps2
            // 
            this.label_ps2.AutoSize = true;
            this.label_ps2.Location = new System.Drawing.Point(158, 71);
            this.label_ps2.Name = "label_ps2";
            this.label_ps2.Size = new System.Drawing.Size(191, 104);
            this.label_ps2.TabIndex = 2;
            this.label_ps2.Text = resources.GetString("label_ps2.Text");
            this.label_ps2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_psp
            // 
            this.label_psp.AutoSize = true;
            this.label_psp.Location = new System.Drawing.Point(375, 71);
            this.label_psp.Name = "label_psp";
            this.label_psp.Size = new System.Drawing.Size(140, 52);
            this.label_psp.TabIndex = 3;
            this.label_psp.Text = "PSP***\r\n\r\nRevelations (ULUS-10063)\r\nRival Swords (ULUS-10240)";
            this.label_psp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_notes
            // 
            this.label_notes.AutoSize = true;
            this.label_notes.Location = new System.Drawing.Point(15, 222);
            this.label_notes.Name = "label_notes";
            this.label_notes.Size = new System.Drawing.Size(244, 39);
            this.label_notes.TabIndex = 4;
            this.label_notes.Text = "*only drm-free version is supported.\r\n**tested on PCSX2 1.6.0 release (32-bit).\r\n" +
    "***works only with PPSSPP 1.11.3 release (32-bit).";
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(359, 228);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 5;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // Support
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 272);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label_notes);
            this.Controls.Add(this.label_psp);
            this.Controls.Add(this.label_ps2);
            this.Controls.Add(this.label_pc);
            this.Controls.Add(this.label_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Support";
            this.Text = "Support";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_header;
        private System.Windows.Forms.Label label_pc;
        private System.Windows.Forms.Label label_ps2;
        private System.Windows.Forms.Label label_psp;
        private System.Windows.Forms.Label label_notes;
        private System.Windows.Forms.Button btn_close;
    }
}