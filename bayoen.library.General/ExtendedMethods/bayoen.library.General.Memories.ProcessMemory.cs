using System;

using bayoen.library.General.Memories;

namespace bayoen.library.General.ExtendedMethods
{
    public static partial class ExtendedMethods
    {
        public static string ReadValidString(this ProcessMemory pm, IntPtr pOffset, uint pSize, params long[] offsets)
        {
            string tempString = pm.ReadStringUnicode(pOffset, pSize, offsets);

            int escapeIndex = tempString.IndexOf("\u0000");
            if (escapeIndex > -1) tempString = tempString.Remove(escapeIndex);

            escapeIndex = tempString.IndexOf("\\u");
            if (escapeIndex > -1) tempString = tempString.Remove(escapeIndex);

            return tempString;
        }

        public static bool ReadBinary(this ProcessMemory pm, int index, IntPtr pOffset) => pm.ReadBinary(index, pOffset, new long[] { });
        public static bool ReadBinary(this ProcessMemory pm, int index, IntPtr pOffset, params long[] offsets) => (pm.ReadByte(pm.Traverse(pOffset, offsets)) & (1 << index)) != 0;
    }
}
