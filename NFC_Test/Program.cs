using System.Threading;
using Gadgeteer.Modules.Luca_Sasselli;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Test
{
    public partial class Program
    {
        private Adafruit_PN532 _nfc;

        // This method is run when the mainboard is powered up or reset.   
        private void ProgramStarted()
        {
            Debug.Print("Program Started");

            _nfc = new Adafruit_PN532(6);
            _nfc.Init();

            // Print version
            var version = _nfc.GetFirmwareVersion();
            Debug.Print("NFC ver. " + version);

            _nfc.TagFound += TagFound;
            _nfc.StartScan(1000, 100);
        }

        // Tag Found event
        private void TagFound(string uid)
        {
            _nfc.StopScan();
            Debug.Print("TAG! " + uid);

            Debug.Print("Starting dummy operation...");

            // Perform dummy operation...
            for (var i = 0; i < 100; i++)
                Thread.Sleep(20);

            Debug.Print("Dummy operation completed! Resuming scan...");

            _nfc.StartScan(1000, 100);
        }
    }
}