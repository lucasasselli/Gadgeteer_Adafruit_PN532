using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.SocketInterfaces;

namespace Gadgeteer.Modules.Luca_Sasselli
{
    /// <summary>
    /// A module for Adafruit PN532 NFC for Microsoft .NET Gadgeteer
    /// </summary>
    public class Adafruit_PN532 : GTM.Module
    {
        //
        const bool DEBUG = true;

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
        const bool SPI_CS_ACTIVE_STATE = true;
        const uint SPI_CS_SETUP_TIME = 100;   // Milliseconds
        const uint SPI_CS_HOLD_TIME = 100;    // Milliseconds

        // Clock constants
        const bool SPI_CLK_IDLE_STATE = true;
        const bool SPI_CLK_EDGE_RISE = true;
        const uint SPI_CLK_FREQ = 100;

        // Line constants
        const byte SPI_NULL_BYTE = 0xFF;


        // Main objects
        SPI spi;

        // -- CHANGE FOR MICRO FRAMEWORK 4.2 and higher --
        // If you want to use Serial, SPI, or DaisyLink (which includes GTI.SoftwareI2C), you must do a few more steps
        // since these have been moved to separate assemblies for NETMF 4.2 (to reduce the minimum memory footprint of Gadgeteer)
        // 1) add a reference to the assembly (named Gadgeteer.[interfacename])
        // 2) in GadgeteerHardware.xml, uncomment the lines under <Assemblies> so that end user apps using this module also add a reference.

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <param name="chipSelect_port">Chip select port</param>
        /// <param name="spiModule">Spi module used</param>
        public Adafruit_PN532(Cpu.Pin chipSelect_port, SPI.SPI_module spiModule)
        {
            SPI.Configuration SpiConfig = new SPI.Configuration(
                chipSelect_port,
                SPI_CS_ACTIVE_STATE,
                SPI_CS_SETUP_TIME,
                SPI_CS_HOLD_TIME,
                SPI_CLK_IDLE_STATE, 
                SPI_CLK_EDGE_RISE,
                SPI_CLK_FREQ,
                spiModule);

            spi = new SPI(SpiConfig);

            byte[] tx_data = new byte[10];
            byte[] rx_data = new byte[10];
        }

        // ######################################
        // LOW LEVEL COMMUNICATION METHODS
        // ######################################

        private byte[] readdata(int n)
        {
            byte[] read = new byte[n];
            byte[] write = new byte[n];

            //Initialize all of the variables to spaces.
            for (int i = 0; i < n; i++)
            {
                write[i] = SPI_NULL_BYTE;
            }

            // SPI write.
            byte[] payload = { PN532_SPI_DATAREAD };

            spi.Write(payload);

            if (DEBUG)
                Debug.Print("Reading: ");

            spi.WriteRead(write, read);

            if (DEBUG)
            {

                for (int i = 0; i < n; i++)
                {

                    Debug.Print(" 0x");
                    Debug.Print(read[i].ToString());
                }
                Debug.Print("\n");
            }

            return read;
        }

        private void writecommand(byte cmd, byte cmdlen)
        {

        }

        private bool isready()
        {
            return true;
        }

        private bool waitready(int timeout)
        {

        }

        private bool readack()
        {

        }

        /*private void input_Interrupt(GTI.InterruptInput input, bool value)
        {
            this.OnButtonEvent(this, value ? ButtonState.Released : ButtonState.Pressed);
        }

        private GTI.InterruptInput input;

        /// <summary>
        /// Gets a value that indicates whether the button of the Adafruit_PN532 is pressed.
        /// </summary>
        public bool IsPressed
        {
            get
            {
                return this.input.Read();
            }
        }

        /// <summary>
        /// Represents the state of button of the <see cref="Adafruit_PN532"/>.
        /// </summary>
        public enum ButtonState
        {
            /// <summary>
            /// The button is released.
            /// </summary>
            Released = 0,
            /// <summary>
            /// The button is pressed.
            /// </summary>
            Pressed = 1
        }

        /// <summary>
        /// Represents the delegate that is used to handle the <see cref="ButtonPressed"/>
        /// and <see cref="ButtonReleased"/> events.
        /// </summary>
        /// <param name="sender">The <see cref="Adafruit_PN532"/> object that raised the event.</param>
        /// <param name="state">The state of the button of the <see cref="Adafruit_PN532"/></param>
        public delegate void ButtonEventHandler(Adafruit_PN532 sender, ButtonState state);

        /// <summary>
        /// Raised when the button of the <see cref="Adafruit_PN532"/> is pressed.
        /// </summary>
        /// <remarks>
        /// Implement this event handler and/or the <see cref="ButtonReleased"/> event handler
        /// when you want to provide an action associated with button events.
        /// Since the state of the button is passed to the <see cref="ButtonEventHandler"/> delegate,
        /// so you can use the same event handler for both button states.
        /// </remarks>
        public event ButtonEventHandler ButtonPressed;

        /// <summary>
        /// Raised when the button of the <see cref="Adafruit_PN532"/> is released.
        /// </summary>
        /// <remarks>
        /// Implement this event handler and/or the <see cref="ButtonPressed"/> event handler
        /// when you want to provide an action associated with button events.
        /// Since the state of the button is passed to the <see cref="ButtonEventHandler"/> delegate,
        /// you can use the same event handler for both button states.
        /// </remarks>
        public event ButtonEventHandler ButtonReleased;

        private ButtonEventHandler onButton;

        /// <summary>
        /// Raises the <see cref="ButtonPressed"/> or <see cref="ButtonReleased"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="Adafruit_PN532"/> that raised the event.</param>
        /// <param name="buttonState">The state of the button.</param>
        protected virtual void OnButtonEvent(Adafruit_PN532 sender, ButtonState buttonState)
        {
            if (this.onButton == null)
            {
                this.onButton = new ButtonEventHandler(this.OnButtonEvent);
            }

            if (buttonState == ButtonState.Pressed)
            {
                // Program.CheckAndInvoke helps event callers get onto the Dispatcher thread.  
                // If the event is null then it returns false.
                // If it is called while not on the Dispatcher thread, it returns false but also re-invokes this method on the Dispatcher.
                // If on the thread, it returns true so that the caller can execute the event.
                if (Program.CheckAndInvoke(ButtonPressed, this.onButton, sender, buttonState))
                {
                    this.ButtonPressed(sender, buttonState);
                }
            }
            else
            {
                if (Program.CheckAndInvoke(ButtonReleased, this.onButton, sender, buttonState))
                {
                    this.ButtonReleased(sender, buttonState);
                }
            }
        }*/
    }
}
