

namespace Multimetro1_0_2.View_Model
{
    internal class Measure : Notification
    {
        public ulong Time
        {
            get
            {
                return time;
            }
            private set
            {
                if (value != time)
                {
                    time = value;
                    OnPropertyChanged();
                }
            }
        }
        private ulong time;
        public float Voltage
        {
            get
            {
                return voltage;
            }
            private set
            {
                if (value != voltage)
                {
                    voltage = value;
                    OnPropertyChanged();
                }
            }
        }
        private float voltage;
        public Measure(float voltage, ulong time)
        {
            Voltage = voltage;
            Time = time;
        }
    }
}
