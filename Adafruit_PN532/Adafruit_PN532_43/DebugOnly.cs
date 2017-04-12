using System.Diagnostics;

namespace Gadgeteer.Modules.Luca_Sasselli
{
    internal static class DebugOnly
    {
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
                text += "0x" + Common.ByteToHex(array[i]) + " "; 
            }
            Microsoft.SPOT.Debug.Print(text);
        }
    }
}
