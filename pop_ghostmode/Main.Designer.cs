namespace pop_ghostmode
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.label_status = new System.Windows.Forms.Label();
            this.label_pointer = new System.Windows.Forms.Label();
            this.label_ghostmode = new System.Windows.Forms.Label();
            this.label_movement = new System.Windows.Forms.Label();
            this.label_collision = new System.Windows.Forms.Label();
            this.label_animation = new System.Windows.Forms.Label();
            this.label_log = new System.Windows.Forms.Label();
            this.label_about = new System.Windows.Forms.Label();
            this.label_support = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBox_log.Location = new System.Drawing.Point(30, 202);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.ReadOnly = true;
            this.richTextBox_log.Size = new System.Drawing.Size(352, 96);
            this.richTextBox_log.TabIndex = 0;
            this.richTextBox_log.Text = "";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(28, 25);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(40, 13);
            this.label_status.TabIndex = 1;
            this.label_status.Text = "Status:";
            // 
            // label_pointer
            // 
            this.label_pointer.AutoSize = true;
            this.label_pointer.Location = new System.Drawing.Point(28, 50);
            this.label_pointer.Name = "label_pointer";
            this.label_pointer.Size = new System.Drawing.Size(72, 13);
            this.label_pointer.TabIndex = 2;
            this.label_pointer.Text = "Pointer value:";
            // 
            // label_ghostmode
            // 
            this.label_ghostmode.AutoSize = true;
            this.label_ghostmode.Location = new System.Drawing.Point(28, 75);
            this.label_ghostmode.Name = "label_ghostmode";
            this.label_ghostmode.Size = new System.Drawing.Size(93, 13);
            this.label_ghostmode.TabIndex = 3;
            this.label_ghostmode.Text = "Ghostmode value:";
            // 
            // label_movement
            // 
            this.label_movement.AutoSize = true;
            this.label_movement.Location = new System.Drawing.Point(28, 100);
            this.label_movement.Name = "label_movement";
            this.label_movement.Size = new System.Drawing.Size(89, 13);
            this.label_movement.TabIndex = 4;
            this.label_movement.Text = "Movement value:";
            // 
            // label_collision
            // 
            this.label_collision.AutoSize = true;
            this.label_collision.Location = new System.Drawing.Point(28, 125);
            this.label_collision.Name = "label_collision";
            this.label_collision.Size = new System.Drawing.Size(77, 13);
            this.label_collision.TabIndex = 5;
            this.label_collision.Text = "Collision value:";
            // 
            // label_animation
            // 
            this.label_animation.AutoSize = true;
            this.label_animation.Location = new System.Drawing.Point(28, 150);
            this.label_animation.Name = "label_animation";
            this.label_animation.Size = new System.Drawing.Size(85, 13);
            this.label_animation.TabIndex = 6;
            this.label_animation.Text = "Animation value:";
            // 
            // label_log
            // 
            this.label_log.AutoSize = true;
            this.label_log.Location = new System.Drawing.Point(28, 186);
            this.label_log.Name = "label_log";
            this.label_log.Size = new System.Drawing.Size(25, 13);
            this.label_log.TabIndex = 7;
            this.label_log.Text = "Log";
            // 
            // label_about
            // 
            this.label_about.AutoSize = true;
            this.label_about.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_about.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_about.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_about.Location = new System.Drawing.Point(90, 312);
            this.label_about.Name = "label_about";
            this.label_about.Size = new System.Drawing.Size(227, 13);
            this.label_about.TabIndex = 8;
            this.label_about.Text = "Prince of Persia: WW/T2T ghostmode enabler";
            this.label_about.Click += new System.EventHandler(this.label_about_Click);
            // 
            // label_support
            // 
            this.label_support.AutoSize = true;
            this.label_support.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_support.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_support.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_support.Location = new System.Drawing.Point(277, 150);
            this.label_support.Name = "label_support";
            this.label_support.Size = new System.Drawing.Size(105, 13);
            this.label_support.TabIndex = 9;
            this.label_support.Text = "Supported games list";
            this.label_support.Click += new System.EventHandler(this.label_support_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 337);
            this.Controls.Add(this.label_support);
            this.Controls.Add(this.label_about);
            this.Controls.Add(this.label_log);
            this.Controls.Add(this.label_animation);
            this.Controls.Add(this.label_collision);
            this.Controls.Add(this.label_movement);
            this.Controls.Add(this.label_ghostmode);
            this.Controls.Add(this.label_pointer);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.richTextBox_log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Prince of Persia WW/T2T ghostmode enabler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_pointer;
        private System.Windows.Forms.Label label_ghostmode;
        private System.Windows.Forms.Label label_movement;
        private System.Windows.Forms.Label label_collision;
        private System.Windows.Forms.Label label_animation;
        private System.Windows.Forms.Label label_log;
        private System.Windows.Forms.Label label_about;
        private System.Windows.Forms.Label label_support;
    }
}

