namespace Multimetro1_0_2.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public enum TypeInstrument
    {
        Capacimeter,
        Voltimeter,
        Osciloscope,
        Amperimeter
    }
    public abstract class Instrument
    {
        public const string keyRead = "Osciloscope";
        private Driver port;
        /// <summary>
        /// Manejador de eventos para el envío de mediciones individuales;
        /// </summary>
        public virtual event EventHandler<MeasureEventArgs> Measure_Detected;

        protected MicroController microController;
        protected virtual void On_New_Measure((float, ulong) measure)
        {
            MeasureEventArgs args = new(measure.Item1, measure.Item2);
            Measure_Detected?.Invoke(this, args);
        }
        public Driver Driver
        {
            get { return port; }
        }
        public Instrument(MicroController? microController)
        {
            this.microController = microController;
            port = microController.MicroPort;
        }

        public abstract void StartMeasurement();

        public abstract void StopMeasurement();

        protected abstract Task MeasurementAsync(Reading_EventArgs e);

    }

    public class Osciloscope : Instrument
    {

        /// <summary>
        /// Manejador de eventos para el envío de mediciones de una muestra de señales
        /// </summary>
        public event EventHandler<MeasureSignalEventArgs> Sample_Detected;
        private ulong correction; // factor de correción de desplazamiento del tiempo al iniciar la lectura.
        private float convertion; // factor de conversión de voltaje [v]

        public Osciloscope(MicroController microController) : base(microController)
        {
            convertion = (float)(microController.VRef / Math.Pow(2, microController.ADC_BitsResolution));
            Driver.Write(keyRead);
            Buffer2 buffer = new(Driver.ReadBuffer());
            foreach (string newline in buffer)
            {
                Task<(float, ulong)> taskMeausure = Measure(newline);
                taskMeausure.Wait();
                (float, ulong) values = taskMeausure.Result;
                if (values.Item2 == 0)
                {
                    continue;
                }
                else
                {
                    correction = values.Item2;
                    break;
                }

            }

        }
        /// <summary>
        /// Método que establece la frecuencia de muestreo en bytes/s.
        /// </summary>
        public void SetSampleRate(int sampleRate)
        {
            Driver.SampleRate_relative = sampleRate;
        }

        private void On_New_Sample((float, ulong)[] sample)
        {
            MeasureSignalEventArgs args = new(sample);
            Sample_Detected?.Invoke(this, args);
        }

        public override void StartMeasurement()
        {
            Driver.DiscardInBuffer(); 
            Driver.Write(keyRead);
            Driver.DataSerial_Received += ReadMeasurementAsync;
            Driver.StartEventRead();
        }
        public override void StopMeasurement()
        {
            try
            {
                Driver.DataSerial_Received -= ReadMeasurementAsync;
                Driver.StopEventRead();
            }
            catch (Exception)
            {

            }
        }

        //ReadMeasures al ser un método que ejecuta tareas asíncronas para la medición de datos,
        //permitirá aumentar la velocidad de medición de datos para no retrasar
        //las llegadas de la información en el buffer del puerto del microcontrolador.
        /// <summary>
        /// Método que ejecuta tareas asíncronas para la medición de datos.
        /// </summary>

        private async void ReadMeasurementAsync(object? obj, Reading_EventArgs e)
        {
            (float, ulong)[] results = await MeasurementAsync(e);
            On_New_Sample(results);
        }

        /// <summary>
        /// Método que mide las señales que llegan del puerto de manera óptima mediante procesos asíncronos.
        /// </summary>
        /// <param name="e">Argumento en el que contiene un buffer con interfaces de enumeración.{IEnumerator,IEnumerable}</param>
        /// <returns>TResult de Array de tuplas del tipo (float,long)</returns>

        protected override async Task<(float, ulong)[]> MeasurementAsync(Reading_EventArgs e) 
        {
            List<Task<(float, ulong)>> taskMeasures = [];
            foreach (string newLine in e.CurrentBuffer)
            {
                Task<(float, ulong)> taskMeausure = Measure(newLine);
                taskMeasures.Add(taskMeausure);
            }
            List<(float, ulong)> results = [];
            //Filtrado de lecturas erróneas
            for (int i = 0; i < taskMeasures.Count - 1; i++)
            {
                (float, ulong) result = await taskMeasures[i];
                float adc = result.Item1;
                ulong time = result.Item2;
                if (time == 0)
                {
                    continue;
                }
                results.Add((adc, time - correction));
            }

            return [.. results];
        }
        private async Task<(float, ulong)> Measure(string data)
        {
            long time = 0;
            float value = 0;
            bool finishedMeasure = false;
            bool flag1 = true;
            bool flag2 = false;
            bool isNegative = false;
            foreach (char character in data)
            {
                if (finishedMeasure)
                {
                    if (isNegative)
                    {
                        return (-value * convertion, (ulong)time);
                    }
                    else
                    {
                        return (value * convertion, (ulong)time);
                    }

                }
                if (character == '-')
                {
                    isNegative = true;
                }
                if (character == ',')
                {
                    flag1 = false;
                    flag2 = true;
                }
                if (character == '.')
                {
                    flag1 = false;
                    flag2 = false;
                    finishedMeasure = true;
                }
                if (byte.TryParse(character.ToString(), out byte digit))
                {
                    if (flag1)
                    {
                        value *= 10;
                        value += digit;
                    }
                    if (flag2)
                    {
                        time *= 10;
                        time += digit;
                    }
                }
            }
            return (0, 0);
        }

    }


}
