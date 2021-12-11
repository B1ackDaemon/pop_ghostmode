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
        int ww_pointer;
        int ww_state_pointer;
        int ww_flymode_pointer;
        int ww_curlife_pointer;

        int ww_collision_offset = 0;
        int ww_animation_offset = 0;
        int ww_flymode_offset = 0;
        int ww_state_offset = 0;
        int ww_curlife_offset = 0;
        int ww_maxlife_offset = 0;
        int ww_cursands_offset = 0;
        int ww_damage_offset = 0;

        int ww_animation_pointer = 0;
        int ww_animation_normal_offset = 0;
        int ww_animation_ghost_offset = 0;
        int ww_state_normal_offset = 0;
        int ww_state_ghost_offset = 0;

        uint ww_flymode_val;
        uint ww_state_val;
        uint ww_collision_val;
        uint ww_animation_val;
        uint ww_curlife_val;
        uint ww_maxlife_val;
        uint ww_cursands_val;

        uint ww_state_ghost = 0;
        uint ww_animation_ghost = 0;

        uint ww_damage_enabled = 3;
        uint ww_damage_disabled = 1;
        uint ww_flymode_enabled = 0x80C61044;
        uint ww_flymode_disabled = 0xC0421044;
        uint ww_collision_enabled = 0xFFFFF;
        uint ww_collision_disabled = 0xF0000;

        int ww_weapon_offset = 0;
        int ww_weapon_collision_offset = 0;
        uint[] ww_primary_weapon;
        string ww_game_version = "";
        public void PatchWWProcess()
        {
            Process[] pname = Process.GetProcessesByName("pop2");
            if (pname.Length > 0)
            {
                var p = Process.GetProcessesByName("pop2").FirstOrDefault();

                

                int[] ww_game_version_offset = new int[3];
                ww_game_version_offset[0] = 0x92394C; //WW release version
                ww_game_version_offset[1] = 0x8AE028; //WW demo 29/09/04 version
                ww_game_version_offset[2] = 0x9229E4; //WW demo 20/01/05 version
                

                //check game version
                for (int i = 0; i < ww_game_version_offset.Length; i++)
                {
                    string exe_version = ReadMemString(p, ww_game_version_offset[i]);
                    if (exe_version.Contains("Nov  5 2004-17:15:14"))
                    {
                        //richTextBox_log.Text += "Detected WW release version!" + "\n";
                        ww_game_version = "ww_release";
                    }
                    else if (exe_version.Contains("Sep 29 2004-20:35:42"))
                    {
                        //richTextBox_log.Text += "Detected WW demo 29/09/04 version!" + "\n";
                        ww_game_version = "ww_demo_sep29";
                    }
                    else if (exe_version.Contains("Jan 20 2005-11:05:10"))
                    {
                        //richTextBox_log.Text += "Detected WW demo 20/01/05 version!" + "\n";
                        ww_game_version = "ww_demo_jan20";
                    }
                }

                WWGameOffsets(p);

                if (command == "patch_mode")
                {
                    WWPatchMode(p);
                }
                else if (command == "patch_weapon")
                {
                    WWPatchWeapon(p);
                }


                WWUpdateInfo(p);

                
            }
        }

        public void WWGameOffsets(Process p)
        {
            if (ww_game_version == "ww_release")
            {
                ww_pointer = 0x90C414;
                ww_state_pointer = 0x523710;
                ww_flymode_pointer = 0x523710;
                ww_curlife_pointer = 0x90C414;

                ww_animation_pointer = 0x90C414;

                //read pointer
                ww_collision_offset = ReadMemPointer(p, ww_pointer, 0x70);

                //read flymode pointer
                ww_flymode_offset = ReadMemPointer(p, ww_flymode_pointer, 0x30, 0x14, 0x4, 0x0);
                ww_animation_offset = ReadMemPointer(p, ww_flymode_pointer, 0x30, 0x14, 0x4, -0x16F4);

                //read state pointer
                ww_state_offset = ReadMemPointer(p, ww_state_pointer, 0x30, 0x18, 0x4, 0x20);

                //read curlife pointer
                ww_curlife_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1F4);
                ww_maxlife_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1490);
                ww_cursands_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0xA94);

                //read animation and state value pointers
                ww_animation_ghost_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x5C);
                ww_animation_normal_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0xC);
                //ww_state_ghost_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x10FE8);
                //ww_state_normal_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x10FC4);
                ww_state_ghost_offset = ReadMemPointer(p, 0x967F20, 0x4, 0x38);
                ww_state_normal_offset = ReadMemPointer(p, 0x967F20, 0x4, 0x14);

                //read damage pointer
                ww_damage_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x644);

                ww_weapon_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1400);
                ww_weapon_collision_offset = ww_weapon_offset + 0x9C;
                ww_primary_weapon = new uint[8];
                ww_primary_weapon[0] = ReadMem(p, ww_weapon_offset + 0x5c); //eagle sword
                ww_primary_weapon[1] = ReadMem(p, ww_weapon_offset + 0x60); //wooden stick
                ww_primary_weapon[2] = ReadMem(p, ww_weapon_offset + 0x64); //spider sword
                ww_primary_weapon[3] = ReadMem(p, ww_weapon_offset + 0x68); //serpent sword
                ww_primary_weapon[4] = ReadMem(p, ww_weapon_offset + 0x6c); //lion sword
                ww_primary_weapon[5] = ReadMem(p, ww_weapon_offset + 0x70); //lion sword (broken)
                ww_primary_weapon[6] = ReadMem(p, ww_weapon_offset + 0x74); //scorpion sword
                ww_primary_weapon[7] = ReadMem(p, ww_weapon_offset + 0x78); //water sword
            }
            else if (ww_game_version == "ww_demo_sep29")
            {
                //ww_state_pointer = 0x4CA1B0;
                ww_state_pointer = 0x815F88;
                ww_curlife_pointer = 0x815F88;

                ww_animation_pointer = 0x815F88;

                //read state pointer
                /*ww_state_offset = ReadMemPointer(p, ww_state_pointer, 0x14c, 0x15c, 0x104, 0x20);
                ww_collision_offset = ReadMemPointer(p, ww_state_pointer, 0x14c, 0x15c, 0x104, 0xDC20);
                ww_flymode_offset = ReadMemPointer(p, ww_state_pointer, 0x14c, 0x15c, 0x104, -0x60);
                ww_animation_offset = ReadMemPointer(p, ww_state_pointer, 0x14c, 0x15c, 0x104, -0x15D64);*/
                ww_state_offset = ReadMemPointer(p, ww_state_pointer, 0xc, 0x94, 0x14, 0x24, 0x80);
                ww_collision_offset = ReadMemPointer(p, ww_state_pointer, 0xc, 0x94, 0x14, 0x24, 0xDC80);
                ww_flymode_offset = ReadMemPointer(p, ww_state_pointer, 0xc, 0x94, 0x14, 0x24, 0x0);
                ww_animation_offset = ReadMemPointer(p, ww_state_pointer, 0xc, 0x94, 0x14, 0x24, -0x15D04);

                ww_animation_ghost_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x5C);
                ww_animation_normal_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0xC);
                ww_state_ghost_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x19838);
                ww_state_normal_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x19814);

                //read damage pointer
                ww_damage_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x654);

                //read curlife pointer
                ww_curlife_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x204);
                ww_maxlife_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x18E4);
                ww_cursands_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0xA94);

                ww_weapon_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1860);
                ww_weapon_collision_offset = ww_weapon_offset + 0x9C;
                ww_primary_weapon = new uint[8];
                ww_primary_weapon[0] = ReadMem(p, ww_weapon_offset + 0x54); //eagle sword
                ww_primary_weapon[1] = ReadMem(p, ww_weapon_offset + 0x58); //wooden stick
                ww_primary_weapon[2] = ReadMem(p, ww_weapon_offset + 0x5c); //spider sword
                ww_primary_weapon[3] = ReadMem(p, ww_weapon_offset + 0x60); //serpent sword
                ww_primary_weapon[4] = ReadMem(p, ww_weapon_offset + 0x64); //lion sword
                ww_primary_weapon[5] = ReadMem(p, ww_weapon_offset + 0x68); //lion sword (broken)
                ww_primary_weapon[6] = ReadMem(p, ww_weapon_offset + 0x6c); //scorpion sword
                ww_primary_weapon[7] = ReadMem(p, ww_weapon_offset + 0x70); //water sword
            }
            else if (ww_game_version == "ww_demo_jan20")
            {
                ww_state_pointer = 0x965350;
                ww_curlife_pointer = 0x90B4AC;
                ww_animation_pointer = 0x90B4AC;

                //read state pointer
                ww_state_offset = ReadMemPointer(p, ww_state_pointer, 0x18, 0x4, 0x440);
                ww_flymode_offset = ReadMemPointer(p, ww_state_pointer, 0x18, 0x4, 0x3C0);
                ww_collision_offset = ReadMemPointer(p, ww_state_pointer, 0x18, 0x4, -0x13F0);
                ww_animation_offset = ReadMemPointer(p, ww_state_pointer, 0x18, 0x4, -0x6A64);

                //read state/animation value pointers
                ww_animation_ghost_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x5C);
                ww_animation_normal_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0xC);
                ww_state_ghost_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x10388);
                ww_state_normal_offset = ReadMemPointer(p, ww_animation_pointer, 0x14, 0, 0x30, 0x10364);

                //read damage pointer
                ww_damage_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x644);

                //read curlife pointer
                ww_curlife_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1F4);
                ww_maxlife_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1490);
                ww_cursands_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0xA94);

                ww_weapon_offset = ReadMemPointer(p, ww_curlife_pointer, 0x18, 0x4, 0x48, 0x1400);
                ww_weapon_collision_offset = ww_weapon_offset + 0x9C;
                ww_primary_weapon = new uint[8];
                ww_primary_weapon[0] = ReadMem(p, ww_weapon_offset + 0x5c); //eagle sword
                ww_primary_weapon[1] = ReadMem(p, ww_weapon_offset + 0x60); //wooden stick
                ww_primary_weapon[2] = ReadMem(p, ww_weapon_offset + 0x64); //spider sword
                ww_primary_weapon[3] = ReadMem(p, ww_weapon_offset + 0x68); //serpent sword
                ww_primary_weapon[4] = ReadMem(p, ww_weapon_offset + 0x6c); //lion sword
                ww_primary_weapon[5] = ReadMem(p, ww_weapon_offset + 0x70); //lion sword (broken)
                ww_primary_weapon[6] = ReadMem(p, ww_weapon_offset + 0x74); //scorpion sword
                ww_primary_weapon[7] = ReadMem(p, ww_weapon_offset + 0x78); //water sword
            }

            //read values
            ww_flymode_val = ReadMem(p, ww_flymode_offset);
            ww_state_val = ReadMem(p, ww_state_offset);
            ww_collision_val = ReadMem(p, ww_collision_offset);
            ww_animation_val = ReadMem(p, ww_animation_offset);
            ww_curlife_val = ReadMem(p, ww_curlife_offset);
            ww_maxlife_val = ReadMem(p, ww_maxlife_offset);
            ww_cursands_val = ReadMem(p, ww_cursands_offset);

            ww_animation_normal = ReadMem(p, ww_animation_normal_offset);
            ww_animation_ghost = ReadMem(p, ww_animation_ghost_offset);
            ww_state_normal = ReadMem(p, ww_state_normal_offset);
            ww_state_ghost = ReadMem(p, ww_state_ghost_offset);
        }

        public void WWPatchMode(Process p)
        {
            if (ww_collision_val != ww_collision_disabled)
            {
                /*if (ww_game_version == "ww_release" || ww_game_version == "ww_demo_jan20")
                {
                    //writing proper state value for ghostmode
                    if (ww_state_normal == 0)
                    {
                        ww_state_normal = ww_state_val;
                    }
                    if (ww_animation_normal == 0)
                    {
                        ww_animation_normal = ww_animation_val;
                    }
                    ww_state_ghost = ww_state_normal + 0x120;
                    WriteMem16(p, ww_state_offset, (ushort)ww_state_ghost);
                    ww_animation_ghost = ww_animation_normal + 0x500;
                    WriteMem16(p, ww_animation_offset, (ushort)ww_animation_ghost);
                }*/
                //writing proper state value for ghostmode
                WriteMem32(p, ww_state_offset, ww_state_ghost);

                //writing ghost animation
                WriteMem32(p, ww_animation_offset, ww_animation_ghost);

                //disable damage
                WriteMem32(p, ww_damage_offset, ww_damage_disabled);

                //disable collision
                WriteMem32(p, ww_collision_offset, ww_collision_disabled);

                //writing ghost mode value, because game tries to restore normal one
                for (int i = 0; i < 5; i++)
                {
                    //writing flymode value - 0x80C61044
                    WriteMem32(p, ww_flymode_offset, ww_flymode_enabled);
                    Thread.Sleep(20);
                }

                //restore life
                WriteMem32(p, ww_curlife_offset, ww_maxlife_val);

                //restore sands
                WriteMem32(p, ww_cursands_offset, 6);
            }
            else if (ww_collision_val == ww_collision_disabled)
            {
                /*if (ww_game_version == "ww_release" || ww_game_version == "ww_demo_jan20")
                {
                    //writing normal state
                    if (ww_state_normal == 0)
                    {
                        ww_state_normal = ww_state_val - 0x120;
                    }
                    if (ww_animation_normal == 0)
                    {
                        ww_animation_normal = ww_animation_val - 0x500;
                    }
                    WriteMem16(p, ww_state_offset, (ushort)ww_state_normal);
                    WriteMem16(p, ww_animation_offset, (ushort)ww_animation_normal);
                }*/
                //writing normal state
                WriteMem32(p, ww_state_offset, ww_state_normal);

                //writing normal animation
                WriteMem32(p, ww_animation_offset, ww_animation_normal);

                //writing normal mode value - 0xC0421044
                WriteMem32(p, ww_flymode_offset, ww_flymode_disabled);

                //enable damage
                WriteMem32(p, ww_damage_offset, ww_damage_enabled);

                //enable collision
                WriteMem32(p, ww_collision_offset, ww_collision_enabled);
            }
        }

        public void WWPatchWeapon(Process p)
        {
            //read current weapon value
            uint ww_weapon_val = ReadMem(p, ww_weapon_offset);

            //switch to next weapon
            for (int i = 0; i < ww_primary_weapon.Length; i++)
            {
                if (ww_primary_weapon[i] == ww_weapon_val && i < (ww_primary_weapon.Length - 1))
                {
                    WriteMem32(p, ww_weapon_offset, ww_primary_weapon[i + 1]);
                    WriteMem32(p, ww_weapon_collision_offset, ww_primary_weapon[i + 1]);
                }
                else if (ww_primary_weapon[i] == ww_weapon_val && i == (ww_primary_weapon.Length - 1))
                {
                    WriteMem32(p, ww_weapon_offset, ww_primary_weapon[0]);
                    WriteMem32(p, ww_weapon_collision_offset, ww_primary_weapon[0]);
                }
            }
        }

        public void WWUpdateInfo(Process p)
        {
            //read values again
            ww_flymode_val = ReadMem(p, ww_flymode_offset);
            ww_state_val = ReadMem(p, ww_state_offset);
            ww_collision_val = ReadMem(p, ww_collision_offset);
            ww_animation_val = ReadMem(p, ww_animation_offset);

            //update data in window
            label_pointer.Text = "Pointer value: various pointers";
            label_ghostmode.Text = "Ghostmode value: " + ww_flymode_val.ToString("X8").Replace("-", "");
            label_movement.Text = "State value: " + ww_state_val.ToString("X8").Replace("-", "");
            label_collision.Text = "Collision value: " + ww_collision_val.ToString("X8").Replace("-", "");
            label_animation.Text = "Animation value: " + ww_animation_val.ToString("X8").Replace("-", "");
            richTextBox_log.Text += "done!" + "\n";
        }
    }
}
