using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elf_s_World
{
    static class PaletteList
    {
        public struct Pal
        {
            public Color[] colors;

            public void make()
            {
                colors = new Color[4];
            }
        }
        public static List<Pal> pals = new List<Pal>();

        public static void reset()
        {
            pals = new List<Pal>();
        }

        public static void loadImmediate(ushort nc1, ushort nc2, ushort nc3, ushort nc4)
        {
            Pal p = new Pal();
            p.make();
            //five-bit color
            p.colors[0] = Color.FromArgb(((nc1 & 0x1F) << 3), (((nc1 >> 5) & 0x1F) << 3), (((nc1 >> 10) & 0x1F) << 3));
            p.colors[1] = Color.FromArgb(((nc2 & 0x1F) << 3), (((nc2 >> 5) & 0x1F) << 3), (((nc2 >> 10) & 0x1F) << 3));
            p.colors[2] = Color.FromArgb(((nc3 & 0x1F) << 3), (((nc3 >> 5) & 0x1F) << 3), (((nc3 >> 10) & 0x1F) << 3));
            p.colors[3] = Color.FromArgb(((nc4 & 0x1F) << 3), (((nc4 >> 5) & 0x1F) << 3), (((nc4 >> 10) & 0x1F) << 3));
            pals.Add(p);
        }

        /*public static void addPalette(byte[] data)
        {
            Palette p = new Palette();
            p.load(data);
            pals.Add(p);
        }*/
    }
}
