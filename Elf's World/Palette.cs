using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elf_s_World
{
    class Palette
    {
        public List<Color> colors = new List<Color> { Color.White, Color.LightGray, Color.DarkGray, Color.Black };
        public ushort c1, c2, c3, c4;

        public Palette()
        {

        }

        public Palette(Palette p)
        {
            c1 = p.c1;
            c2 = p.c2;
            c3 = p.c3;
            c4 = p.c4;
        }

        public void load(byte[] cols)//should be eight bytes, two per colour
        {
            c1 = BitConverter.ToUInt16(cols, 0);
            c2 = BitConverter.ToUInt16(cols, 2);
            c3 = BitConverter.ToUInt16(cols, 4);
            c4 = BitConverter.ToUInt16(cols, 6);
        }
        public void loadImmediate(ushort nc1, ushort nc2, ushort nc3, ushort nc4)
        {
            //five-bit color X 8.22, round
            colors[0] = Color.FromArgb(((nc1 & 0x1F) << 3), (((nc1 >> 5) & 0x1F) << 3), (((nc1 >> 10) & 0x1F) << 3));
            colors[1] = Color.FromArgb(((nc2 & 0x1F) << 3), (((nc2 >> 5) & 0x1F) << 3), (((nc2 >> 10) & 0x1F) << 3));
            colors[2] = Color.FromArgb(((nc3 & 0x1F) << 3), (((nc3 >> 5) & 0x1F) << 3), (((nc3 >> 10) & 0x1F) << 3));
            colors[3] = Color.FromArgb(((nc4 & 0x1F) << 3), (((nc4 >> 5) & 0x1F) << 3), (((nc4 >> 10) & 0x1F) << 3));
        }
        public void build()
        {
            colors[0] = Color.FromArgb(((c1 & 0x1F) << 3), (((c1 >> 5) & 0x1F) << 3), (((c1 >> 10) & 0x1F) << 3));
            colors[1] = Color.FromArgb(((c2 & 0x1F) << 3), (((c2 >> 5) & 0x1F) << 3), (((c2 >> 10) & 0x1F) << 3));
            colors[2] = Color.FromArgb(((c3 & 0x1F) << 3), (((c3 >> 5) & 0x1F) << 3), (((c3 >> 10) & 0x1F) << 3));
            colors[3] = Color.FromArgb(((c4 & 0x1F) << 3), (((c4 >> 5) & 0x1F) << 3), (((c4 >> 10) & 0x1F) << 3));
        }
    }
}
