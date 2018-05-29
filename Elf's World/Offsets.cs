using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elf_s_World
{
    static class Offsets
    {
        public static int tilesetHeaderPointer = 0x0;
        public static int palletesPointer = 0x0;
        public static int mapHeaderPointer = 0x0;
        public static int mapBankPointer = 0x0;
        public static int mapSpriteSetPointer = 0x0;
        public static int hiddenItemPointer = 0x0;
        public static int trainersPointer = 0x0;
        public static int pokemonNamesPointer = 0x0;
        public static int trainerNamesPointer = 0x0;


        public static void populate(byte region, int gameID)//r is 0, g is 1, b is 2, y is 3
        {
            if(region == 0x00)//japan
            {
                switch (gameID)
                {
                    case 0:
                    case 1:
                        tilesetHeaderPointer = 0xCDF7;
                        palletesPointer = 0x72AB6;
                        mapHeaderPointer = 0x1BCB;
                        mapBankPointer = 0xC883;
                        mapSpriteSetPointer = 0x17A49;
                        hiddenItemPointer = 0x77DA8;
                        trainersPointer = 0x3A0AC;
                        pokemonNamesPointer = 0x39068;
                        trainerNamesPointer = 0x39D1C;
                        break;
                    case 2:
                        tilesetHeaderPointer = 0xC7E9;
                        palletesPointer = 0x72AA5;
                        mapHeaderPointer = 0x167;
                        mapBankPointer = 0xC275;
                        mapSpriteSetPointer = 0x17A64;
                        hiddenItemPointer = 0x77DAB;
                        trainersPointer = 0x3A0AC;
                        pokemonNamesPointer = 0x39446;
                        trainerNamesPointer = 0x39DB5;
                        break;
                    case 3:
                        tilesetHeaderPointer = 0xC554;
                        palletesPointer = 0x726E7;
                        mapHeaderPointer = 0xFC1F2;
                        mapBankPointer = 0xFC3E4;
                        mapSpriteSetPointer = 0x141E6;
                        hiddenItemPointer = 0x77CA4;
                        trainersPointer = 0x3A142;
                        pokemonNamesPointer = 0x39462;
                        trainerNamesPointer = 0x39D34;
                        break;
                    case 4://poke2 nodebug
                        tilesetHeaderPointer = 0xC875;
                        palletesPointer = 0x9C4C;
                        mapBankPointer = 0x10023;
                        mapSpriteSetPointer = 0x1423B;
                        break;
                }
            }
            else//US
            {
                switch (gameID)
                {
                    case 0:
                    case 2:
                        tilesetHeaderPointer = 0xC7BE;//c767?
                        palletesPointer = 0x72660;
                        mapHeaderPointer = 0x1AE;
                        mapBankPointer = 0xC23D;
                        mapSpriteSetPointer = 0x17A64; //17AB9; +55 for sprite sets
                        hiddenItemPointer = 0x766B9;
                        trainersPointer = 0x39D3B;
                        pokemonNamesPointer = 0x1C21E;
                        trainerNamesPointer = 0x399FF;
                        break;
                    case 3:
                        tilesetHeaderPointer = 0xC558;
                        palletesPointer = 0x729B9;//0x72AF9 for GBC pals
                        mapHeaderPointer = 0xFC1F2;
                        mapBankPointer = 0xFC3E4;
                        mapSpriteSetPointer = 0x141E6;
                        hiddenItemPointer = 0x75FCB;
                        trainersPointer = 0x39DD1;
                        pokemonNamesPointer = 0xE8000;
                        trainerNamesPointer = 0x3997E;
                        break;
                }
            }
        }
    }
}
