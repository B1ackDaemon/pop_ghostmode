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
        int pcsx2_flymode_offset = 0;
        int pcsx2_state_offset = 0;
        int pcsx2_collision_offset = 0;
        int pcsx2_animation_offset = 0;
        int pcsx2_damage_offset = 0;
        int pcsx2_curlife_offset = 0;
        int pcsx2_maxlife_offset = 0;
        int pcsx2_cursands_offset = 0;

        uint pcsx2_state_val = 0;
        uint pcsx2_flymode_val = 0;
        uint pcsx2_collision_val = 0;
        uint pcsx2_animation_val = 0;
        uint pcsx2_maxlife_val = 0;

        uint pcsx2_state_ghost = 0;
        uint pcsx2_state_normal = 0;
        uint pcsx2_animation_ghost = 0;
        uint pcsx2_animation_normal = 0;
        uint pcsx2_damage_enabled = 3;
        uint pcsx2_damage_disabled = 1;
        uint pcsx2_flymode_enabled = 0x80C61044;
        uint pcsx2_flymode_disabled = 0xC0421044;
        uint pcsx2_collision_enabled = 0xFFFFF;
        uint pcsx2_collision_disabled = 0xF0000;

        int pcsx2_weapon_offset = 0;
        int pcsx2_weapon_collision_offset = 0;
        uint[] pcsx2_primary_weapon;

        string pcsx2_game_version = "";

        public void PatchPCSX2Process()
        {
            Process[] pname = Process.GetProcessesByName("pcsx2");
            if (pname.Length > 0)
            {
                var p = Process.GetProcessesByName("pcsx2").FirstOrDefault();

                int[] pcsx2_game_version_offset = new int[5];
                pcsx2_game_version_offset[0] = 0x2079F37B; //WW release version
                pcsx2_game_version_offset[1] = 0x2079852B; //WW beta 25/10/04 version
                pcsx2_game_version_offset[2] = 0x2079880B; //WW beta 27/10/04 version
                pcsx2_game_version_offset[3] = 0x207CD07B; //WW demo 29/08/04 version
                pcsx2_game_version_offset[4] = 0x20A70450; //T2T release version
                
                //check game version
                for (int i = 0; i < pcsx2_game_version_offset.Length; i++)
                {
                    string elf_version = ReadMemString(p, pcsx2_game_version_offset[i]);

                    if (elf_version.Contains("Nov 12 2004 20:05:45"))
                    {
                        //richTextBox_log.Text += "Detected WW release version!" + "\n";
                        pcsx2_game_version = "ww_release";
                    }
                    else if (elf_version.Contains("Oct 25 2004 11:26:10"))
                    {
                        //richTextBox_log.Text += "Detected WW beta 25/10/04 version!" + "\n";
                        pcsx2_game_version = "ww_beta_oct25";
                    }
                    else if (elf_version.Contains("Oct 27 2004 07:05:07"))
                    {
                        //richTextBox_log.Text += "Detected WW beta 27/10/04 version!" + "\n";
                        pcsx2_game_version = "ww_beta_oct27";
                    }
                    else if (elf_version.Contains("Aug 29 2004 16:03:18"))
                    {
                        //richTextBox_log.Text += "Detected WW demo 29/08/04 version!" + "\n";
                        pcsx2_game_version = "ww_demo_aug29";
                    }
                    else if (elf_version.Contains("Prince of Persia\nT2T"))
                    {
                        //richTextBox_log.Text += "Detected T2T release version!" + "\n";
                        pcsx2_game_version = "t2t_release";
                    }
                }

                PCSX2GameOffsets(p);

                if (command == "patch_mode")
                {
                    PCSX2PatchMode(p);
                }
                else if (command == "patch_weapon")
                {
                    PCSX2PatchWeapon(p);
                }

                PCSX2UpdateInfo(p);
            }
        }
        public void PCSX2GameOffsets(Process p)
        {
            if (pcsx2_game_version == "ww_release")
            {
                pcsx2_flymode_offset = 0x210BF690;
                pcsx2_state_offset = 0x210C1F50;
                pcsx2_collision_offset = 0x210C1FA0;
                pcsx2_animation_offset = 0x210BF4BC;
                pcsx2_curlife_offset = 0x21365814;
                pcsx2_maxlife_offset = 0x21366AB0;
                pcsx2_cursands_offset = 0x213660B4;
                pcsx2_damage_offset = 0x21365C64;

                pcsx2_state_ghost = 0x12FB0D0;
                pcsx2_state_normal = 0x12FAE00;
                pcsx2_animation_ghost = 0x10C3AD0;
                pcsx2_animation_normal = 0x10C2CA0;

                pcsx2_weapon_offset = 0x21366A20;
                pcsx2_weapon_collision_offset = 0x21366ABC;
                pcsx2_primary_weapon = new uint[8];
                pcsx2_primary_weapon[0] = ReadMem(p, pcsx2_weapon_offset + 0x5c); //eagle sword
                pcsx2_primary_weapon[1] = ReadMem(p, pcsx2_weapon_offset + 0x60); //wooden stick
                pcsx2_primary_weapon[2] = ReadMem(p, pcsx2_weapon_offset + 0x64); //spider sword
                pcsx2_primary_weapon[3] = ReadMem(p, pcsx2_weapon_offset + 0x68); //serpent sword
                pcsx2_primary_weapon[4] = ReadMem(p, pcsx2_weapon_offset + 0x6c); //lion sword
                pcsx2_primary_weapon[5] = ReadMem(p, pcsx2_weapon_offset + 0x70); //lion sword (broken)
                pcsx2_primary_weapon[6] = ReadMem(p, pcsx2_weapon_offset + 0x74); //scorpion sword
                pcsx2_primary_weapon[7] = ReadMem(p, pcsx2_weapon_offset + 0x78); //water sword
            }
            else if (pcsx2_game_version == "ww_beta_oct25")
            {
                pcsx2_flymode_offset = 0x2151ED10;
                pcsx2_state_offset = 0x215215E0;
                pcsx2_collision_offset = 0x21521630;
                pcsx2_animation_offset = 0x2151EB3C;
                pcsx2_curlife_offset = 0x217C5594;
                pcsx2_maxlife_offset = 0x217C6824;
                pcsx2_cursands_offset = 0x217C5E14;
                pcsx2_damage_offset = 0x217C59D0;

                pcsx2_state_ghost = 0x175A100;
                pcsx2_state_normal = 0x1759E30;
                pcsx2_animation_ghost = 0x15231D0;
                pcsx2_animation_normal = 0x1522350;

                pcsx2_weapon_offset = 0x217C6794;
                pcsx2_weapon_collision_offset = 0x217C683C;
                pcsx2_primary_weapon = new uint[8];
                pcsx2_primary_weapon[0] = ReadMem(p, pcsx2_weapon_offset + 0x5c); //eagle sword
                pcsx2_primary_weapon[1] = ReadMem(p, pcsx2_weapon_offset + 0x60); //wooden stick
                pcsx2_primary_weapon[2] = ReadMem(p, pcsx2_weapon_offset + 0x64); //spider sword
                pcsx2_primary_weapon[3] = ReadMem(p, pcsx2_weapon_offset + 0x68); //serpent sword
                pcsx2_primary_weapon[4] = ReadMem(p, pcsx2_weapon_offset + 0x6c); //lion sword
                pcsx2_primary_weapon[5] = ReadMem(p, pcsx2_weapon_offset + 0x70); //lion sword (broken)
                pcsx2_primary_weapon[6] = ReadMem(p, pcsx2_weapon_offset + 0x74); //scorpion sword
                pcsx2_primary_weapon[7] = ReadMem(p, pcsx2_weapon_offset + 0x78); //water sword
            }
            else if (pcsx2_game_version == "ww_beta_oct27")
            {
                pcsx2_flymode_offset = 0x2152FB90;
                pcsx2_state_offset = 0x21532460;
                pcsx2_collision_offset = 0x215324B0;
                pcsx2_animation_offset = 0x2152F9BC;
                pcsx2_curlife_offset = 0x217D5614;
                pcsx2_maxlife_offset = 0x217D68B0;
                pcsx2_cursands_offset = 0x217D5EB4;
                pcsx2_damage_offset = 0x217D5A64;

                pcsx2_state_ghost = 0x176AF10;
                pcsx2_state_normal = 0x176AC40;
                pcsx2_animation_ghost = 0x1533FE0;
                pcsx2_animation_normal = 0x15331B0;

                pcsx2_weapon_offset = 0x217D681C;
                pcsx2_weapon_collision_offset = 0x217D68BC;
                pcsx2_primary_weapon = new uint[8];
                pcsx2_primary_weapon[0] = ReadMem(p, pcsx2_weapon_offset + 0x5c); //eagle sword
                pcsx2_primary_weapon[1] = ReadMem(p, pcsx2_weapon_offset + 0x60); //wooden stick
                pcsx2_primary_weapon[2] = ReadMem(p, pcsx2_weapon_offset + 0x64); //spider sword
                pcsx2_primary_weapon[3] = ReadMem(p, pcsx2_weapon_offset + 0x68); //serpent sword
                pcsx2_primary_weapon[4] = ReadMem(p, pcsx2_weapon_offset + 0x6c); //lion sword
                pcsx2_primary_weapon[5] = ReadMem(p, pcsx2_weapon_offset + 0x70); //lion sword (broken)
                pcsx2_primary_weapon[6] = ReadMem(p, pcsx2_weapon_offset + 0x74); //scorpion sword
                pcsx2_primary_weapon[7] = ReadMem(p, pcsx2_weapon_offset + 0x78); //water sword
            }
            else if (pcsx2_game_version == "ww_demo_aug29")
            {
                pcsx2_flymode_offset = 0x2105B690;
                pcsx2_state_offset = 0x2105DA10;
                pcsx2_collision_offset = 0x2105DA70;
                pcsx2_animation_offset = 0x2105B4BC;
                pcsx2_curlife_offset = 0x2132D784;
                pcsx2_maxlife_offset = 0x2132EE64;
                pcsx2_cursands_offset = 0x2132E014;
                pcsx2_damage_offset = 0x2132DBD4;

                pcsx2_state_ghost = 0x12BE540;
                pcsx2_state_normal = 0x12BE250;
                pcsx2_animation_ghost = 0x105F590;
                pcsx2_animation_normal = 0x105E7A0;

                pcsx2_weapon_offset = 0x2132EDE0;
                pcsx2_weapon_collision_offset = 0x2132EE7C;
                pcsx2_primary_weapon = new uint[8];
                pcsx2_primary_weapon[0] = ReadMem(p, pcsx2_weapon_offset + 0x54); //eagle sword
                pcsx2_primary_weapon[1] = ReadMem(p, pcsx2_weapon_offset + 0x58); //wooden stick
                pcsx2_primary_weapon[2] = ReadMem(p, pcsx2_weapon_offset + 0x5c); //spider sword
                pcsx2_primary_weapon[3] = ReadMem(p, pcsx2_weapon_offset + 0x60); //serpent sword
                pcsx2_primary_weapon[4] = ReadMem(p, pcsx2_weapon_offset + 0x64); //lion sword
                pcsx2_primary_weapon[5] = ReadMem(p, pcsx2_weapon_offset + 0x68); //lion sword (broken)
                pcsx2_primary_weapon[6] = ReadMem(p, pcsx2_weapon_offset + 0x6c); //scorpion sword
                pcsx2_primary_weapon[7] = ReadMem(p, pcsx2_weapon_offset + 0x70); //water sword
            }
            else if (pcsx2_game_version == "t2t_release")
            {
                pcsx2_flymode_offset = 0x2107CF20;
                pcsx2_state_offset = 0x21080250;
                pcsx2_collision_offset = 0x210802A0;
                pcsx2_animation_offset = 0x2107CD4C;
                pcsx2_curlife_offset = 0x20E9DFF0;
                pcsx2_maxlife_offset = 0x20E9F93C;
                pcsx2_cursands_offset = 0x20E9EA48;
                pcsx2_damage_offset = 0x20E9E530;

                pcsx2_state_ghost = 0x119D910;
                pcsx2_state_normal = 0x119CC00;
                pcsx2_animation_ghost = 0x1081670;
                pcsx2_animation_normal = 0x1080530;

                pcsx2_weapon_offset = 0x20E9E6D8;
                pcsx2_weapon_collision_offset = 0x20E9F94C;
                pcsx2_primary_weapon = new uint[3];
                pcsx2_primary_weapon[0] = ReadMem(p, pcsx2_weapon_offset + 0x11F8); //knife
                pcsx2_primary_weapon[1] = ReadMem(p, pcsx2_weapon_offset + 0x11FC); //dagger
                pcsx2_primary_weapon[2] = ReadMem(p, pcsx2_weapon_offset + 0x1200); //sword
            }
        }
        public void PCSX2PatchMode(Process p)
        {
            //read collision value
            pcsx2_collision_val = ReadMem(p, pcsx2_collision_offset);

            //read max life value
            pcsx2_maxlife_val = ReadMem(p, pcsx2_maxlife_offset);

            if (pcsx2_collision_val != pcsx2_collision_disabled)
            {
                //writing proper state value for ghostmode
                WriteMem32(p, pcsx2_state_offset, pcsx2_state_ghost);

                //disable collision
                WriteMem32(p, pcsx2_collision_offset, pcsx2_collision_disabled);

                //writing ghost mode value, because game tries to restore normal one
                for (int i = 0; i < 5; i++)
                {
                    //writing flymode value - 0x80C61044
                    WriteMem32(p, pcsx2_flymode_offset, pcsx2_flymode_enabled);
                    Thread.Sleep(20);
                }

                //writing proper animation value
                WriteMem32(p, pcsx2_animation_offset, pcsx2_animation_ghost);

                //disable damage
                WriteMem32(p, pcsx2_damage_offset, pcsx2_damage_disabled);

                //restore life
                WriteMem32(p, pcsx2_curlife_offset, pcsx2_maxlife_val);

                //restore sands
                WriteMem32(p, pcsx2_cursands_offset, 6);
            }
            else if (pcsx2_collision_val == pcsx2_collision_disabled)
            {
                //writing proper state value for normalmode
                WriteMem32(p, pcsx2_state_offset, pcsx2_state_normal);

                //writing proper animation value
                WriteMem32(p, pcsx2_animation_offset, pcsx2_animation_normal);

                //enable damage
                WriteMem32(p, pcsx2_damage_offset, pcsx2_damage_enabled);

                //enable collision
                WriteMem32(p, pcsx2_collision_offset, pcsx2_collision_enabled);

                //writing normal value - 0xC0421044
                WriteMem32(p, pcsx2_flymode_offset, pcsx2_flymode_disabled);
            }
        }
        public void PCSX2PatchWeapon(Process p)
        {
            //read current weapon value
            uint pcsx2_weapon_val = ReadMem(p, pcsx2_weapon_offset);

            //switch to next weapon
            for (int i = 0; i < pcsx2_primary_weapon.Length; i++)
            {
                if (pcsx2_primary_weapon[i] == pcsx2_weapon_val && i < (pcsx2_primary_weapon.Length - 1))
                {
                    WriteMem32(p, pcsx2_weapon_offset, pcsx2_primary_weapon[i + 1]);
                    WriteMem32(p, pcsx2_weapon_collision_offset, pcsx2_primary_weapon[i + 1]);
                }
                else if (pcsx2_primary_weapon[i] == pcsx2_weapon_val && i == (pcsx2_primary_weapon.Length - 1))
                {
                    WriteMem32(p, pcsx2_weapon_offset, pcsx2_primary_weapon[0]);
                    WriteMem32(p, pcsx2_weapon_collision_offset, pcsx2_primary_weapon[0]);
                }
            }
        }
        public void PCSX2UpdateInfo(Process p)
        {
            //read values again
            pcsx2_flymode_val = ReadMem(p, pcsx2_flymode_offset);
            pcsx2_state_val = ReadMem(p, pcsx2_state_offset);
            pcsx2_collision_val = ReadMem(p, pcsx2_collision_offset);
            pcsx2_animation_val = ReadMem(p, pcsx2_animation_offset);

            //update data in window
            label_pointer.Text = "Pointer value: not required";
            label_ghostmode.Text = "Ghostmode value: " + pcsx2_flymode_val.ToString("X8").Replace("-", "");
            label_movement.Text = "State value: " + pcsx2_state_val.ToString("X8").Replace("-", "");
            label_collision.Text = "Collision value: " + pcsx2_collision_val.ToString("X8").Replace("-", "");
            label_animation.Text = "Animation value: " + pcsx2_animation_val.ToString("X8").Replace("-", "");
            richTextBox_log.Text += "done!" + "\n";
        }
    }
}
