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
        Adafruit_PN532 nfc;
        GT.Timer timer;
        
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            nfc = new Adafruit_PN532(6);
            nfc.Start();
            uint version = nfc.GetFirmwareVersion();
            version = (version >> 8) & 0xFF;
            Debug.Print("NFC ver. " + version.ToString());

            nfc.SAMConfig();

            timer = new GT.Timer(300); 
            timer.Tick += NfcScan; 
            timer.Start(); 
        }

        void NfcScan(GT.Timer timer)
        {
            byte[] uid = nfc.ReadPassiveTargetID(Adafruit_PN532.PN532_MIFARE_ISO14443A, 1000);
            if (uid != null) timer.Stop();
        }
    }
}
