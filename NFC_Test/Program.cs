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
using Gadgeteer.Modules.Luca_Sasselli;

namespace Test
{
    public partial class Program
    {
        Adafruit_PN532 nfc;
        
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            nfc = new Adafruit_PN532(6);
            nfc.Init();

            // Print version
            uint version = nfc.GetFirmwareVersion();
            Debug.Print("NFC ver. " + version.ToString());

            nfc.TagFound += TagFound;
            nfc.StartScan(1000, 100);

        }

        // Tag Found event
        void TagFound(string uid)
        {
            nfc.StopScan();
            Debug.Print("TAG! " + uid);

            Debug.Print("Starting dummy operation...");

            // Perform dummy operation...
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(20);
            }

            Debug.Print("Dummy operation completed! Resuming scan...");

            nfc.StartScan(1000, 100);
        }
    }
}
