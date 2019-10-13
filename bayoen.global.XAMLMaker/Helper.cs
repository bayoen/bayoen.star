using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.global.XAMLMaker
{
    public class Helper
    {
        public static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static int AlphabetToNumber(string alpha) => char.Parse(alpha) - 64;
        public static string NumberToAlphabet(int num) => char.ToString((char)(64 + num));
    }
}
