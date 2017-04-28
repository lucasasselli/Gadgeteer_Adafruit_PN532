using System.Threading;
using Gadgeteer.Modules.Luca_Sasselli;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Test
{
    public partial class Program
    {
        private Adafruit_PN532 nfc;

        // This method is run when the mainboard is powered up or reset.   
        private void ProgramStarted()
        {
            Debug.Print("Program Started");

            nfc = new Adafruit_PN532(6);
            nfc.Init();

            // Print version
            var version = nfc.GetFirmwareVersion();
            Debug.Print("NFC ver. " + version);

            nfc.TagFound += TagFound;
            nfc.Error += Error;
            nfc.StartScan(1000, 100);
        }

        // Tag Found event
        private void TagFound(string uid)
        {
            nfc.StopScan();
            Debug.Print("TAG! " + uid);

            Debug.Print("Starting dummy operation...");

            // Perform dummy operation...
            for (var i = 0; i < 100; i++)
                Thread.Sleep(20);

            Debug.Print("Dummy operation completed! Resuming scan...");

            nfc.StartScan(1000, 100);
        }

        // Error event
        private void Error()
        {
            Debug.Print("ERROR: NFC Not responding!");
        }
    }
}