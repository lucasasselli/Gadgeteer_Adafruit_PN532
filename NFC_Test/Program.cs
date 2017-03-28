using Gadgeteer.Modules.GHIElectronics;
using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Hardware;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

// Added reference to version 43
// TODO: Check if installer generated works fine.
using Gadgeteer.Modules.Luca_Sasselli;

namespace NFC_Test
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            Adafruit_PN532 nfc = new Adafruit_PN532(6);
            nfc.Start();
            uint version = nfc.GetFirmwareVersion();

            Debug.Print("NFC ver. " + version.ToString());
        }
    }
}
