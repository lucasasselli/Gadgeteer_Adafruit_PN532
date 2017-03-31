using System;
using Microsoft.SPOT;

namespace Gadgeteer.Modules.Luca_Sasselli
{
    class Common
    {
        const string HEX_CHARS = "0123456789ABCDEF";

        public static string ByteToHex(byte input)
        {
            string output = "";
            output += HEX_CHARS[input >> 4];
            output += HEX_CHARS[input & 0x0F];
            return output;
        }
    }
}
