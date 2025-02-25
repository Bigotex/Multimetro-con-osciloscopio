#if WINDOWS
using System.Diagnostics;
using System.IO.Ports;
#endif
#if ANDROID
using Android.Content;
using Android.Hardware.Usb;
using Android.App;
using Application = Android.App.Application;
#endif
using System.Timers;

namespace Multimetro1_0_2.Model
{
    public enum TypeConnection
    {
        USB,
        Bluetooth,
        WIFI 
    }

    public class Driver : IConnection
    {
        private object connection;
        public int BaudRate { get; set; }
        public string Id { get; set; }

        private System.Timers.Timer timer;
        public int SampleRate_relative
        {
            get
            {
                return sampleRate_relative;
            }
            set
            {
                if (value != sampleRate_relative)
                {
                    sampleRate_relative = value;
                    double time = (double)(value / (BaudRate / 8));//Obtenemos el tiempo que se tarda en recibir datos en segundos.
                    time = time * Math.Pow(10, 3); //Convertimos a milisegundos
                    timer.Interval = time;
                }
            }
        }

        private int sampleRate_relative;

        public event EventHandler<Reading_EventArgs> DataSerial_Received;

        public Driver(object connection)
        {
            this.connection = connection;
            timer = new(1000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                BaudRate = usb.BaudRate;
            }
            if(connection.GetType() == typeof(Bluetooth))
            {

            }
            if(connection.GetType()==typeof(WiFi))
            {

            }
            SampleRate_relative = BaudRate / 8;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (BytesToRead() > SampleRate_relative)
            {
                DataSerial_Received?.Invoke(this, new([]));
            }
            else
            {
                DataSerial_Received?.Invoke(this, new([]));
            }
        }
        #region Write
        public void Write(string data)
        {
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                usb.Write(data);
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                throw new NotImplementedException();
            }
        }
        public void Write(int dataOut)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] dataOut)
        {
            throw new NotImplementedException();
        }
        #endregion
        public char ReadChar()
        {
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                return usb.ReadChar();
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                Bluetooth bluetooth = (Bluetooth)connection;
                throw new NotImplementedException();

            }
            throw new NotImplementedException();

        }
        public static string[] GetDevicesAvailables(TypeConnection typeConnection)
        {
            if (typeConnection == TypeConnection.USB)
            {
                return USB.GetDevicesAvailables();
            }
            throw new NotImplementedException();
        }
        public bool Connect()
        {
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                if (usb.Connect())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                //Bluetooth bluetooth = (Bluetooth)connection;
                throw new NotImplementedException();
            }
            return false;
        }

        public void DiscardInBuffer()
        {
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                usb.DiscardInBuffer();
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                Bluetooth bluetooth = (Bluetooth)connection;
            }
        }
        public byte[] ReadBuffer(int position=0,int nbytes = -1)
        {
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                return usb.ReadBuffer(position,nbytes);
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                Bluetooth bluetooth = (Bluetooth)connection;
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        public void StartEventRead()
        {
            if (connection.GetType() == typeof(USB))
            {
                USB usb = (USB)connection;
                usb.DiscardInBuffer();
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                Bluetooth bluetooth = (Bluetooth)connection;
                throw new NotImplementedException();
            }
            timer.Start();

        }

        public void StopEventRead()
        {
            timer.Stop();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns> número de bytes disponibles en el Buffer2 de entrada</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int BytesToRead()
        {
            if (connection.GetType() == typeof(USB))
            {
                var usb = (USB)connection;
                return usb.BytesToRead();
            }
            else if (connection.GetType() == typeof(Bluetooth))
            {
                Bluetooth bluetooth = (Bluetooth)connection;
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }


    }

    public class USB :

#if ANDROID
        BroadcastReceiver, IConnection
#else
        IConnection
#endif
    {
        public event EventHandler<Reading_EventArgs>? DataSerial_Received;
        public int BaudRate { get; set; }
        public string Id { get; set; }

#if WINDOWS
        #region Windows  
        public SerialPort port;

        public USB(int baudRate, string id)
        {
            port = new SerialPort();
            port.BaudRate = baudRate;
            BaudRate = baudRate;
            port.PortName = id;
            Id = id;
            port.Parity = Parity.None;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.DataReceived += Port_DataReceived;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DataSerial_Received?.Invoke(this, new(""));
        }

        public bool Connect()
        {
            try
            {
                port.Open();
                Debug.Print("Se pudo establecer una conexión exitosa con el puerto serial");
                Debug.WriteLine(" ");
                return true;
            }
            catch (Exception)
            {
                Debug.Print("No se pudo establecer una conexión con el puerto serial");
                Debug.WriteLine(" ");
                return false;
            }
        }
        #region Write
        public void Write(string dataOut)
        {
            port.Write(dataOut);
        }
        public void Write(int dataOut)
        {
            port.Write(dataOut.ToString());

        }

        public void Write(byte[] dataOut)
        {
            if(dataOut.Length!=0)
            {
               port.Write(dataOut, 0, dataOut.Length-1);
            }
        }
        #endregion
        public char ReadChar()
        {
            return (char)port.ReadChar();
        }

        public void DiscardInBuffer()
        {
            port.DiscardInBuffer();
        }
        public static string[] GetDevicesAvailables()
        {
            return SerialPort.GetPortNames();
        }
        public byte[] ReadBuffer(int position=0,int nbytes = -1)
        {
            byte[] buffer;
            if (nbytes == -1)
            {
                buffer = new byte[port.BytesToRead-position];
                port.Read(buffer, position, port.BytesToRead);
                return buffer;
            }
            buffer = new byte[port.BytesToRead - position];
            port.Read(buffer, position, nbytes);
            return buffer;

        }
        public int BytesToRead()
        {
            return port.BytesToRead;
        }



        #endregion
#elif ANDROID
        #region Android
        private Android.Hardware.Usb.UsbManager usbManager;
        private Android.App.Application application;
        public USB(int baudRate, string id)
        {
            usbManager = (UsbManager)application.GetSystemService(Context.UsbService);
        }
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == UsbManager.ActionUsbDeviceAttached)
            {

                // Aquí puedes manejar la conexión del dispositivo
            }
        }
        public bool Connect()
        {
            throw new NotImplementedException();
        }
        public void Write(string dataOut)
        {
            throw new NotImplementedException();
        }
        public char ReadChar()
        {
            throw new NotImplementedException();
        }
        public void DiscardInBuffer()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBuffer(int position=0,int nbytes = -1)
        {
            throw new NotImplementedException();
        }
        public int BytesToRead()
        {
            throw new NotImplementedException();
        }
        public static string[] GetDevicesAvailables()
        {
            throw new NotImplementedException();
        }

        public void Write(int dataOut)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] dataOut)
        {
            throw new NotImplementedException();
        }
        #endregion
#else
        #region AnyPlatforms
        public bool Connect()
        {
            throw new NotImplementedException();
        }
        public void Write(string dataOut)
        {
            throw new NotImplementedException();
        }
        public char ReadChar()
        {
            throw new NotImplementedException();
        }
        public void DiscardInBuffer()
        {
            throw new NotImplementedException();
        }

        public int BytesToRead()
        {
            throw new NotImplementedException();
        }
        public static string[] GetDevicesAvailables()
        {
            throw new NotImplementedException();
        }

        public void Write(int dataOut)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] dataOut)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBuffer(int position = 0, int nbytes = -1)
        {
            throw new NotImplementedException();
        }
        #endregion

#endif
    }
    public class WiFi : IConnection
    {
        public event EventHandler<Reading_EventArgs>? DataSerial_Received;
        public int BaudRate { get; set; }
        public string Id { get; set; }

        public WiFi(int baudRate, string id)
        {

        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }
        #region Write
        public void Write(string dataOut)
        {
            throw new NotImplementedException();
        }
        public void Write(int dataOut)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] dataOut)
        {
            throw new NotImplementedException();
        }
        #endregion
        public char ReadChar()
        {
            throw new NotImplementedException();
        }

        public void DiscardInBuffer()
        {
            throw new NotImplementedException();
        }
        public static string[] GetDevicesAvailables()
        {
            throw new NotImplementedException();
        }
        public byte[] ReadBuffer(int position=0,int nbytes = -1)
        {
            throw new NotImplementedException();

        }
        public int BytesToRead()
        {
            throw new NotImplementedException();
        }


    }
    public class Bluetooth : IConnection
    {
        public event EventHandler<Reading_EventArgs>? DataSerial_Received;
        public int BaudRate { get; set; }
        public string Id { get; set; }

        public Bluetooth(int baudRate, string id)
        {

        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }
        #region Write
        public void Write(string dataOut)
        {
            throw new NotImplementedException();
        }
        public void Write(int dataOut)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] dataOut)
        {
            throw new NotImplementedException();
        }
        #endregion
        public char ReadChar()
        {
            throw new NotImplementedException();
        }

        public void DiscardInBuffer()
        {
            throw new NotImplementedException();
        }
        public static string[] GetDevicesAvailables()
        {
            throw new NotImplementedException();
        }
        public byte[] ReadBuffer(int position=0,int nbytes = -1)
        {
            throw new NotImplementedException();

        }
        public int BytesToRead()
        {
            throw new NotImplementedException();
        }


    }
}




