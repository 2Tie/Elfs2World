using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elf_s_World
{
    class Map
    {
        public struct Header
        {
            public byte hBank;
            public byte tsID;
            public byte height;
            public byte width;
            public ushort pPart2;
            public byte loc;
            public byte maptype;
            public byte song;
            public ushort pMapData;
            public ushort pMapText;
            public ushort pMapScript;
            public ushort pUnknown;
            public byte conByte;
            public List<Connection> connections;
            public ushort pMapObject;
            public int group;

            
            public void make1(byte b, byte t, byte u1, ushort p, byte l, byte m, byte u2)
            {
                hBank = b;
                tsID = t;
                maptype = u1; //1 for towns, 2 for routes, 3 indoors, 4 cave, 6 is gates and cable club
                pPart2 = p;
                loc = l;
                song = m;
                //u2 is always zero
            }

            public void make2(byte h, byte w, ushort d, ushort u1, ushort script, ushort obj, byte mask, byte[] data)
            {
                height = h;
                width = w;
                pMapData = d;
                pUnknown = u1;
                pMapScript = script;
                pMapObject = obj;
                conByte = mask;
                connections = new List<Connection>();

                int size = 0;
                int i = 0;

                for (; i < 4; i++)
                {
                    if (((mask >> i) & 1) == 1)
                        size++;
                }
                for (i = 0; i < size; i++)
                {
                    Connection c = new Connection();
                    c.connGroup = data[i * 12];
                    c.connID = data[i * 12+ 1];
                    c.pConnTile = BitConverter.ToUInt16(data, i * 12 + 2);
                    c.pCurrTile = BitConverter.ToUInt16(data, i * 12 + 4);
                    c.bigness = data[i * 12 + 6];
                    c.width = data[i * 12 + 7];
                    c.yAlign = data[i * 12 + 8];
                    c.xAlign = data[i * 12 + 9];
                    c.window = BitConverter.ToUInt16(data, i * 12 + 10);
                    connections.Add(c);
                }

                //pMapObject = BitConverter.ToUInt16(data, i * 11);

            }
            /*public void setBank(byte id)
            {
                bank = id;
            }*/
        }

        public struct TrainerHeader
        {
            public bool valid;
            public byte flagBit;
            public byte viewRange;
            public ushort pFlag;
            public ushort pTextBefore;
            public ushort pTextAfter;
            public ushort pTextDefeat;
            public ushort pTextWin;//like what rival does?
        }

        public struct Connection
        {
            public byte connGroup;
            public byte connID;
            public ushort pConnTile;
            public ushort pCurrTile;
            public byte bigness;
            public byte width;
            public byte yAlign;
            public byte xAlign;
            public ushort window;
        }

        public struct ObjData
        {
            public byte border;
            public byte warpNum;
            public List<Warp> warps;
            public byte signNum;
            public List<Sign> signs;
            public byte NPCnum;
            public List<NPC> NPCs;

            public void make()
            {
                warps = new List<Warp>();
                signs = new List<Sign>();
                NPCs = new List<NPC>();
            }
        }

        public struct Warp
        {
            public byte ypos;
            public byte xpos;
            public byte destID;
            public byte destMap;
            public byte destGroup;
        }

        public struct Sign
        {
            public byte ypos;
            public byte xpos;
            public byte textID;
        }

        public struct NPC
        {
            public enum Types { NPC, TRAINER, PKMN=1, ITEM};
            public int type;
            public bool trainer;
            public byte pic;
            public byte ypos;
            public byte xpos;
            public byte mov1;
            public byte mov2;
            public byte textID;
            public byte NPCID;
            public byte levelroster;
            public byte u1, u2, u3, u4, u5, u6, u7;
        }

        public Header header;
        public ObjData objData;
        public Bitmap img;
        public Tileset tileset;
        public List<TrainerHeader> trainers = new List<TrainerHeader>();

        public byte[] mapdata;

        public bool hasScripts = true;

        //public int pal = 0;
        public int spriteSet = -1;

        public void loadMap(byte[] data, Tileset t, int p)
        {
            mapdata = data;
            tileset = new Tileset();
            tileset.header = t.header;
            tileset.tiledata = t.tiledata;
            tileset.blockdata = t.blockdata;
            tileset.pal = p;
            //tileset.pal.build();
        }

        public void buildMap()
        {
            //make pallete
            //tileset.buildColors();
            //draw blocks
            tileset.buildBlocks();
            img = new Bitmap(header.width * 32, header.height * 32);
            Graphics g = Graphics.FromImage(img);
            
            //List<SolidBrush> mcol = new List<SolidBrush> { new SolidBrush(p.colors[0]), new SolidBrush(p.colors[1]), new SolidBrush(p.colors[2]), new SolidBrush(p.colors[3]) };
            
            for (int i = 0; i < mapdata.Count(); i++)//for each block in map
            {
                /*
                //draw each block
                for (int j = 0; j < 16; j++)//for each tile in block (4x4)
                {
                    //draw each tile
                    for (int k = 0; k < 8; k++)//for each row in tile
                    {
                        //draw each row
                        for (int l = 7; l > -1; l--)//for each pixel in row
                        {
                            //draw the pixels
                            int c = (t.tiledata[t.blockdata[data[i] * 16 + j] * 16 + k * 2] >> l) & 1;
                            c += ((t.tiledata[t.blockdata[data[i] * 16 + j] * 16 + k * 2 + 1] >> l) & 1) << 1;
                            g.FillRectangle(mcol[c], 7 - l + (j % 4) * 8, k + (int)(j / 4) * 8, 1, 1);
                        }
                    }
                }*/
                g.DrawImageUnscaled(tileset.blocks[mapdata[i]], (i%header.width)*32, (int)(i/header.width)*32);
            }
            
            //draw the NPCs
            if(spriteSet != -1 && spriteSet < 0xf0)
            {
                //if we're not indoors, aka map < 25
                //build the set with our pallete
                SpriteSetsOW.sets[spriteSet-1].build(tileset.pal);
                //draw them
                for(int i = 0; i < objData.NPCnum; i++)
                {
                    int cel = Array.IndexOf(SpriteSetsOW.sets[spriteSet-1].indexes, objData.NPCs[i].pic);
                    Bitmap img;
                    int dir = objData.NPCs[i].u2 & 0x3;
                    if (objData.NPCs[i].u2 == 0xff)
                        dir = 0;
                    if (dir < 3)
                        img = new Bitmap(SpriteSetsOW.sets[spriteSet - 1].sprites[cel].frames[dir]);
                    else
                    {
                        img = new Bitmap(SpriteSetsOW.sets[spriteSet - 1].sprites[cel].frames[2]);
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    g.DrawImageUnscaled(img, (objData.NPCs[i].xpos - 4) * 16, (objData.NPCs[i].ypos - 4) * 16);
                }
            } else if (spriteSet == -1)//interior, use scratch
            {
                SpriteSetsOW.interiorSet.make();
                //populate the Set
                int skip = 0;
                for (int i = 0; i < objData.NPCs.Count(); i++)
                {
                    if (SpriteSetsOW.interiorSet.indexes.Contains(objData.NPCs[i].pic) == false && i - skip < 11)//if it's not already in, add it
                    {
                        SpriteSetsOW.interiorSet.indexes[i - skip] = objData.NPCs[i].pic;
                    }
                    else skip += 1;
                }
                //build the Set
                SpriteSetsOW.interiorSet.build(tileset.pal);
                //draw them
                for (int i = 0; i < objData.NPCnum; i++)
                {
                    int cel = Array.IndexOf(SpriteSetsOW.interiorSet.indexes, objData.NPCs[i].pic);
                    Bitmap img;
                    int dir = (objData.NPCs[i].mov1-1) & 0x3;
                    if (objData.NPCs[i].u2 == 0xff)
                        dir = 0;
                    if (dir < 3)
                        img = new Bitmap(SpriteSetsOW.interiorSet.sprites[cel].frames[dir]);
                    else
                    {
                        img = new Bitmap(SpriteSetsOW.interiorSet.sprites[cel].frames[2]);
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    g.DrawImageUnscaled(img, (objData.NPCs[i].xpos - 4) * 16, (objData.NPCs[i].ypos - 4) * 16);
                }
            }
            else //set > 0xf0, use split set
            {
                int sprset = spriteSet - 0xF1;
                //build both sets
                SpriteSetsOW.sets[SpriteSetsOW.splitSets[sprset].lset - 1].build(tileset.pal);
                SpriteSetsOW.sets[SpriteSetsOW.splitSets[sprset].rset - 1].build(tileset.pal);
                //draw each npc
                for (int i = 0; i < objData.NPCnum; i++)
                {
                    //determine which set the npc belongs to
                    int set = 0;
                    if (SpriteSetsOW.splitSets[sprset].dir == 1)//left & right
                    {
                        if (objData.NPCs[i].xpos < SpriteSetsOW.splitSets[sprset].divider)
                            set = SpriteSetsOW.splitSets[sprset].lset - 1;
                        else
                            set = SpriteSetsOW.splitSets[sprset].rset - 1;
                    } else //up & down
                    {
                        if (objData.NPCs[i].ypos < SpriteSetsOW.splitSets[sprset].divider)
                            set = SpriteSetsOW.splitSets[sprset].lset - 1;
                        else
                            set = SpriteSetsOW.splitSets[sprset].rset - 1;
                    }

                    //draw the npc
                    int cel = Array.IndexOf(SpriteSetsOW.sets[set].indexes, objData.NPCs[i].pic);
                    Bitmap img;
                    int dir = objData.NPCs[i].mov2 & 0x3;
                    if (objData.NPCs[i].mov2 == 0xff)
                        dir = 0;
                    if (dir < 3)
                        img = new Bitmap(SpriteSetsOW.sets[set].sprites[cel].frames[dir]);
                    else
                    {
                        img = new Bitmap(SpriteSetsOW.sets[set].sprites[cel].frames[2]);
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    g.DrawImageUnscaled(img, (objData.NPCs[i].xpos - 4) * 16, (objData.NPCs[i].ypos - 4) * 16);
                }
            }
        }
    }
}
