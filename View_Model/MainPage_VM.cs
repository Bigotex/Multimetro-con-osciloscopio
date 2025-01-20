using Multimetro1_0_2.Model;



namespace Multimetro1_0_2.View_Model
{
    public class MainPage_VM : Notification
    {
        public MicroController MicroController { get; set; }
        public int[] Array_Baudrate_Range { get; set; }

        private string[] array_Microcontroller;
        public string[] Array_Microcontroller
        {
            get
            {
                return array_Microcontroller;
            }
            set
            {
                if (array_Microcontroller != value)
                {
                    array_Microcontroller = value;
                    OnPropertyChanged();
                }
            }
        }


        private string[] arrayPorts;
        public string[]? ArrayPorts
        {
            get
            {
                return arrayPorts;
            }
            set
            {
                if (arrayPorts != value)
                {
                    arrayPorts = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NameMicrocontroller { get; set; }
        public string DeviceName { get; set; }
        public int BaudRate { get; set; }
        //private Port port;


        public MainPage_VM()
        {
            Array_Baudrate_Range = [300, 600, 750, 1200, 9600, 115200, 921600];
            Array_Microcontroller = ["Arduino_Uno", "Raspberry_Pi_Pico", "c", "d", "e"];
        }


    }

}
