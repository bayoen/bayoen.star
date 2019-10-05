using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.star.Functions
{
    public class Mapping
    {
        public static int SelectionToAvatar(int index)
        {
            switch (index)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 8;
                case 3: return 16;
                case 4: return 17;
                case 5: return 2;
                case 6: return 3;
                case 7: return 20;
                case 8: return 13;
                case 9: return 14;
                case 10: return 4;
                case 11: return 5;
                case 12: return 9;
                case 13: return 15;
                case 14: return 12;
                case 15: return 6;
                case 16: return 7;
                case 17: return 21;
                case 18: return 19;
                case 19: return 18;
                case 20: return 10;
                case 21: return 22;
                case 22: return 23;
                case 23: return 11;
                default: return -1;
            }
        }
    }
}
