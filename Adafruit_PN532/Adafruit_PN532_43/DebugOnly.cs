using System;
using System.Diagnostics;

using Microsoft.SPOT;

namespace Gadgeteer.Modules.Luca_Sasselli
{
    class DebugOnly
    {
        const string HEX_CHARS = "0123456789ABCDEF";

        [Conditional("DEBUG")]
        public static void Print(string text)
        {
            Microsoft.SPOT.Debug.Print(text);
        }

        [Conditional("DEBUG")]
        public static void PrintByteArray(byte[] array)
        {
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text += "0x" + ByteToHex(array[i]) + " "; 
            }
            Microsoft.SPOT.Debug.Print(text);
        }

        private static string ByteToHex(byte input)
        {
            string output = "";
            output += HEX_CHARS[input >> 4];
            output += HEX_CHARS[input & 0x0F];
            return output;
        }
    }
}
