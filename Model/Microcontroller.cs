namespace Multimetro1_0_2.Model
{

    public class MicroController
    {

        private byte adc_BitsResolution;
        public byte ADC_BitsResolution
        {
            get { return adc_BitsResolution; }
            set
            {
                if (value == adc_BitsResolution)
                {
                    adc_BitsResolution = value;
                }
            }
        }
        private double vRef;
        public double VRef
        {
            get { return vRef; }
            set
            {
                if (value == vRef)
                {
                    vRef = value;
                }
            }
        }
        private string nameMicroController;
        public string NameMicroController
        {
            get
            {
                return nameMicroController;
            }
            private set
            {
                if (nameMicroController != value)
                {
                    nameMicroController = value;
                }
            }
        }
        private Driver microPort;
        public Driver MicroPort
        {
            get { return microPort; }
        }
        public MicroController(int baudRate,string nameMicrocontroller,string deviceName,TypeConnection typeConnection, double vRef, byte adc_BitsResolution)
        {
            this.nameMicroController = nameMicrocontroller;
            this.vRef = vRef;
            this.adc_BitsResolution = adc_BitsResolution;
            if (typeConnection == TypeConnection.USB)
            {
#if WINDOWS
                this.microPort = new(new USB(baudRate, deviceName));
#endif
            }

        }

    }
    internal class Arduino : MicroController
    {
        public Arduino(int baudRate, string nameMicrocontroller, string deviceName, TypeConnection typeConnection, double vRef, byte adc_BitsResolution):base(baudRate,nameMicrocontroller,deviceName,typeConnection,vRef,adc_BitsResolution)
        {

        }
    }
    internal class RaspberryPi : MicroController
    {
        public RaspberryPi(int baudRate, string nameMicrocontroller, string deviceName, TypeConnection typeConnection, double vRef, byte adc_BitsResolution) : base(baudRate, nameMicrocontroller, deviceName, typeConnection, vRef, adc_BitsResolution)
        {

        }
    }
    internal class ESP8266 : MicroController
    {
        public ESP8266(int baudRate, string nameMicrocontroller, string deviceName, TypeConnection typeConnection, double vRef, byte adc_BitsResolution) : base(baudRate, nameMicrocontroller, deviceName, typeConnection, vRef, adc_BitsResolution)
        {

        }
    }
}
