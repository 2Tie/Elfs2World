using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Elf_s_World
{
    public partial class Form1 : Form
    {

        List<Tileset> tilesets;
        List<Map> maps;
        List<HiddenItem> hiddenItems;
        PokeTeam curTeam;
        //List<Palette> palettes;

        int[] mapTs = { 13, 12, 03, 08, 06, 11, 04, 05, 10, 02, 03, 02, 02, 09, 01 };

        List<string> pokeNames = new List<string>();
        List<string> trainerNames = new List<string>();

        List<int> groupStarts = new List<int>{ 0, 15, 37, 56, 70, 86, 100, 120, 128, 143, 156, 194, 196, 198, 204 };

        int maxmaps = 248;
        int maxtiles = 24;

        int curMap = 0;
        int prevMap = -1;
        bool viewEnts = false;

        int curTiles = 0;

        int selx = -1;
        int sely = -1;

        enum DEETS { NULL, MAP, WARP }
        DEETS dState;

        string file;
        bool loaded = false;

        bool viewTiles = false;

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Started";
            toolStripStatusLabel3.Text = "";
        }

        private void openROM(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog() { Filter = "Super Game Boy ROMs (*.sgb)|*.sgb" };
            if (dial.ShowDialog() != DialogResult.OK) return;

            file = dial.FileName;

            //reset spritesets
            SpriteSetsOW.init();

            BinaryReader bR = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read));

            bR.ReadBytes(308);
            string title = new string(bR.ReadChars(15));

            int gameid = -1;
            if (title == "POKEMON RED\0\0\0\0")
                gameid = 0;
            if (title == "POKEMON GREEN\0\0")
                gameid = 1;
            if (title == "POKEMON BLUE\0\0\0")
                gameid = 2;
            if (title == "POKEMON YELLOW\0")
            {
                gameid = 3;
                maxmaps = 249;
                maxtiles = 25;
            }
            else
            {
                maxmaps = 248;
                maxtiles = 24;
            }
            if (title == "POKEMON2GOLD\0\0\0" || title == "POKEMON2SILVER\0")
            {
                bR.BaseStream.Seek(0x1E5, SeekOrigin.Begin);
                byte debug = bR.ReadByte();
                if (debug == 0x9F)
                    gameid = 4;
                if (debug == 0xDB)
                {
                    gameid = 5;
                    title += "Debug ";
                }
                maxmaps = 0xE4;//228
                maxtiles = 28;
            }

            if (gameid == -1)
            {
                bR.Close();
                Application.Exit();//popup and return in the future
            }

            tilesets = new List<Tileset>();
            maps = new List<Map>();
            hiddenItems = new List<HiddenItem>();
            //palettes = new List<Palette>();
            PaletteList.reset();

            bR.BaseStream.Seek(0x14A, SeekOrigin.Begin);
            byte region = bR.ReadByte();
            Offsets.populate(region, gameid);

            toolStripStatusLabel1.Text = "Loaded " + title + ((region == 0) ? " JP" : " US");
            resetPanel();

            bR.BaseStream.Seek(Offsets.tilesetHeaderPointer, SeekOrigin.Begin);//tileset header pointers

            for (int i = 0; i < maxtiles; i++)
            {
                Tileset t = new Tileset();
                t.header.make(bR.ReadByte(), bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadByte(), bR.ReadByte(), bR.ReadByte(), bR.ReadByte());
                tilesets.Add(t);
            }

            //load palettes
            bR.BaseStream.Seek(Offsets.palletesPointer, SeekOrigin.Begin);
            for (int i = 0; i < 58; i++)//34?
            {
                /*Palette p = new Palette();
                p.load(bR.ReadBytes(8));
                palettes.Add(p);*/
                PaletteList.loadImmediate(bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadUInt16());
            }

            //set tileset palettes???
            //tilesets[0].loadPal(palettes[0]);
            //tilesets[17].loadPal(palettes[35]);

            for (int i = 0; i < maxtiles; i++)//load the tilesets
            {

                int bank = (tilesets[i].header.bankID - 1) * 0x4000;
                byte[] full;
                if (i < 9 || i > 24)
                {
                    bR.BaseStream.Seek(0x33C00, SeekOrigin.Begin);
                    byte[] top = bR.ReadBytes(0x20 * 0x10);
                    
                    bR.BaseStream.Seek(bank + tilesets[i].header.pTiles, SeekOrigin.Begin);
                    //bR.BaseStream.Seek(0x19800, SeekOrigin.Begin);
                    //int length = tilesets[i].header.pBlocks - tilesets[i].header.pTiles;
                    byte[] bottom = bR.ReadBytes(0x40 * 0x10);

                    full = new byte[top.Length + bottom.Length];
                    System.Buffer.BlockCopy(top, 0, full, 0, top.Length);
                    System.Buffer.BlockCopy(bottom, 0, full, top.Length, bottom.Length);
                }
                else
                {
                    bR.BaseStream.Seek(bank + tilesets[i].header.pTiles, SeekOrigin.Begin);
                    full = bR.ReadBytes(0x60 * 0x10);
                }
                tilesets[i].loadTiles(full);
                bR.BaseStream.Seek(bank + tilesets[i].header.pBlocks, SeekOrigin.Begin);
                //bR.BaseStream.Seek(0x19C00, SeekOrigin.Begin);
                tilesets[i].loadBlocks(bR.ReadBytes(128 * 16));//TODO: find actual length?

                tilesets[i].buildBlocks();
            }

            /*tilesets[0].pal = 0;
            tilesets[0].buildBlocks();
            tilesets[1].pal = 1;
            tilesets[1].buildBlocks();
            Graphics g = pictureBox1.CreateGraphics();
            for (int i = 0; i < tilesets[tid].tiles.Count(); i++)
                g.DrawImageUnscaled(tilesets[tid].tiles[i], (i % 16) * 8, (int)(i / 16) * 8);

            for (int i = 0; i < 0x30; i++)
                g.DrawImageUnscaled(tilesets[tid].blocks[i], (i%9)*32, (2 + (int)(i/9)) * 32);*/

            //TODO: this stuff later
            

            bR.BaseStream.Seek(Offsets.mapBankPointer, SeekOrigin.Begin);//map headers

            for(int i = 0; i < maxmaps; i++)
            {
                Map m = new Map();
                m.header.make1(bR.ReadByte(), bR.ReadByte(), bR.ReadByte(), bR.ReadUInt16(), bR.ReadByte(), bR.ReadByte(), bR.ReadByte());
                int temp = 0;
                while (temp < groupStarts.Count())
                {
                    if (groupStarts[temp] > i)
                        break;
                    temp++;
                }
                m.header.group = temp;
                maps.Add(m);
            }

            //build the map headers and load the maps
            for(int i = 0; i < maxmaps; i++)//248
            {
                bR.BaseStream.Seek((maps[i].header.hBank-1)*0x4000 + maps[i].header.pPart2, SeekOrigin.Begin);//seek to second map header
                //bR.BaseStream.Seek(0xBD3DD, SeekOrigin.Begin);

                maps[i].header.make2(bR.ReadByte(), bR.ReadByte(), bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadUInt16(), bR.ReadByte(), bR.ReadBytes(48));
                if (maps[i].header.hBank != 1 && maps[i].header.tsID < maxtiles && maps[i].header.pMapData != 0) //sanity check the invalid maps out
                {
                    bR.BaseStream.Seek((maps[i].header.hBank - 1) * 0x4000 + maps[i].header.pMapData, SeekOrigin.Begin);//seek to map data
                    int p = 0;
                    if (maps[i].header.maptype%2 == 1)
                        p = mapTs[maps[i].header.group - 1];//14 - group
                    /*if (maps[i].header.tsID == 0x11)//cave
                        p = 0x23;
                    if (maps[i].header.tsID == 0x0F)//tower/agitha
                        p = 0x19;
                    if (i < 11)
                       p = i+1;*/
                    maps[i].loadMap(bR.ReadBytes(maps[i].header.width * maps[i].header.height), tilesets[maps[i].header.tsID], p);

                    bR.BaseStream.Seek((maps[i].header.hBank - 1) * 0x4000 + maps[i].header.pMapObject, SeekOrigin.Begin);//seek to map's object data
                    maps[i].objData.make();
                    //maps[i].objData.border = bR.ReadByte();
                    bR.ReadByte();
                    bR.ReadByte();//padding?
                    maps[i].objData.warpNum = bR.ReadByte();
                    for(int j = 0; j < maps[i].objData.warpNum; j++)
                    {
                        Map.Warp w = new Map.Warp();
                        w.ypos = bR.ReadByte();
                        w.xpos = bR.ReadByte();
                        w.destID = bR.ReadByte();
                        w.destGroup = bR.ReadByte();
                        w.destMap = bR.ReadByte();
                        bR.ReadUInt16();
                        maps[i].objData.warps.Add(w);
                    }


                    //signs
                    maps[i].objData.signNum = bR.ReadByte();
                    for(int j=0;j<maps[i].objData.signNum; j++)
                    {
                        Map.Sign s = new Map.Sign();
                        s.ypos = bR.ReadByte();
                        s.xpos = bR.ReadByte();
                        bR.ReadByte();
                        s.textID = bR.ReadByte();
                        maps[i].objData.signs.Add(s);
                    }

                    //coord events??
                    /*int skip = bR.ReadByte();
                    byte[] temp = bR.ReadBytes(4 * skip);
                    byte[] temp2 = bR.ReadBytes(40);*/

                    //npcs
                    maps[i].objData.NPCnum = bR.ReadByte();
                    for(int j = 0; j < maps[i].objData.NPCnum; j++)
                    {
                        Map.NPC n = new Map.NPC();
                        n.pic = bR.ReadByte();
                        n.ypos = bR.ReadByte();
                        n.xpos = bR.ReadByte();
                        n.mov1 = bR.ReadByte();
                        n.mov2 = bR.ReadByte();
                        n.textID = bR.ReadByte();
                        n.trainer = false;
                        /*if (((n.textID >> 7) & 1) == 1)//one extra byte, item
                        {
                            n.type = (int)Map.NPC.Types.ITEM;
                            n.NPCID = bR.ReadByte();
                        }

                        if (((n.textID >> 6) & 1) == 1)//two extra bytes, trainer or poke
                        {
                            n.NPCID = bR.ReadByte();
                            n.levelroster = bR.ReadByte();
                            n.trainer = true;
                            if (n.NPCID < 200)
                                n.type = (int)Map.NPC.Types.PKMN;
                            else
                                n.type = (int)Map.NPC.Types.TRAINER;
                        }*/
                        n.u1 = bR.ReadByte();
                        n.u2 = bR.ReadByte();
                        n.u3 = bR.ReadByte();
                        n.u4 = bR.ReadByte();
                        n.u5 = bR.ReadByte();
                        n.u6 = bR.ReadByte();
                        n.u7 = bR.ReadByte();
                        maps[i].objData.NPCs.Add(n);
                    }

                    //check for scripts for sanky
                    bR.BaseStream.Seek((maps[i].header.hBank - 1) * 0x4000 + maps[i].header.pMapScript, SeekOrigin.Begin);
                    if (bR.ReadByte() == 0xC9)
                        maps[i].hasScripts = false;

                    /*
                    for(int j=0; j<maps[i].objData.warpNum; j++)
                    {
                        Map.Warp w = maps[i].objData.warps[j];
                        w.disp = bR.ReadUInt16();
                        w.dispypos = bR.ReadByte();
                        w.dispxpos = bR.ReadByte();
                        maps[i].objData.warps[j] = w;

                    }

                    //load map's sprite set
                    if (i < 37)
                    {
                        bR.BaseStream.Seek(Offsets.mapSpriteSetPointer + i, SeekOrigin.Begin);
                        maps[i].spriteSet = bR.ReadByte();
                    }

                    //load trainer data, if any
                    bool ttemp = false;
                    for(int j = 0; j < maps[i].objData.NPCnum; j++)
                    {
                        if(maps[i].objData.NPCs[j].trainer)
                        {
                            ttemp = true;
                            break;
                        }
                    }

                    if(ttemp)//if there are trainers, find the trainer headers
                    {
                        bR.BaseStream.Seek((maps[i].header.bank - 1) * 0x4000 + maps[i].header.pMapScript, SeekOrigin.Begin);

                        byte[] test;

                        test = bR.ReadBytes(16);

                        bR.BaseStream.Seek((maps[i].header.bank - 1) * 0x4000 + maps[i].header.pMapScript, SeekOrigin.Begin);

                        ushort off = 0;
                        while (true)
                        {
                            //look for the last 0x21 in the set, 0xC9 is ret
                            byte b = bR.ReadByte();

                            if (b == 0x11)//ld DE
                                bR.ReadUInt16();

                            else if (b == 0x21)//ld H1, we want this
                                off = bR.ReadUInt16();

                            else if (b == 0x3A)//ld A [xx]
                                bR.ReadUInt16();

                            else if (b == 0x3E)//LD a, x
                                bR.ReadByte();

                            else if (b == 0xAF)//XOR a, used to clear A
                                continue;

                            else if (b == 0xC3)//JP xx, this is an end
                                break;

                            else if (b == 0xC4)//call nz, xx
                                bR.ReadUInt16();

                            else if (b == 0xC8)//ret z, treat this like an end for mt. moon
                                break;

                            else if (b == 0xC9)//ret, this is an end
                                break;

                            else if (b == 0xCB)//copy bit
                                bR.ReadByte();

                            else if (b == 0xCD)//call
                                bR.ReadUInt16();

                            else if (b == 0xEA)//JP pe, xx
                                bR.ReadUInt16();

                            else if (b == 0xFA)//JP m, xx
                                bR.ReadUInt16();

                            else
                                ;// throw new Exception();

                            if (bR.BaseStream.Position >= (maps[i].header.bank - 1) * 0x4000 + maps[i].header.pMapText)//sanity check??
                                break;

                            if (bR.ReadByte() != 0xCD)//look for the jump; SS ANNE 9 (map 103) doesn't actually have this and is broken
                            {
                                t++;
                                if (t > 0xFF)
                                    break;
                            }
                            else
                            if (bR.ReadUInt16() == Offsets.autoTextBoxPointer)//jump addr 1
                            {
                                if (bR.ReadByte() == 0x21)//load [h1] address
                                    off = bR.ReadUInt16();
                            }
                        }
                        if (off != 0)//found it!!
                        {
                            //load trainers
                            bR.BaseStream.Seek((maps[i].header.bank - 1) * 0x4000 + off, SeekOrigin.Begin);
                            for (int j = 0; j < maps[i].objData.NPCnum; j++)
                            {
                                Map.TrainerHeader th = new Map.TrainerHeader();
                                if (maps[i].objData.NPCs[j].trainer && noPathTrainers.Contains(maps[i].objData.NPCs[j].NPCID) == false)//disqualify gym leaders. might need more discretion, mt moon?
                                {
                                    th.valid = true;
                                    th.flagBit = bR.ReadByte();
                                    th.viewRange = bR.ReadByte();
                                    th.pFlag = bR.ReadUInt16();
                                    th.pTextBefore = bR.ReadUInt16();
                                    th.pTextAfter = bR.ReadUInt16();
                                    th.pTextDefeat = bR.ReadUInt16();
                                    th.pTextWin = bR.ReadUInt16();
                                }
                                maps[i].trainers.Add(th);//add a blank one if not trainer, so we can just call the same element number
                            }
                        }
                    }*/
                }
            }
            /*
            //load the split sets
            bR.BaseStream.Seek(Offsets.mapSpriteSetPointer + 37, SeekOrigin.Begin);
            for (int i = 0; i < 12; i++)
            {
                SpriteSetsOW.SetSplit s = new SpriteSetsOW.SetSplit();
                s.dir = bR.ReadByte();
                s.divider = bR.ReadByte();
                s.lset = bR.ReadByte();
                s.rset = bR.ReadByte();
                SpriteSetsOW.splitSets.Add(s);
            }

            //build the sprite sets
            bR.BaseStream.Seek(Offsets.mapSpriteSetPointer + 0x55, SeekOrigin.Begin);
            for(int i = 0; i < 10; i++)//ten sets
            {
                SpriteSetsOW.Set s = new SpriteSetsOW.Set();
                s.make();
                for(int j = 0; j < 11; j++)//eleven sprites per set
                {
                    s.indexes[j] = bR.ReadByte();
                }
                SpriteSetsOW.sets.Add(s);
            }
            */
            int totalsprites = 72;
            if(gameid == 3)
            {
                totalsprites = 82;
            }
            if (gameid > 3)
                totalsprites = 0x5B;

            bR.BaseStream.Seek(Offsets.mapSpriteSetPointer, SeekOrigin.Begin);

            //load the sprite headers
            for(int i = 0; i < totalsprites; i++)//72 sprites in most - 82 in yellow/pikachu
            {
                SpriteSetsOW.SpriteData s = new SpriteSetsOW.SpriteData();
                s.dataP = bR.ReadUInt16();
                s.size = bR.ReadByte();
                s.bank = bR.ReadByte();
                SpriteSetsOW.spritedatas.Add(s);
            }
            //load each sprite's data[]
            for(int i = 0; i < totalsprites; i++)
            {
                bR.BaseStream.Seek((SpriteSetsOW.spritedatas[i].bank - 1) * 0x4000 + SpriteSetsOW.spritedatas[i].dataP, SeekOrigin.Begin);
                SpriteSetsOW.SpriteData s = SpriteSetsOW.spritedatas[i];
                s.data = bR.ReadBytes(SpriteSetsOW.spritedatas[i].size);
                SpriteSetsOW.spritedatas[i] = s;
            }
            /*
            //load up the hidden items
            bR.BaseStream.Seek(Offsets.hiddenItemPointer, SeekOrigin.Begin);
            byte temp = bR.ReadByte();
            do
            {
                HiddenItem item = new HiddenItem();
                item.map = temp;
                item.ypos = bR.ReadByte();
                item.xpos = bR.ReadByte();
                hiddenItems.Add(item);
                temp = bR.ReadByte();
            } while (temp != 0xFF);

            //load up the pokemon names
            bR.BaseStream.Seek(Offsets.pokemonNamesPointer, SeekOrigin.Begin);
            for(int i = 0; i < 0xBE; i++)//for each pokemon
            {
                string nom = "";
                for (int j = 0; j < 10; j++)//assemble the name
                {
                    byte letter = bR.ReadByte();
                    if (letter == 0x50)
                        continue;
                    nom += Convert.ToChar(letter-0x3F, System.Globalization.CultureInfo.InvariantCulture);
                }
                pokeNames.Add(nom);
            }

            //load trainer names
            bR.BaseStream.Seek(Offsets.trainerNamesPointer, SeekOrigin.Begin);
            for(int i = 0; i < 47; i++)//for each class
            {
                string nom = "";
                while(true)
                {
                    byte letter = bR.ReadByte();
                    if (letter == 0x50)
                        break;
                    nom += Convert.ToChar(letter - 0x3F, System.Globalization.CultureInfo.InvariantCulture);
                }
                trainerNames.Add(nom);
            }
            */
            bR.Close();

            loaded = true;

            toolStripStatusLabel3.Text = "Map: " + curMap.ToString();
            detailsPanel.Visible = true;
            viewToolStripMenuItem.Enabled = true;

            drawMap(curMap);

            //detailsPanel.Visible = true;
        }

        private void drawTileset(int tid)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(SystemColors.Control);
            for (int i = 0; i < tilesets[tid].tiles.Count(); i++)
                g.DrawImageUnscaled(tilesets[tid].tiles[i], (i % 16) * 8, (int)(i / 16) * 8);

            for (int i = 0; i < 128; i++)
                g.DrawImageUnscaled(tilesets[tid].blocks[i], (i % 10) * 32, (2 + (int)(i / 10)) * 32);
        }

        private void drawMap(int mid)
        {
            if (!loaded)
                return;

            if (maps[mid].img == null)
                maps[mid].buildMap();

            Graphics g = pictureBox1.CreateGraphics();

            g.Clear(SystemColors.Control);
            
            g.DrawImageUnscaled(maps[mid].img, 0, 0);

            if (viewEnts)
            {
                for (int i = 0; i < maps[mid].objData.warpNum; i++)//highlight warps
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.MediumBlue)), maps[mid].objData.warps[i].xpos * 16, maps[mid].objData.warps[i].ypos * 16, 16, 16);
                }
                for (int i = 0; i < maps[mid].objData.signNum; i++)//highlight signs
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Turquoise)), maps[mid].objData.signs[i].xpos * 16, maps[mid].objData.signs[i].ypos * 16, 16, 16);
                }
                for (int i = 0; i < maps[mid].objData.NPCnum; i++)//highlight NPCs
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Purple)), (maps[mid].objData.NPCs[i].xpos - 4) * 16, (maps[mid].objData.NPCs[i].ypos - 4) * 16, 16, 16);
                    //draw trainer sight lines
                    if (maps[mid].trainers.Count() > 0)
                        if (maps[mid].trainers[i].valid)//is npc a trainer?
                        {
                            if ((maps[mid].trainers[i].viewRange >> 4) == 0)//is range 0? mark with a red dot
                                g.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Red)), (maps[mid].objData.NPCs[i].xpos - 4) * 16 + 6, (maps[mid].objData.NPCs[i].ypos - 4) * 16 + 6, 4, 4);
                            else
                            {
                                int dir = maps[mid].objData.NPCs[i].mov2 & 0x3;
                                if (maps[mid].objData.NPCs[i].mov2 == 0xff)
                                    dir = 0;
                                if (dir == 0)//down
                                    g.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Red)), (maps[mid].objData.NPCs[i].xpos - 4) * 16 + 6, (maps[mid].objData.NPCs[i].ypos - 3) * 16, 4, 16 * (maps[mid].trainers[i].viewRange >> 4));
                                if (dir == 1)//up
                                    g.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Red)), (maps[mid].objData.NPCs[i].xpos - 4) * 16 + 6, (maps[mid].objData.NPCs[i].ypos - 4 - (maps[mid].trainers[i].viewRange >> 4)) * 16, 4, 16 * (maps[mid].trainers[i].viewRange >> 4));
                                if (dir == 2)//left
                                    g.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Red)), (maps[mid].objData.NPCs[i].xpos - 4 - (maps[mid].trainers[i].viewRange >> 4)) * 16, (maps[mid].objData.NPCs[i].ypos - 4) * 16 + 6, 16 * (maps[mid].trainers[i].viewRange >> 4), 4);
                                if (dir == 3)//right
                                    g.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Red)), (maps[mid].objData.NPCs[i].xpos - 3) * 16, (maps[mid].objData.NPCs[i].ypos - 4) * 16 + 6, 16 * (maps[mid].trainers[i].viewRange >> 4), 4);
                            }
                        }
                }

                for (int i = 0; i < hiddenItems.Count(); i++)//hidden items
                {
                    if (hiddenItems[i].map == mid)
                        g.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.Purple)), hiddenItems[i].xpos * 16, hiddenItems[i].ypos * 16, 16, 16);
                }

            }

            //draw selection box
            if (selx != -1 && sely != -1)
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), selx * 16, sely * 16, 16, 16);
            

            toolStripStatusLabel3.Text = "Map: " + mid.ToString();
        }

        struct HiddenItem
        {
            public byte map;
            public byte xpos;
            public byte ypos;
        }

        public struct PokeTeam
        {
            public List<byte> levels;
            public List<byte> species;
        }

        public void getTeam(int type, int roster)
        {
            //open the bR
            BinaryReader bR = new BinaryReader(File.OpenRead(file));
            //read 'type' entries into the pointers, jump
            bR.BaseStream.Seek(Offsets.trainersPointer + 2 * (type - 201), SeekOrigin.Begin);
            bR.BaseStream.Seek(((long)Math.Floor(bR.BaseStream.Position / (decimal)0x4000) - 1) * 0x4000 + bR.ReadUInt16(), SeekOrigin.Begin);
            //read 0x0s until you meet roster
            for(int i = 0; i < roster - 1; i++)
            {
                while (bR.ReadByte() != 0x00) ;
            }
            //parse and return party
            curTeam.levels = new List<byte>();
            curTeam.species = new List<byte>();
            byte first = bR.ReadByte();
            if (first == 0xFF)//individual levels for poke, read level and then species
            {
                while (true)
                {
                    byte b = bR.ReadByte();
                    if (b == 0x00)
                        break;
                    curTeam.levels.Add(b);
                    curTeam.species.Add(bR.ReadByte());
                }
            }
            else //whole team is level, read indexes
            {
                while (true)
                {
                    byte b = bR.ReadByte();
                    if (b == 0x00)
                        break;
                    curTeam.levels.Add(first);
                    curTeam.species.Add(b);
                }
            }
        }

        //window events

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void onResize(object sender, EventArgs e)
        {
            if (viewTiles)
                drawTileset(curTiles);
            else
                drawMap(curMap);
        }

        private void entityMarkersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewEnts = !viewEnts;
            entityMarkersToolStripMenuItem.Checked = viewEnts;
            drawMap(curMap);
        }

        private void toolStripStatusLabel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (viewTiles)
                {
                    curTiles += 1;
                    curTiles %= maxtiles;
                    toolStripStatusLabel3.Text = "TS: " + curTiles.ToString();
                }
                else
                {
                    prevMap = -1;
                    do
                    {
                        curMap += 1;
                        if (curMap == maxmaps)
                            curMap = 0;
                    } while (maps[curMap].objData.NPCnum > 0x80); //sanity check the invalid maps out
                    showMapDeets();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (viewTiles)
                {
                    curTiles -= 1;
                    if (curTiles < 0)
                        curTiles = maxtiles - 1;
                    toolStripStatusLabel3.Text = "TS: " + curTiles.ToString();
                }
                else
                {
                    prevMap = -1;
                    do
                    {
                        curMap -= 1;
                        if (curMap == -1)
                            curMap = maxmaps - 1;
                    } while (maps[curMap].objData.NPCnum > 0x80); //sanity check the invalid maps out
                    showMapDeets();
                }
            }
            if (e.Button == MouseButtons.Middle)
            {
                //popup to select new map
                string destS = Prompt.ShowDialog("Enter destination map", "");
                int destI = -1;
                bool p = false;
                if(destS.Length > 2)
                    if(destS.Substring(0,2) == "0x")//hex
                    {
                        destI = Convert.ToInt32(destS, 16);
                        p = true;
                    }
                if (!p)
                {
                    int temp;
                    if(int.TryParse(destS, out temp))
                        destI = temp;
                }

                if (destI != -1)
                    if (maps[destI].header.pMapData != 0 && maps[destI].header.hBank != 1 && maps[destI].header.tsID < maxtiles)
                    {
                        curMap = destI;
                        prevMap = -1;
                    }

            }
            selx = -1;
            sely = -1;
            detailsLabel.Text = "";
            if (viewTiles)
                drawTileset(curTiles);
            else
            {
                drawMap(curMap);
                showMapDeets();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!loaded)
                return;

            //translate click into tile coords
            selx = (int)Math.Floor((float)e.X / 16);
            sely = (int)Math.Floor((float)e.Y / 16);
            detailsLabel.Text = "";

            bool selected = false;

            //find if any NPCs were clicked on
            for(int i = 0; i < maps[curMap].objData.NPCnum; i++)
            {
                if (selx+4 == maps[curMap].objData.NPCs[i].xpos && sely+4 == maps[curMap].objData.NPCs[i].ypos)
                {
                    //clicked on an NPC
                    selected = true;
                    /*if (maps[curMap].objData.NPCs[i].trainer)
                    {
                        if (maps[curMap].objData.NPCs[i].NPCID > 200)//trainer
                        {
                            getTeam(maps[curMap].objData.NPCs[i].NPCID, maps[curMap].objData.NPCs[i].levelroster);
                            detailsLabel.Text = trainerNames[maps[curMap].objData.NPCs[i].NPCID - 201] + "\nClass Roster #" + maps[curMap].objData.NPCs[i].levelroster.ToString() + "\n";
                            for (int j = 0; j < curTeam.levels.Count(); j++)
                                detailsLabel.Text += "LV: " + curTeam.levels[j].ToString() + " " + pokeNames[curTeam.species[j] - 1] + "\n";
                        }
                        else //pokemon
                        {
                            detailsLabel.Text = pokeNames[maps[curMap].objData.NPCs[i].NPCID - 1] + " LV: " + maps[curMap].objData.NPCs[i].levelroster.ToString();
                        }
                    }
                    else //generic npc*/
                    {
                        detailsLabel.Text = maps[curMap].objData.NPCs[i].mov1.ToString();
                        detailsLabel.Text += "\n" + maps[curMap].objData.NPCs[i].u2.ToString();
                    }
                    break;
                }
            }
            if(!selected)
            {
                for(int i = 0; i < maps[curMap].objData.signNum; i++)
                {
                    if (selx == maps[curMap].objData.signs[i].xpos && sely == maps[curMap].objData.signs[i].ypos)
                    {
                        //clicked on a sign
                        selected = true;
                        detailsLabel.Text = "Text ID: " + maps[curMap].objData.signs[i].textID.ToString();
                        break;
                    }
                }
            }
            if(!selected)
            {
                for(int i = 0; i < maps[curMap].objData.warpNum; i++)
                {
                    if (selx == maps[curMap].objData.warps[i].xpos && sely == maps[curMap].objData.warps[i].ypos)
                    {
                        //clicked on a warp
                        selected = true;
                        dState = DEETS.WARP;
                        if (e.Button == MouseButtons.Left)//display
                        {
                            detailsLabel.Text = "Destination group: " + maps[curMap].objData.warps[i].destGroup.ToString() + "\nDestination map: " + maps[curMap].objData.warps[i].destMap.ToString() + "\nDestination ID: " + maps[curMap].objData.warps[i].destID.ToString();
                            if (maps[curMap].objData.warps[i].destID == 0xFF && prevMap != -1)
                                detailsLabel.Text += " (" + prevMap.ToString() + ")";
                            break;
                        }
                        if (e.Button == MouseButtons.Right)//follow
                        {
                            int dm = groupStarts[maps[curMap].objData.warps[i].destGroup - 1] + maps[curMap].objData.warps[i].destMap - 1;
                            if (dm == 0xFF && prevMap != -1)
                                dm = prevMap;
                            if (dm != 0xFF)
                                if (maps[dm].header.pMapData != 0 && maps[dm].header.hBank != 1 && maps[dm].header.tsID < maxtiles)
                                {
                                    //highlight where you came from
                                    selx = maps[dm].objData.warps[maps[curMap].objData.warps[i].destID - 1].xpos;
                                    sely = maps[dm].objData.warps[maps[curMap].objData.warps[i].destID - 1].ypos;
                                    detailsLabel.Text = "";
                                    //go there
                                    if (curMap < 37)
                                        prevMap = curMap;
                                    curMap = dm;
                                    resetPanel();
                                }
                            break;
                        }
                    }
                }
            }
            if (!selected)//clicked on nothing, show map data again
            {
                showMapDeets();
            }

            drawMap(curMap);
        }

        protected override void WndProc(ref Message m)//prevent alt key redraw from hiding picturebox's image
        {
            // Suppress the WM_UPDATEUISTATE message
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }

        private void mapOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maps[curMap].img.Save(maps[curMap].header.group.ToString() + " " + (curMap - groupStarts[maps[curMap].header.group - 1] + 1).ToString() + " (" + curMap.ToString() + ")" + ".png");
        }

        private void tilesetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewTiles = !viewTiles;
            tilesetToolStripMenuItem.Checked = viewTiles;
            drawTileset(curTiles);
        }

        private void detailsLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && dState == DEETS.MAP)
            {
                int y = e.Y / 25;
                if (y < maps[curMap].header.connections.Count())
                {
                    //detailsLabel.Text += y + "\n";
                    curMap = groupStarts[maps[curMap].header.connections[y].connGroup - 1] + maps[curMap].header.connections[y].connID - 1;
                    drawMap(curMap);
                    resetPanel();
                    showMapDeets();
                }
            }
        }

        void resetPanel()
        {
            detailsLabel.Text = "";
            dState = DEETS.NULL;
        }

        private void dataDumpToTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryWriter bW = new BinaryWriter(File.Create("dump.txt"));
            for (int i = 0; i < maxmaps; i++)
            {
                
                bW.Write("Map " + i.ToString() + " (" + maps[curMap].header.group.ToString() + "-" + (curMap - groupStarts[maps[curMap].header.group - 1] + 1).ToString() + ")\n");
                bW.Write("W: " + maps[i].header.width.ToString() + "\n");
                bW.Write("H: " + maps[i].header.height.ToString() + "\n");
                bW.Write("NPC total: " + maps[i].objData.NPCnum.ToString() + "\n");
                bW.Write("Number of connections: " + maps[i].header.connections.Count().ToString() + "\n");
                bW.Write("Has script: " + maps[i].hasScripts.ToString() + "\n");

            }

            bW.Close();
        }

        void showMapDeets()
        {
            selx = -1;
            sely = -1;

            dState = DEETS.MAP;
            byte dummy = maps[curMap].header.conByte;
            for (int i = 0; i < maps[curMap].header.connections.Count(); i++)
            {
                if ((dummy & 0x08) == 0x08)
                {
                    detailsLabel.Text += "North connection: \n";
                    dummy = (byte)(dummy - 8);
                }
                else if ((dummy & 0x04) == 0x04)
                {
                    detailsLabel.Text += "South connection: \n";
                    dummy = (byte)(dummy - 4);
                }
                else if ((dummy & 0x02) == 0x02)
                {
                    detailsLabel.Text += "West connection: \n";
                    dummy = (byte)(dummy - 2);
                }
                else if ((dummy & 0x01) == 0x01)
                {
                    detailsLabel.Text += "East connection: \n";
                    dummy = (byte)(dummy - 1);
                }
                detailsLabel.Text += "Group: " + maps[curMap].header.connections[i].connGroup.ToString() + " Map: " + maps[curMap].header.connections[i].connID.ToString() + "\n";
            }
            //list the pointers?

            detailsLabel.Text += "\nWidth: " + maps[curMap].header.width.ToString() + "\nHeight: " + maps[curMap].header.height.ToString();

            detailsLabel.Text += "\nHas scripts? " + maps[curMap].hasScripts.ToString();

            detailsLabel.Text += "\nNumber of signs: " + maps[curMap].objData.signNum.ToString();

            detailsLabel.Text += "\nNumber of NPCs: " + maps[curMap].objData.NPCnum.ToString();

            detailsLabel.Text += "\nMap Type?: " + maps[curMap].header.maptype.ToString();
        }
    }
}
