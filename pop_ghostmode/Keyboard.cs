using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pop_ghostmode
{
    public partial class Main : Form
    {
        public void KeyboardWatcher()
        {
            if (Keyboard.CurrentKey == "Back" && Keyboard.CurrentKeyNumber != Keyboard.WrittenKeyNumber)
            {
                first_input = true;
                Keyboard.WrittenKeyNumber = Keyboard.CurrentKeyNumber;
                richTextBox_log.Text += "Input detected, switching mode...";
                command = "patch_mode";
                PatchProcess();
            }
            else if(Keyboard.CurrentKey == "Home" && Keyboard.CurrentKeyNumber != Keyboard.WrittenKeyNumber)
            {
                first_input = true;
                Keyboard.WrittenKeyNumber = Keyboard.CurrentKeyNumber;
                richTextBox_log.Text += "Input detected, switching mode...";
                command = "patch_weapon";
                PatchProcess();
            }
            //richTextBox_log.Text += Keyboard.CurrentKey + "\n";

            if (first_input == false)
            {
                label_pointer.Text = "Pointer value: waiting for input...";
                label_ghostmode.Text = "Ghostmode value: waiting for input...";
                label_movement.Text = "State value: waiting for input...";
                label_collision.Text = "Collision value: waiting for input...";
                label_animation.Text = "Animation value: waiting for input...";
            }
        }
    }
    public static class Keyboard
    {
        public static string CurrentKey;
        public static ulong CurrentKeyNumber = 0;
        public static ulong WrittenKeyNumber = 0;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static bool lastKeyWasLetter = false;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _hookID = SetHook(_proc);
            Application.Run(new Main());

            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static void ToggleCapsLock()
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            UnhookWindowsHookEx(_hookID);
            keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            _hookID = SetHook(_proc);
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                if (lastKeyWasLetter)
                {
                    if (Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
                    {
                        //ToggleCapsLock();
                    }
                    lastKeyWasLetter = false;
                }
                Keys key = (Keys)Marshal.ReadInt32(lParam);
                if (key == Keys.Space)
                {
                    if (!Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
                    {
                        //ToggleCapsLock();
                    }
                }
                else if (key >= Keys.A && key <= Keys.Z)
                {
                    lastKeyWasLetter = true;
                }
                Keyboard.CurrentKey = key.ToString();
                Keyboard.CurrentKeyNumber++;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
