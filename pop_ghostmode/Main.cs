using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace pop_ghostmode
{
    public partial class Main : Form
    {
        bool ww_running = false;
        bool t2t_running = false;
        bool pcsx2_running = false;
        bool ppsspp_running = false;
        bool first_input = false;

        uint ww_state_normal = 0;
        uint ww_animation_normal = 0;
        uint t2t_state_normal = 0;
        uint t2t_animation_normal = 0;

        string command = "";

        public Main()
        {
            InitializeComponent();

            Thread process_watcher;
            process_watcher = new Thread(UpdateProcessStatus);
            process_watcher.Start();
            process_watcher.IsBackground = true;

            richTextBox_log.Text += "Program started. Start game and press BACKSPACE to switch between ghost/normal mode ingame." + " ";
            richTextBox_log.Text += "Press HOME to switch primary weapon." + "\n";
            richTextBox_log.Text += "WARNING: to avoid crashes switch into ghost mode and weapon, when Prince is standing still!" + "\n";
        }

        private void label_about_Click(object sender, EventArgs e)
        {
            About about_dialog = new About();
            about_dialog.Show();
        }

        private void label_support_Click(object sender, EventArgs e)
        {
            Support support_dialog = new Support();
            support_dialog.Show();
        }
    }
}
