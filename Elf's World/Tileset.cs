using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elf_s_World
{
    class Tileset
    {
        public struct Header
        {
            public byte bankID;
            public ushort pBlocks, pTiles, pCollision;
            byte TOT1, TOT2, TOT3;
            byte grass;
            byte animated;//nonzero animates water, 2 animates flower

            public byte[] collision;

            public void make(byte bid, ushort pb, ushort pt, ushort pc, ushort pu1, byte t3, byte unknown)
            {
                bankID = bid;
                pBlocks = pb;
                pTiles = pt;
                pCollision = pc;
                //TOT1 = t1;
                //TOT2 = t2;
                TOT3 = t3;
            }
        }

        //public PaletteList.Pal pal;
        public int pal;

        public Header header;
        public List<Bitmap> tiles = new List<Bitmap>();
        public List<Bitmap> blocks = new List<Bitmap>();
        public List<byte> collision = new List<byte>();

        public byte[] tiledata;
        public byte[] blockdata;

        public void loadTiles(byte[] data)
        {
            tiledata = data;
        }

        public void buildTiles()
        {
            for (int i = 0; i < tiledata.Count() / 0x10; i++)//0x10 per tile
            {
                //for each tile
                tiles.Add(new Bitmap(8, 8));
                Graphics g = Graphics.FromImage(tiles[i]);
                int tile = i * 16;
                for (int j = 0; j < 8; j++)
                {
                    //for each row
                    for (int k = 7; k > -1; k--)
                    {
                        //for each pixel
                        int c = (tiledata[tile + j * 2] >> k) & 1;
                        c += ((tiledata[tile + j * 2 + 1] >> k) & 1) << 1;
                        g.FillRectangle(new SolidBrush(PaletteList.pals[pal].colors[c]), 7 - k, j, 1, 1);
                    }
                }
            }
        }

        public void loadBlocks(byte[] data)
        {
            blockdata = data;
        }

        public void buildBlocks()
        {
            buildTiles();
            for (int i = 0; i < blockdata.Count() / 0x10; i++)//0x10 per block
            {
                //sixteen bytes, rows from top to bottom
                blocks.Add(new Bitmap(32, 32));
                Graphics g = Graphics.FromImage(blocks[i]);
                for (int j = 0; j < 16; j++)
                {
                    g.DrawImageUnscaled(tiles[blockdata[i * 16 + j] % 0x60], (j % 4) * 8, (int)(j / 4) * 8);
                }
            }
        }

        /*public void buildColors()
        {
            pal.build();
        }*/

        /*public void loadPal(Palette p)
        {
            pal = p;
        }*/
    }
}
