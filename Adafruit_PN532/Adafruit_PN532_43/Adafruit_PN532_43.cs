using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Gadgeteer.Modules.Luca_Sasselli
{
    /// <summary>
    /// A module for Adafruit PN532 NFC for Microsoft .NET Gadgeteer
    /// </summary>
    public class Adafruit_PN532 : GTM.Module
    {
        static byte[] BitReverseTable =
        {
            0x00, 0x80, 0x40, 0xc0, 0x20, 0xa0, 0x60, 0xe0,
            0x10, 0x90, 0x50, 0xd0, 0x30, 0xb0, 0x70, 0xf0,
            0x08, 0x88, 0x48, 0xc8, 0x28, 0xa8, 0x68, 0xe8,
            0x18, 0x98, 0x58, 0xd8, 0x38, 0xb8, 0x78, 0xf8,
            0x04, 0x84, 0x44, 0xc4, 0x24, 0xa4, 0x64, 0xe4,
            0x14, 0x94, 0x54, 0xd4, 0x34, 0xb4, 0x74, 0xf4,
            0x0c, 0x8c, 0x4c, 0xcc, 0x2c, 0xac, 0x6c, 0xec,
            0x1c, 0x9c, 0x5c, 0xdc, 0x3c, 0xbc, 0x7c, 0xfc,
            0x02, 0x82, 0x42, 0xc2, 0x22, 0xa2, 0x62, 0xe2,
            0x12, 0x92, 0x52, 0xd2, 0x32, 0xb2, 0x72, 0xf2,
            0x0a, 0x8a, 0x4a, 0xca, 0x2a, 0xaa, 0x6a, 0xea,
            0x1a, 0x9a, 0x5a, 0xda, 0x3a, 0xba, 0x7a, 0xfa,
            0x06, 0x86, 0x46, 0xc6, 0x26, 0xa6, 0x66, 0xe6,
            0x16, 0x96, 0x56, 0xd6, 0x36, 0xb6, 0x76, 0xf6,
            0x0e, 0x8e, 0x4e, 0xce, 0x2e, 0xae, 0x6e, 0xee,
            0x1e, 0x9e, 0x5e, 0xde, 0x3e, 0xbe, 0x7e, 0xfe,
            0x01, 0x81, 0x41, 0xc1, 0x21, 0xa1, 0x61, 0xe1,
            0x11, 0x91, 0x51, 0xd1, 0x31, 0xb1, 0x71, 0xf1,
            0x09, 0x89, 0x49, 0xc9, 0x29, 0xa9, 0x69, 0xe9,
            0x19, 0x99, 0x59, 0xd9, 0x39, 0xb9, 0x79, 0xf9,
            0x05, 0x85, 0x45, 0xc5, 0x25, 0xa5, 0x65, 0xe5,
            0x15, 0x95, 0x55, 0xd5, 0x35, 0xb5, 0x75, 0xf5,
            0x0d, 0x8d, 0x4d, 0xcd, 0x2d, 0xad, 0x6d, 0xed,
            0x1d, 0x9d, 0x5d, 0xdd, 0x3d, 0xbd, 0x7d, 0xfd,
            0x03, 0x83, 0x43, 0xc3, 0x23, 0xa3, 0x63, 0xe3,
            0x13, 0x93, 0x53, 0xd3, 0x33, 0xb3, 0x73, 0xf3,
            0x0b, 0x8b, 0x4b, 0xcb, 0x2b, 0xab, 0x6b, 0xeb,
            0x1b, 0x9b, 0x5b, 0xdb, 0x3b, 0xbb, 0x7b, 0xfb,
            0x07, 0x87, 0x47, 0xc7, 0x27, 0xa7, 0x67, 0xe7,
            0x17, 0x97, 0x57, 0xd7, 0x37, 0xb7, 0x77, 0xf7,
            0x0f, 0x8f, 0x4f, 0xcf, 0x2f, 0xaf, 0x6f, 0xef,
            0x1f, 0x9f, 0x5f, 0xdf, 0x3f, 0xbf, 0x7f, 0xff
        };

        // ######################################
        // PN532 CONSTANTS
        // ######################################
        
        const byte PN532_PREAMBLE = 0x00;
        const byte PN532_STARTCODE1 = 0x00;
        const byte PN532_STARTCODE2 = 0xFF;
        const byte PN532_POSTAMBLE = 0x00;

        const byte PN532_HOSTTOPN532 = 0xD4;
        const byte PN532_PN532TOHOST = 0xD5;

        // PN532 Commands
        const byte PN532_COMMAND_DIAGNOSE = 0x00;
        const byte PN532_COMMAND_GETFIRMWAREVERSION = 0x02;
        const byte PN532_COMMAND_GETGENERALSTATUS = 0x04;
        const byte PN532_COMMAND_READREGISTER = 0x06;
        const byte PN532_COMMAND_WRITEREGISTER = 0x08;
        const byte PN532_COMMAND_READGPIO = 0x0C;
        const byte PN532_COMMAND_WRITEGPIO = 0x0E;
        const byte PN532_COMMAND_SETSERIALBAUDRATE = 0x10;
        const byte PN532_COMMAND_SETPARAMETERS = 0x12;
        const byte PN532_COMMAND_SAMCONFIGURATION = 0x14;
        const byte PN532_COMMAND_POWERDOWN = 0x16;
        const byte PN532_COMMAND_RFCONFIGURATION = 0x32;
        const byte PN532_COMMAND_RFREGULATIONTEST = 0x58;
        const byte PN532_COMMAND_INJUMPFORDEP = 0x56;
        const byte PN532_COMMAND_INJUMPFORPSL = 0x46;
        const byte PN532_COMMAND_INLISTPASSIVETARGET = 0x4A;
        const byte PN532_COMMAND_INATR = 0x50;
        const byte PN532_COMMAND_INPSL = 0x4E;
        const byte PN532_COMMAND_INDATAEXCHANGE = 0x40;
        const byte PN532_COMMAND_INCOMMUNICATETHRU = 0x42;
        const byte PN532_COMMAND_INDESELECT = 0x44;
        const byte PN532_COMMAND_INRELEASE = 0x52;
        const byte PN532_COMMAND_INSELECT = 0x54;
        const byte PN532_COMMAND_INAUTOPOLL = 0x60;
        const byte PN532_COMMAND_TGINITASTARGET = 0x8C;
        const byte PN532_COMMAND_TGSETGENERALBYTES = 0x92;
        const byte PN532_COMMAND_TGGETDATA = 0x86;
        const byte PN532_COMMAND_TGSETDATA = 0x8E;
        const byte PN532_COMMAND_TGSETMETADATA = 0x94;
        const byte PN532_COMMAND_TGGETINITIATORCOMMAND = 0x88;
        const byte PN532_COMMAND_TGRESPONSETOINITIATOR = 0x90;
        const byte PN532_COMMAND_TGGETTARGETSTATUS = 0x8A;

        const byte PN532_RESPONSE_INDATAEXCHANGE = 0x41;
        const byte PN532_RESPONSE_INLISTPASSIVETARGET = 0x4B;

        const byte PN532_WAKEUP = 0x55;

        const byte PN532_SPI_STATREAD = 0x02;
        const byte PN532_SPI_DATAWRITE = 0x01;
        const byte PN532_SPI_DATAREAD = 0x03;
        const byte PN532_SPI_READY = 0x01;

        const byte PN532_I2C_ADDRESS = 0x48 >> 1;
        const byte PN532_I2C_READBIT = 0x01;
        const byte PN532_I2C_BUSY = 0x00;
        const byte PN532_I2C_READY = 0x01;
        const byte PN532_I2C_READYTIMEOUT = 20;

        const byte PN532_MIFARE_ISO14443A = 0x00;

        // Mifare Commands
        const byte MIFARE_CMD_AUTH_A = 0x60;
        const byte MIFARE_CMD_AUTH_B = 0x61;
        const byte MIFARE_CMD_READ = 0x30;
        const byte MIFARE_CMD_WRITE = 0xA0;
        const byte MIFARE_CMD_TRANSFER = 0xB0;
        const byte MIFARE_CMD_DECREMENT = 0xC0;
        const byte MIFARE_CMD_INCREMENT = 0xC1;
        const byte MIFARE_CMD_STORE = 0xC2;
        const byte MIFARE_ULTRALIGHT_CMD_WRITE = 0xA2;

        // Prefixes for NDEF Records to identify record type;
        const byte NDEF_URIPREFIX_NONE = 0x00;
        const byte NDEF_URIPREFIX_HTTP_WWWDOT = 0x01;
        const byte NDEF_URIPREFIX_HTTPS_WWWDOT = 0x02;
        const byte NDEF_URIPREFIX_HTTP = 0x03;
        const byte NDEF_URIPREFIX_HTTPS = 0x04;
        const byte NDEF_URIPREFIX_TEL = 0x05;
        const byte NDEF_URIPREFIX_MAILTO = 0x06;
        const byte NDEF_URIPREFIX_FTP_ANONAT = 0x07;
        const byte NDEF_URIPREFIX_FTP_FTPDOT = 0x08;
        const byte NDEF_URIPREFIX_FTPS = 0x09;
        const byte NDEF_URIPREFIX_SFTP = 0x0A;
        const byte NDEF_URIPREFIX_SMB = 0x0B;
        const byte NDEF_URIPREFIX_NFS = 0x0C;
        const byte NDEF_URIPREFIX_FTP = 0x0D;
        const byte NDEF_URIPREFIX_DAV = 0x0E;
        const byte NDEF_URIPREFIX_NEWS = 0x0F;
        const byte NDEF_URIPREFIX_TELNET = 0x10;
        const byte NDEF_URIPREFIX_IMAP = 0x11;
        const byte NDEF_URIPREFIX_RTSP = 0x12;
        const byte NDEF_URIPREFIX_URN = 0x13;
        const byte NDEF_URIPREFIX_POP = 0x14;
        const byte NDEF_URIPREFIX_SIP = 0x15;
        const byte NDEF_URIPREFIX_SIPS = 0x16;
        const byte NDEF_URIPREFIX_TFTP = 0x17;
        const byte NDEF_URIPREFIX_BTSPP = 0x18;
        const byte NDEF_URIPREFIX_BTL2CAP = 0x19;
        const byte NDEF_URIPREFIX_BTGOEP = 0x1A;
        const byte NDEF_URIPREFIX_TCPOBEX = 0x1B;
        const byte NDEF_URIPREFIX_IRDAOBEX = 0x1C;
        const byte NDEF_URIPREFIX_FILE = 0x1D;
        const byte NDEF_URIPREFIX_URN_EPC_ID = 0x1E;
        const byte NDEF_URIPREFIX_URN_EPC_TAG = 0x1F;
        const byte NDEF_URIPREFIX_URN_EPC_PAT = 0x20;
        const byte NDEF_URIPREFIX_URN_EPC_RAW = 0x21;
        const byte NDEF_URIPREFIX_URN_EPC = 0x22;
        const byte NDEF_URIPREFIX_URN_NFC = 0x23;

        const byte PN532_GPIO_VALIDATIONBIT = 0x80;
        const byte PN532_GPIO_P30 = 0;
        const byte PN532_GPIO_P31 = 1;
        const byte PN532_GPIO_P32 = 2;
        const byte PN532_GPIO_P33 = 3;
        const byte PN532_GPIO_P34 = 4;
        const byte PN532_GPIO_P35 = 5;

        // ######################################
        // SPI CONSTANTS
        // ######################################

        // Chip select constants
        const bool SPI_CS_ACTIVE_STATE = false;
        const uint SPI_CS_SETUP_TIME = 2;   // ms
        const uint SPI_CS_HOLD_TIME = 2;    // ms

        // Clock constants
        const bool SPI_CLK_IDLE_STATE = false;
        const bool SPI_CLK_EDGE_RISE = true;
        const uint SPI_CLK_FREQ = 1;        // kHz

        // Line constants
        const byte SPI_NULL_BYTE = 0x00;

        // Main objects
        SPI spi;

        // TODO: these are constants, but can't be created as such
        byte[] PN532_ACK = { 0x00, 0x00, 0xFF, 0x00, 0xFF, 0x00 };
        byte[] PN532_FV_HEAD = { 0x00, 0xFF, 0x06, 0xFA, 0xD5, 0x03 };

        // -- CHANGE FOR MICRO FRAMEWORK 4.2 and higher --
        // If you want to use Serial, SPI, or DaisyLink (which includes GTI.SoftwareI2C), you must do a few more steps
        // since these have been moved to separate assemblies for NETMF 4.2 (to reduce the minimum memory footprint of Gadgeteer)
        // 1) add a reference to the assembly (named Gadgeteer.[interfacename])
        // 2) in GadgeteerHardware.xml, uncomment the lines under <Assemblies> so that end user apps using this module also add a reference.

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <param name="socketNumber">Socket number</param>
        public Adafruit_PN532(int socketNumber)
        {
            // Get socket
            Socket socket = Socket.GetSocket(socketNumber, true, this, null);
            SPI.SPI_module spiModule = socket.SPIModule;
            Cpu.Pin chipSelectPort = socket.CpuPins[6];
           
            SPI.Configuration SpiConfig = new SPI.Configuration(
                chipSelectPort,
                SPI_CS_ACTIVE_STATE,
                SPI_CS_SETUP_TIME,
                SPI_CS_HOLD_TIME,
                SPI_CLK_IDLE_STATE, 
                SPI_CLK_EDGE_RISE,
                SPI_CLK_FREQ,
                spiModule);

            spi = new SPI(SpiConfig);
        }

        /// <summary>Initialize NFC module</summary>
        public void Start()
        {
            Debug.Print("Starting NFC module...");

            // This is needed for synchronization
            byte[] packet = {PN532_COMMAND_GETFIRMWAREVERSION};
            SendCommandCheckAck(packet);

            // Wait some time
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>Send command and check acknowledgment</summary>
        /// <param name="cmd">Command</param>
        /// <param name="timeout">Timeout (milliseconds, default = 1000)</param>
        public bool SendCommandCheckAck(byte[] cmd, uint timeout = 1000)
        {
            // Write the command
            writecommand(cmd);

            // Wait for chip to say its ready!
            if (!waitready(timeout))
            {
                return false;
            }

            // Read acknowledgement
            if (!readack()) {
                Debug.Print("No ACK frame received!");
                return false;
            }

            Debug.Print("OK!");

            return true;

        }

        /// <summary>Returns current firmware version</summary>
        public uint GetFirmwareVersion() {
            // Request firmware version
            byte[] packet = { PN532_COMMAND_GETFIRMWAREVERSION };
            if (!SendCommandCheckAck(packet))
            {
                return 0;
            }

            // Read response
            byte[] response = readdata(12);

            // Extract firmware version
            byte[] header = Utility.ExtractRangeFromArray(response, 0, 6);

            if (!CompareByteArray(header, PN532_FV_HEAD))
            {
                Debug.Print("Response is not firmware!");
            }

            return Utility.ExtractValueFromArray(response, 6, 4);
        }

        // ######################################
        // LOW LEVEL COMMUNICATION METHODS
        // ######################################

        private byte[] readdata(int n)
        {
            byte[] read = new byte[n+1];

            byte[] write = { PN532_SPI_DATAREAD };
            write = Utility.CombineArrays(write, CreateNullPayload(n));

            write = ReverseBit(write); // TODO

            spi.WriteRead(write, read);

            read = Utility.ExtractRangeFromArray(read, 1, n);
            read = ReverseBit(read); // TODO

            // Debug informations
            Debug.Print("\nReading: ");
            for (int i = 0; i < n; i++)
            {
                Debug.Print(read[i].ToString());
            }

            return read;
        }

        private void writecommand(byte[] cmd)
        {
            uint cmdlen = (uint) cmd.Length + 1;

            // Compute checksum
            int checksum = PN532_PREAMBLE + PN532_PREAMBLE + PN532_STARTCODE2 + PN532_HOSTTOPN532;

            for (uint i = 0; i < cmdlen - 1; i++)
            {
                checksum += cmd[i];
            }

            byte[] write_header = {
                                      PN532_SPI_DATAWRITE,
                                      PN532_PREAMBLE, 
                                      PN532_PREAMBLE,
                                      PN532_STARTCODE2,
                                      (byte) cmdlen,
                                      (byte) (~cmdlen + 1),
                                      PN532_HOSTTOPN532};

            byte[] write_footer = {
                                    (byte) (~checksum), 
                                    PN532_POSTAMBLE};

            // Create write array
            byte[] write = Utility.CombineArrays(write_header, cmd);
            write = Utility.CombineArrays(write, write_footer);

            write = ReverseBit(write); // TODO

            spi.Write(write);

            // Debug informations
            Debug.Print("\nSending: ");
            for (int i = 0; i < write.Length; i++)
            {

                Debug.Print(write[i].ToString());
            }

            Debug.Print("Checksum: " + checksum.ToString());
        }

        private bool readack()
        {
            byte[] read = readdata(6);

            return CompareByteArray(read, PN532_ACK);
        }

        private bool isready()
        {
            // SPI read status and check if ready.
            byte[] read = new byte[1];
            byte[] write = { PN532_SPI_STATREAD };

            write = ReverseBit(write); // TODO: more lightweigth solution

            spi.Write(write);
            spi.WriteRead(CreateNullPayload(1), read);

            read = ReverseBit(read); // TODO

            return read[0] == PN532_SPI_READY;
        }

        private bool waitready(uint timeout)
        {
            uint timer = 0;
            while (!isready())
            {
                if (timeout != 0)
                {
                    timer += 10;
                    if (timer > timeout)
                    {
                        Debug.Print("TIMEOUT!");
                        return false;
                    }
                }
                // TODO: Does this give access to other threads? Is it bad?
                System.Threading.Thread.Sleep(10);
            }

            Debug.Print("READY!");

            return true;
        }

        // My methods, they are probably bad.
        // TODO: Move to separate class

        private byte[] CreateNullPayload(int size)
        {
            byte[] output = new byte[size];

            //Initialize all of the variables to spaces.
            for (int i = 0; i < size; i++)
            {
                output[i] = SPI_NULL_BYTE;
            }

            return output;
        }

        private byte[] ReverseBit(byte[] input)
        {
            byte[] output = new byte[input.Length];

            for (uint i = 0; i < input.Length; i++)
            {
                output[i] = BitReverseTable[input[i]];
            }

            return output;
        }

        private bool CompareByteArray(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }
    }
}
