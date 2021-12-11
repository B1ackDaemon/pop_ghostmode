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
        int t2t_flymode_pointer = 0x9F9728;
        int t2t_others_pointer = 0x5EBD78;
        int t2t_collision_pointer = 0x9D10B4;
        int t2t_state_pointer = 0x8D9384;


        int t2t_flymode_offset;
        int t2t_state_offset;
        int t2t_collision_offset;
        int t2t_animation_offset;
        int t2t_curlife_offset;
        int t2t_maxlife_offset;
        int t2t_cursands_offset;

        int t2t_animation_pointer = 0x5EBD78;
        int t2t_animation_normal_offset;
        int t2t_animation_ghost_offset;
        int t2t_state_normal_offset;
        int t2t_state_ghost_offset;
        int t2t_damage_offset = 0;

        uint t2t_flymode_val;
        uint t2t_state_val;
        uint t2t_collision_val;
        uint t2t_animation_val;
        uint t2t_curlife_val;
        uint t2t_maxlife_val;
        uint t2t_cursands_val;

        uint t2t_animation_ghost;
        uint t2t_state_ghost;

        uint t2t_damage_enabled = 3;
        uint t2t_damage_disabled = 1;
        uint t2t_flymode_enabled = 0x80C61044;
        uint t2t_flymode_disabled = 0xC0421044;
        uint t2t_collision_enabled = 0xFFFFF;
        uint t2t_collision_disabled = 0xF0000;

        int t2t_weapon_offset = 0;
        int t2t_weapon_collision_offset = 0;
        uint[] t2t_primary_weapon;

        public void PatchT2TProcess()
        {
            Process[] pname = Process.GetProcessesByName("pop3");
            if (pname.Length > 0)
            {
                var p = Process.GetProcessesByName("pop3").FirstOrDefault();

                T2TGameOffsets(p);

                if (command == "patch_mode")
                {
                    T2TPatchMode(p);
                }
                else if (command == "patch_weapon")
                {
                    T2TPatchWeapon(p);
                }


                T2TUpdateInfo(p);
            }
        }

        public void T2TPatchMode(Process p)
        {
            if (t2t_collision_val != t2t_collision_disabled)
            {
                //writing proper state value for ghostmode
                WriteMem32(p, t2t_state_offset, t2t_state_ghost);
                /*if (t2t_state_normal == 0)
                {
                    t2t_state_normal = t2t_state_val;
                    //t2t_animation_normal = t2t_animation_val;
                }
                uint t2t_state_ghost = t2t_state_normal + 0x700;
                WriteMem16(p, t2t_state_offset, (ushort)t2t_state_ghost);*/

                //writing proper animation
                WriteMem32(p, t2t_animation_offset, t2t_animation_ghost);

                //disable damage
                WriteMem32(p, t2t_damage_offset, t2t_damage_disabled);

                //disable collision
                WriteMem32(p, t2t_collision_offset, t2t_collision_disabled);

                //writing ghost mode value, because game tries to restore normal one
                for (int i = 0; i < 5; i++)
                {
                    WriteMem32(p, t2t_flymode_offset, t2t_flymode_enabled);
                    Thread.Sleep(20);
                }

                //restore life
                WriteMem32(p, t2t_curlife_offset, t2t_maxlife_val);

                //restore sands
                WriteMem32(p, t2t_cursands_offset, 6);
            }
            else if (t2t_collision_val == t2t_collision_disabled)
            {
                //writing normal mode value - 0xC0421044
                WriteMem32(p, t2t_flymode_offset, t2t_flymode_disabled);

                //writing normal state
                WriteMem32(p, t2t_state_offset, t2t_state_normal);
                /*if (t2t_state_normal == 0)
                {
                    t2t_state_normal = t2t_state_val - 0x700;
                }
                WriteMem16(p, t2t_state_offset, (ushort)t2t_state_normal);*/

                //writing proper animation
                WriteMem32(p, t2t_animation_offset, t2t_animation_normal);

                //enable damage
                WriteMem32(p, t2t_damage_offset, t2t_damage_enabled);

                //enable collision
                WriteMem32(p, t2t_collision_offset, t2t_collision_enabled);
            }
        }

        public void T2TPatchWeapon(Process p)
        {
            //read current weapon value
            uint t2t_weapon_val = ReadMem(p, t2t_weapon_offset);

            //switch to next weapon
            for (int i = 0; i < t2t_primary_weapon.Length; i++)
            {
                if (t2t_primary_weapon[i] == t2t_weapon_val && i < (t2t_primary_weapon.Length - 1))
                {
                    WriteMem32(p, t2t_weapon_offset, t2t_primary_weapon[i + 1]);
                    WriteMem32(p, t2t_weapon_collision_offset, t2t_primary_weapon[i + 1]);
                }
                else if (t2t_primary_weapon[i] == t2t_weapon_val && i == (t2t_primary_weapon.Length - 1))
                {
                    WriteMem32(p, t2t_weapon_offset, t2t_primary_weapon[0]);
                    WriteMem32(p, t2t_weapon_collision_offset, t2t_primary_weapon[0]);
                }
            }
        }

        public void T2TGameOffsets(Process p)
        {
            //read flymode pointer
            t2t_flymode_offset = ReadMemPointer(p, t2t_flymode_pointer, 0x240);

            //read collision pointer
            t2t_collision_offset = ReadMemPointer(p, t2t_collision_pointer, 0x190);

            //read state pointers
            //t2t_state_offset = ReadMemPointer(p, t2t_state_pointer, 0x6C, 0xE4, 0x48, 0xA4, 0x20);
            //t2t_animation_offset = ReadMemPointer(p, t2t_state_pointer, 0x6C, 0xE4, 0x48, 0xA4, -0x5FD4);
            t2t_state_offset = ReadMemPointer(p, 0x9D10B4, 0x18, 0x4, 0x20);
            t2t_animation_offset = ReadMemPointer(p, 0x9D10B4, 0x18, 0x4, -0x5FD4);

            //read animation pointers
            t2t_animation_ghost_offset = ReadMemPointer(p, t2t_animation_pointer, 0x30, 0x14, 0, 0x30, 0x5C);
            t2t_animation_normal_offset = ReadMemPointer(p, t2t_animation_pointer, 0x30, 0x14, 0, 0x30, 0xC);
            //t2t_state_ghost_offset = ReadMemPointer(p, t2t_animation_pointer, 0x30, 0x14, 0, 0x30, 0x1AB8);
            //t2t_state_normal_offset = ReadMemPointer(p, t2t_animation_pointer, 0x30, 0x14, 0, 0x30, 0x19DC);
            t2t_state_ghost_offset = ReadMemPointer(p, 0xA2B214, 0x4, 0xF8);
            t2t_state_normal_offset = ReadMemPointer(p, 0xA2B214, 0x4, 0x1C);

            //read damage pointer
            t2t_damage_offset = ReadMemPointer(p, t2t_others_pointer, 0x30, 0x18, 0x4, 0x48, 0x7F0);

            //read other pointers
            t2t_curlife_offset = ReadMemPointer(p, t2t_others_pointer, 0x30, 0x18, 0x4, 0x48, 0x2B0);
            t2t_maxlife_offset = ReadMemPointer(p, t2t_others_pointer, 0x30, 0x18, 0x4, 0x48, 0x1C00);
            t2t_cursands_offset = ReadMemPointer(p, t2t_others_pointer, 0x30, 0x18, 0x4, 0x48, 0xD08);

            t2t_weapon_offset = ReadMemPointer(p, t2t_others_pointer, 0x30, 0x18, 0x4, 0x48, 0x998);
            t2t_weapon_collision_offset = t2t_weapon_offset + 0x1278;
            t2t_primary_weapon = new uint[3];
            t2t_primary_weapon[0] = ReadMem(p, t2t_weapon_offset + 0x11FC); //knife
            t2t_primary_weapon[1] = ReadMem(p, t2t_weapon_offset + 0x1200); //dagger
            t2t_primary_weapon[2] = ReadMem(p, t2t_weapon_offset + 0x1204); //sword

            //read values
            t2t_flymode_val = ReadMem(p, t2t_flymode_offset);
            t2t_state_val = ReadMem(p, t2t_state_offset);
            t2t_collision_val = ReadMem(p, t2t_collision_offset);
            t2t_animation_val = ReadMem(p, t2t_animation_offset);
            t2t_curlife_val = ReadMem(p, t2t_curlife_offset);
            t2t_maxlife_val = ReadMem(p, t2t_maxlife_offset);
            t2t_cursands_val = ReadMem(p, t2t_cursands_offset);

            t2t_animation_normal = ReadMem(p, t2t_animation_normal_offset);
            t2t_animation_ghost = ReadMem(p, t2t_animation_ghost_offset);
            t2t_state_normal = ReadMem(p, t2t_state_normal_offset);
            t2t_state_ghost = ReadMem(p, t2t_state_ghost_offset);
        }

        public void T2TUpdateInfo(Process p)
        {
            //read values again
            t2t_flymode_val = ReadMem(p, t2t_flymode_offset);
            t2t_state_val = ReadMem(p, t2t_state_offset);
            t2t_collision_val = ReadMem(p, t2t_collision_offset);
            t2t_animation_val = ReadMem(p, t2t_animation_offset);

            //update data in window
            label_pointer.Text = "Pointer value: various pointers";
            label_ghostmode.Text = "Ghostmode value: " + t2t_flymode_val.ToString("X8").Replace("-", "");
            label_movement.Text = "State value: " + t2t_state_val.ToString("X8").Replace("-", "");
            label_collision.Text = "Collision value: " + t2t_collision_val.ToString("X8").Replace("-", "");
            label_animation.Text = "Animation value: " + t2t_animation_val.ToString("X8").Replace("-", "");
            richTextBox_log.Text += "done!" + "\n";
        }
    }
}
