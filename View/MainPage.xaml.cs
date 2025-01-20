using Multimetro1_0_2.Model;
using Multimetro1_0_2.View_Model;
namespace Multimetro1_0_2.View;

public partial class MainPage : ContentPage
{
    MainPage_VM microcontroller_ViewModel = new();

    public MainPage()
    {
        InitializeComponent();
        this.BindingContext = microcontroller_ViewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (Option1.IsChecked)
        {
            switch (microcontroller_ViewModel.NameMicrocontroller)
            {

                case "Arduino_Uno":
                    {
#if WINDOWS
                        microcontroller_ViewModel.MicroController = new(new Driver(new USB(microcontroller_ViewModel.BaudRate, microcontroller_ViewModel.DeviceName)), 5, 10);
#endif

                        break;
                    }
                case "Raspberry Pi Pico":
                    {
                        break;
                    }

            }
        }
        if (microcontroller_ViewModel.MicroController.MicroPort.Connect())
        {
            Navigation.PushAsync(new MenuPage(microcontroller_ViewModel.MicroController));
        };



    }

    private void Option1_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        microcontroller_ViewModel.ArrayPorts = Driver.GetDevicesAvailables(Driver.usbConnection);
    }
}

