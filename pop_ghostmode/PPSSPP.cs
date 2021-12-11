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
        int ppsspp_load_offset = 0;
        int ppsspp_flymode_offset = 0;
        int ppsspp_state_offset = 0;
        int ppsspp_collision_offset = 0;
        int ppsspp_animation_offset = 0;
        int ppsspp_curlife_offset = 0;
        int ppsspp_maxlife_offset = 0;
        int ppsspp_cursands_offset = 0;
        int ppsspp_damage_offset = 0;

        uint ppsspp_state_val = 0;
        uint ppsspp_flymode_val = 0;
        uint ppsspp_collision_val = 0;
        uint ppsspp_animation_val = 0;
        uint ppsspp_maxlife_val = 0;

        uint ppsspp_state_ghost = 0;
        uint ppsspp_state_normal = 0;
        uint ppsspp_animation_ghost = 0;
        uint ppsspp_animation_normal = 0;
        uint ppsspp_damage_enabled = 3;
        uint ppsspp_damage_disabled = 1;
        uint ppsspp_flymode_enabled = 0x80C61044;
        uint ppsspp_flymode_disabled = 0xC0421044;
        uint ppsspp_collision_enabled = 0xFFFFF;
        uint ppsspp_collision_disabled = 0xF0000;

        int ppsspp_weapon_offset = 0;
        int ppsspp_weapon_collision_offset = 0;
        uint[] ppsspp_primary_weapon;
        string ppsspp_game_version = "";

        public void PatchPPSSPPProcess()
        {
            Process[] pname = Process.GetProcessesByName("ppssppwindows");
            if (pname.Length > 0)
            {
                var p = Process.GetProcessesByName("ppssppwindows").FirstOrDefault();

                
                
                //retrietving ELF load address
                ppsspp_load_offset = ReadMemPointer(p, 0xA3A9DC, 0x3C, 0x84, 0x0);
                //richTextBox_log.Text += "ELF load address: " + ppsspp_load_offset.ToString("X8").Replace("-", "") + "\n";

                int[] ppsspp_game_version_offset = new int[2];
                ppsspp_game_version_offset[0] = 0x49C3B0; //Revelations release version
                ppsspp_game_version_offset[1] = 0x4D2170; //Rival Swords release version
                

                //check game version
                for (int i = 0; i < ppsspp_game_version_offset.Length; i++)
                {
                    //skip "Build time:" text, +11 to offset
                    string elf_version = ReadMemString(p, ppsspp_load_offset + ppsspp_game_version_offset[i] + 11);

                    if (elf_version.Contains("Nov  9 2005 10:39:35"))
                    {
                        //richTextBox_log.Text += "Detected Revelations release version!" + "\n";
                        ppsspp_game_version = "revelations";
                    }
                    else if (elf_version.Contains("Feb  9 2007 14:26:53"))
                    {
                        //richTextBox_log.Text += "Detected Rival Swords release version!" + "\n";
                        ppsspp_game_version = "rival_swords";
                    }
                }

                PPSSPPGameOffsets(p);

                if (command == "patch_mode")
                {
                    PPSSPPPatchMode(p);
                }
                else if (command == "patch_weapon")
                {
                    PPSSPPPatchWeapon(p);
                }

                PPSSPPUpdateInfo(p);
            }
        }

        public void PPSSPPGameOffsets(Process p)
        {
            if (ppsspp_game_version == "revelations")
            {
                ppsspp_flymode_offset = ppsspp_load_offset + 0x9A64F0;
                ppsspp_state_offset = ppsspp_load_offset + 0x9AA500;
                ppsspp_collision_offset = ppsspp_load_offset + 0x9AA550;
                ppsspp_animation_offset = ppsspp_load_offset + 0x9A803C;
                ppsspp_curlife_offset = ppsspp_load_offset + 0xC206A4;
                ppsspp_maxlife_offset = ppsspp_load_offset + 0xC21940;
                ppsspp_cursands_offset = ppsspp_load_offset + 0xC20F44;
                ppsspp_damage_offset = ppsspp_load_offset + 0xC20AF4;

                ppsspp_state_ghost = 0x93E5B80;
                ppsspp_state_normal = 0x93E58B0;
                ppsspp_animation_ghost = 0x91B00F0;
                ppsspp_animation_normal = 0x91AF270;

                ppsspp_weapon_offset = ppsspp_load_offset + 0xC218B0;
                ppsspp_weapon_collision_offset = ppsspp_weapon_offset + 0x9C;
                ppsspp_primary_weapon = new uint[8];
                ppsspp_primary_weapon[0] = ReadMem(p, ppsspp_weapon_offset + 0x5c); //eagle sword
                ppsspp_primary_weapon[1] = ReadMem(p, ppsspp_weapon_offset + 0x60); //wooden stick
                ppsspp_primary_weapon[2] = ReadMem(p, ppsspp_weapon_offset + 0x64); //spider sword
                ppsspp_primary_weapon[3] = ReadMem(p, ppsspp_weapon_offset + 0x68); //serpent sword
                ppsspp_primary_weapon[4] = ReadMem(p, ppsspp_weapon_offset + 0x6c); //lion sword
                ppsspp_primary_weapon[5] = ReadMem(p, ppsspp_weapon_offset + 0x70); //lion sword (broken)
                ppsspp_primary_weapon[6] = ReadMem(p, ppsspp_weapon_offset + 0x74); //scorpion sword
                ppsspp_primary_weapon[7] = ReadMem(p, ppsspp_weapon_offset + 0x78); //water sword
            }
            else if (ppsspp_game_version == "rival_swords")
            {
                ppsspp_flymode_offset = ppsspp_load_offset + 0xC302D0;
                ppsspp_state_offset = ppsspp_load_offset + 0xC32F00;
                ppsspp_collision_offset = ppsspp_load_offset + 0xC32F50;
                ppsspp_animation_offset = ppsspp_load_offset + 0xC3011C;
                ppsspp_curlife_offset = ppsspp_load_offset + 0x9AC1A0;
                ppsspp_maxlife_offset = ppsspp_load_offset + 0x9ADAF0;
                ppsspp_cursands_offset = ppsspp_load_offset + 0x9ACBF8;
                ppsspp_damage_offset = ppsspp_load_offset + 0x9AC6E0;

                ppsspp_state_ghost = 0x9587F20;
                ppsspp_state_normal = 0x9587140;
                ppsspp_animation_ghost = 0x9437E20;
                ppsspp_animation_normal = 0x94371B0;

                ppsspp_weapon_offset = ppsspp_load_offset + 0x9AC888;
                ppsspp_weapon_collision_offset = ppsspp_weapon_offset + 0x1278;
                ppsspp_primary_weapon = new uint[3];
                ppsspp_primary_weapon[0] = ReadMem(p, ppsspp_weapon_offset + 0x11FC); //knife
                ppsspp_primary_weapon[1] = ReadMem(p, ppsspp_weapon_offset + 0x1200); //dagger
                ppsspp_primary_weapon[2] = ReadMem(p, ppsspp_weapon_offset + 0x1204); //sword
            }
        }

        public void PPSSPPPatchMode(Process p)
        {
            ppsspp_maxlife_val = ReadMem(p, ppsspp_maxlife_offset);
            ppsspp_collision_val = ReadMem(p, ppsspp_collision_offset);

            if (ppsspp_collision_val != ppsspp_collision_disabled)
            {
                //writing proper state value for ghostmode
                WriteMem32(p, ppsspp_state_offset, ppsspp_state_ghost);

                //disable collision
                WriteMem32(p, ppsspp_collision_offset, ppsspp_collision_disabled);

                //writing ghost mode value, because game tries to restore normal one
                for (int i = 0; i < 5; i++)
                {
                    //writing flymode value - 0x80C61044
                    WriteMem32(p, ppsspp_flymode_offset, ppsspp_flymode_enabled);
                    Thread.Sleep(20);
                }

                //writing proper animation value
                WriteMem32(p, ppsspp_animation_offset, ppsspp_animation_ghost);

                //disable damage
                WriteMem32(p, ppsspp_damage_offset, ppsspp_damage_disabled);

                //restore life
                WriteMem32(p, ppsspp_curlife_offset, ppsspp_maxlife_val);

                //restore sands
                WriteMem32(p, ppsspp_cursands_offset, 6);
            }
            else if (ppsspp_collision_val == ppsspp_collision_disabled)
            {
                //writing normal mode value - 0xC0421044
                WriteMem32(p, ppsspp_flymode_offset, ppsspp_flymode_disabled);

                //writing normal state
                WriteMem32(p, ppsspp_state_offset, ppsspp_state_normal);

                //writing proper animation value
                WriteMem32(p, ppsspp_animation_offset, ppsspp_animation_normal);

                //enable damage
                WriteMem32(p, ppsspp_damage_offset, ppsspp_damage_enabled);

                //enable collision
                WriteMem32(p, ppsspp_collision_offset, ppsspp_collision_enabled);
            }
        }

        public void PPSSPPPatchWeapon(Process p)
        {
            //read current weapon value
            uint ppsspp_weapon_val = ReadMem(p, ppsspp_weapon_offset);

            //switch to next weapon
            for (int i = 0; i < ppsspp_primary_weapon.Length; i++)
            {
                if (ppsspp_primary_weapon[i] == ppsspp_weapon_val && i < (ppsspp_primary_weapon.Length - 1))
                {
                    WriteMem32(p, ppsspp_weapon_offset, ppsspp_primary_weapon[i + 1]);
                    WriteMem32(p, ppsspp_weapon_collision_offset, ppsspp_primary_weapon[i + 1]);
                }
                else if (ppsspp_primary_weapon[i] == ppsspp_weapon_val && i == (ppsspp_primary_weapon.Length - 1))
                {
                    WriteMem32(p, ppsspp_weapon_offset, ppsspp_primary_weapon[0]);
                    WriteMem32(p, ppsspp_weapon_collision_offset, ppsspp_primary_weapon[0]);
                }
            }
        }
        public void PPSSPPUpdateInfo(Process p)
        {
            //read values again
            ppsspp_flymode_val = ReadMem(p, ppsspp_flymode_offset);
            ppsspp_state_val = ReadMem(p, ppsspp_state_offset);
            ppsspp_collision_val = ReadMem(p, ppsspp_collision_offset);
            ppsspp_animation_val = ReadMem(p, ppsspp_animation_offset);

            //update data in window
            label_pointer.Text = "Pointer value: " + ppsspp_load_offset.ToString("X8").Replace("-", "");
            label_ghostmode.Text = "Ghostmode value: " + ppsspp_flymode_val.ToString("X8").Replace("-", "");
            label_movement.Text = "State value: " + ppsspp_state_val.ToString("X8").Replace("-", "");
            label_collision.Text = "Collision value: " + ppsspp_collision_val.ToString("X8").Replace("-", "");
            label_animation.Text = "Animation value: " + ppsspp_animation_val.ToString("X8").Replace("-", "");
            richTextBox_log.Text += "done!" + "\n";
        }
    }
}
