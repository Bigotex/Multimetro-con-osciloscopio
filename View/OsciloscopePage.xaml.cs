using Multimetro1_0_2.Model;
using Multimetro1_0_2.View_Model;

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Multimetro1_0_2.View
{
    public partial class OsciloscopePage : ContentPage

    {
        private Osciloscope_VM osciloscope_ViewModel;
        private Osciloscope osciloscope;
        public OsciloscopePage(MicroController microController)
        {
            InitializeComponent();
            osciloscope = new(microController);
            osciloscope_ViewModel = new Osciloscope_VM(osciloscope);
            BindingContext = osciloscope_ViewModel;
            osciloscope.Sample_Detected += Osciloscope_Sample_Received;
        }

        private void Osciloscope_Sample_Received(object? sender, MeasureSignalEventArgs e)
        {

            ObservableCollection<Measure> measures = [];
            Debug.WriteLine("Mediciones {0}", e.Sample.Length);
            foreach (var measure in e.Sample)
            {
                measures.Add(new(measure.Item1, measure.Item2));
            }
            osciloscope_ViewModel.Data = measures;
            osciloscope_ViewModel.AddSample(measures);
            //components=SignalsProcessor.Get_Components(signal, signal.Count);

        }

        private void Start_Toggled(object sender, ToggledEventArgs e)
        {
            osciloscope_ViewModel.ControlRead(Switch_enabler.IsToggled);
        }
    }
}