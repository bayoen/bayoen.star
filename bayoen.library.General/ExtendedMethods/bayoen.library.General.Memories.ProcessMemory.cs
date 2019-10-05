using System;
using System.Text;

using bayoen.library.General.Memories;

namespace bayoen.library.General.ExtendedMethods
{
    public static partial class ExtendedMethods
    {
        public static string ReadValidString(this ProcessMemory pm, IntPtr pOffset, uint pSize, params long[] offsets)
        {
            string output = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(pm.ReadStringUnicode(pOffset, pSize, offsets))
                )
            );

            int escapeIndex = output.IndexOf("\u0000");
            if (escapeIndex > -1) output = output.Remove(escapeIndex);

            return output;
        }

        public static bool ReadBinary(this ProcessMemory pm, int index, IntPtr pOffset) => pm.ReadBinary(index, pOffset, new long[] { });
        public static bool ReadBinary(this ProcessMemory pm, int index, IntPtr pOffset, params long[] offsets) => (pm.ReadByte(pm.Traverse(pOffset, offsets)) & (1 << index)) != 0;
    }
}
