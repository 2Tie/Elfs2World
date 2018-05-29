using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elf_s_World
{
    static class SpriteSetsOW
    {
        public static List<Set> sets = new List<Set>();
        public static List<SpriteData> spritedatas = new List<SpriteData>();

        public static Set interiorSet;

        public static List<SetSplit> splitSets = new List<SetSplit>();

        public static void init()
        {
            sets = new List<Set>();
            spritedatas = new List<SpriteData>();
        }

        public struct Set //each has eleven sprite entries
        {
            public byte[] indexes;
            public List<SetSprite> sprites;

            //public List<Bitmap> sprs;
            public void make()
            {
                indexes = new byte[11];
                sprites = new List<SetSprite>();
                //sprs = new List<Bitmap>();
            }

            public void build(int pal)
            {
                for (int i = 0; i < 11; i++)//0x10 per char, four chars per frame. most have three frames? (down, up, left. mirror left for right)
                {
                    if (indexes[i] != 0)//ignore unused interior slots
                    {
                        SetSprite s = new SetSprite();
                        s.data = spritedatas[indexes[i] - 1].data;
                        s.size = spritedatas[indexes[i] - 1].size;
                        s.frames = new List<Bitmap>();
                        int size = 0;
                        //for each sprite
                        while (size * 0x40 < s.size)
                        {
                            //for each frame
                            s.frames.Add(new Bitmap(16, 16));
                            Graphics g = Graphics.FromImage(s.frames[size]);
                            g.Clear(Color.Transparent);
                            //for each charactor
                            for (int n = 0; n < 4; n++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    //for each row
                                    for (int k = 7; k > -1; k--)
                                    {
                                        //for each pixel
                                        int c = (s.data[size * 0x40 + (n * 0x10) + j * 2] >> k) & 1;
                                        c += ((s.data[size * 0x40 + (n * 0x10) + j * 2 + 1] >> k) & 1) << 1;
                                        if (c < 3)
                                            c = (c + 2) % 3;
                                        g.FillRectangle(new SolidBrush(PaletteList.pals[pal].colors[c]), (n % 2) * 8 + 7 - k, (int)(n / 2) * 8 + j, 1, 1);
                                    }
                                }
                            }
                            s.frames[size].MakeTransparent(PaletteList.pals[pal].colors[2]);
                            size += 1;
                        }
                        if (sprites.Count() < 11)
                            sprites.Add(s);
                        else
                            sprites[i] = s;
                    }
                }
            }
        }

        public struct SpriteData
        {
            public ushort dataP;
            public byte size;
            public byte bank;

            public byte[] data;
        }

        public struct SetSprite
        {
            public byte size;
            public byte[] data;

            public List<Bitmap> frames;
        }

        public struct SetSplit
        {
            public byte dir;
            public byte divider;
            public byte lset;
            public byte rset;
        }
    }
}
