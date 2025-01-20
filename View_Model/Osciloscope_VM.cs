using Multimetro1_0_2.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Multimetro1_0_2.View_Model
{
    internal class Osciloscope_VM : Notification

    {
        private Osciloscope osciloscope;
        public ICommand ExportExcel_Command { get; set; }
        /// <summary>
        /// Propiedad que se enlaza a la gráfica de la interfaz de usuario
        /// </summary>
        public ObservableCollection<Measure> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Measure> data;

        private ObservableCollection<Measure> sample;
        private int valueX;
        /// <summary>
        ///  Propiedad que está enlazada a un slider de la interfaz de usuario que permite el ajuste de la gráfica en el eje X
        /// </summary>
        public int ValueX
        {
            get
            {

                return valueX;
            }
            set
            {
                valueX = value;
                osciloscope.SetSampleRate(osciloscope.Driver.BaudRate / (8 * valueX));
                OnPropertyChanged();
                //mathPlot.PointsXY.Capacity= (int)ValueX;
            }
        }
        private int valueY;
        /// <summary>
        ///  Propiedad que está enlazada a un slider de la interfaz de usuario que permite el ajuste de la gráfica en el eje Y
        /// </summary>
        public int ValueY
        {
            get
            {
                return valueY;
            }
            set
            {
                valueY = value;
                OnPropertyChanged();
            }
        }

        public Osciloscope_VM(Osciloscope osciloscope)
        {
            this.osciloscope = osciloscope;
            ExportExcel_Command = new Command(Send_to_Excel);
            Data = [];
            sample = [];
            ValueX = 1;
        }
        public void AddSample(ObservableCollection<Measure> samples)
        {
            foreach (var item in samples)
            {
                sample.Add(item);
            }

        }
        public void ControlRead(bool control)
        {
            if (control)
            {
                sample.Clear();
                osciloscope.StartMeasuring();
                //if (Application.Current != null)
                //{
                //    Application.Current.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
                //    {

                //        if (canStopTimer) return false;
                //        if (samples.Count == 0) return true;

                //        Data = samples[0];
                //        samples.RemoveAt(0);

                //        return true;
                //    });
                //}
                return;
            }
            //canStopTimer = true;
            osciloscope.StopMeasuring();

        }
        private void Send_to_Excel()
        {
            int n = sample.Count;
            string[,] data = new string[n + 1, 2];
            data[0, 0] = "Voltaje";
            data[0, 1] = "Tiempo";
            for (int i = 1; i < n; i++)
            {
                data[i, 0] = sample[i].Voltage.ToString();
                data[i, 1] = sample[i].Time.ToString();
            }


            Excel.CreateTable(data, "Signal");
        }
    }
}
