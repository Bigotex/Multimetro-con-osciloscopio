namespace Multimetro1_0_2.Model
{

    public partial class MicroController
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
        private Driver microPort;
        public Driver MicroPort
        {
            get { return microPort; }
        }
        public MicroController(Driver driver, double vRef, byte adc_BitsResolution)
        {
            this.microPort = driver;
            this.vRef = vRef;
            this.adc_BitsResolution = adc_BitsResolution;
        }

    }
    internal class Arduino : MicroController
    {
        public Arduino(Driver arduPort, double vRef) : base(arduPort, vRef, 10)
        {

        }
    }
    internal class RaspberryPi
    {

    }
    internal class ESP8266
    {

    }
}
