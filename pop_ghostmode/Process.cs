using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace pop_ghostmode
{
    public partial class Main : Form
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hProcess);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        public static void WriteMem64(Process p, int address, ulong v)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.All, false, (int)p.Id);
            byte[] val = BitConverter.GetBytes(v);


            int bytesWritten = 0;
            WriteProcessMemory(hProc, new IntPtr(address), val, (UInt32)val.LongLength, out bytesWritten);

            CloseHandle(hProc);
        }
        public static void WriteMem32(Process p, int address, uint v)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.All, false, (int)p.Id);
            byte[] val = BitConverter.GetBytes(v);


            int bytesWritten = 0;
            WriteProcessMemory(hProc, new IntPtr(address), val, (UInt32)val.Length, out bytesWritten);

            CloseHandle(hProc);
        }
        public static void WriteMem16(Process p, int address, ushort v)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.All, false, (int)p.Id);
            byte[] val = BitConverter.GetBytes(v);


            int bytesWritten = 0;
            WriteProcessMemory(hProc, new IntPtr(address), val, (UInt32)val.Length, out bytesWritten);

            CloseHandle(hProc);
        }
        public static uint ReadMem(Process p, int address)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[4];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, address, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            uint res = System.BitConverter.ToUInt32(val, 0);
            return res;
        }
        public static ushort ReadMem16(Process p, int address)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[2];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, address, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            ushort res = System.BitConverter.ToUInt16(val, 0);
            return res;
        }
        public static string ReadMemString(Process p, int address)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[20];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, address, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            string res = Encoding.Default.GetString(val);
            return res;
        }

        public static int ReadMemPointer(Process p, int address, int offset1)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[4];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, (int)p.MainModule.BaseAddress + address, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            int res = System.BitConverter.ToInt32(val, 0) + offset1;
            return res;
        }
        public static int ReadMemPointer(Process p, int address, int offset1, int offset2)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[4];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, (int)p.MainModule.BaseAddress + address, val, (int)val.LongLength, ref bytesRead);
            int ptr1_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr1_addr + offset1, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            int res = System.BitConverter.ToInt32(val, 0) + offset2;
            return res;
        }
        public static int ReadMemPointer(Process p, int address, int offset1, int offset2, int offset3)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[4];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, (int)p.MainModule.BaseAddress + address, val, (int)val.LongLength, ref bytesRead);
            int ptr1_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr1_addr + offset1, val, (int)val.LongLength, ref bytesRead);
            int ptr2_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr2_addr + offset2, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            int res = System.BitConverter.ToInt32(val, 0) + offset3;
            return res;
        }
        public static int ReadMemPointer(Process p, int address, int offset1, int offset2, int offset3, int offset4)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[4];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, (int)p.MainModule.BaseAddress + address, val, (int)val.LongLength, ref bytesRead);
            int ptr1_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr1_addr + offset1, val, (int)val.LongLength, ref bytesRead);
            int ptr2_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr2_addr + offset2, val, (int)val.LongLength, ref bytesRead);
            int ptr3_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr3_addr + offset3, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            int res = System.BitConverter.ToInt32(val, 0) + offset4;

            return res;
        }
        public static int ReadMemPointer(Process p, int address, int offset1, int offset2, int offset3, int offset4, int offset5)
        {
            var hProc = OpenProcess((int)ProcessAccessFlags.VMRead, false, (int)p.Id);
            var val = new byte[4];

            int bytesRead = 0;
            ReadProcessMemory((int)hProc, (int)p.MainModule.BaseAddress + address, val, (int)val.LongLength, ref bytesRead);
            int ptr1_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr1_addr + offset1, val, (int)val.LongLength, ref bytesRead);
            int ptr2_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr2_addr + offset2, val, (int)val.LongLength, ref bytesRead);
            int ptr3_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr3_addr + offset3, val, (int)val.LongLength, ref bytesRead);
            int ptr4_addr = System.BitConverter.ToInt32(val, 0);
            ReadProcessMemory((int)hProc, ptr4_addr + offset4, val, (int)val.LongLength, ref bytesRead);

            CloseHandle(hProc);

            int res = System.BitConverter.ToInt32(val, 0) + offset5;

            return res;
        }

        public void UpdateProcessStatus()
        {
            while (true)
            {
                DetectWWProcess();
                DetectT2TProcess();
                DetectPCSX2Process();
                DetectPPSSPPProcess();
                if (ww_running == false && t2t_running == false && pcsx2_running == false && ppsspp_running == false)
                {
                    label_status.Text = "Status: WW/T2T process not running...";
                    label_pointer.Text = "Pointer value: process not running...";
                    label_ghostmode.Text = "Ghostmode value: process not running...";
                    label_movement.Text = "State value: process not running...";
                    label_collision.Text = "Collision value: process not running...";
                    label_animation.Text = "Animation value: process not running...";
                    first_input = false;

                    ww_state_normal = 0;
                    ww_animation_normal = 0;
                    t2t_state_normal = 0;
                }
                else if (ww_running == true)
                {
                    label_status.Text = "Status: pop2.exe process detected!";
                    KeyboardWatcher();
                }
                else if (t2t_running == true)
                {
                    label_status.Text = "Status: pop3.exe process detected!";
                    KeyboardWatcher();
                }
                else if (pcsx2_running == true)
                {
                    label_status.Text = "Status: pcsx2.exe process detected!";
                    KeyboardWatcher();
                }
                else if (ppsspp_running == true)
                {
                    label_status.Text = "Status: ppssppwindows.exe process detected!";
                    KeyboardWatcher();
                }
                Thread.Sleep(50);
            }
        }

        public void DetectWWProcess()
        {
            Process[] pname = Process.GetProcessesByName("pop2");
            if (pname.Length > 0)
            {
                if (ww_running == false)
                {
                    ww_running = true;
                }
            }
            else
            {
                ww_running = false;
            }
        }
        public void DetectT2TProcess()
        {
            Process[] pname = Process.GetProcessesByName("pop3");
            if (pname.Length > 0)
            {
                if (t2t_running == false)
                {
                    t2t_running = true;
                }
            }
            else
            {
                t2t_running = false;
            }
        }
        public void DetectPCSX2Process()
        {
            Process[] pname = Process.GetProcessesByName("pcsx2");
            if (pname.Length > 0)
            {
                if (pcsx2_running == false)
                {
                    pcsx2_running = true;
                }
            }
            else
            {
                pcsx2_running = false;
            }
        }
        public void DetectPPSSPPProcess()
        {
            Process[] pname = Process.GetProcessesByName("ppssppwindows");
            if (pname.Length > 0)
            {
                if (ppsspp_running == false)
                {
                    ppsspp_running = true;
                }
            }
            else
            {
                ppsspp_running = false;
            }
        }
        public void PatchProcess()
        {
            if (ww_running == true)
            {
                PatchWWProcess();
            }
            else if (t2t_running == true)
            {
                PatchT2TProcess();
            }
            else if (pcsx2_running == true)
            {
                PatchPCSX2Process();
            }
            else if (ppsspp_running == true)
            {
                PatchPPSSPPProcess();
            }
        }
    }
}
