using Multimetro1_0_2.Model;
using Multimetro1_0_2.View_Model;
namespace Multimetro1_0_2.View;

public partial class MainPage : ContentPage
{
    MainPage_VM mainPage_VM;
    MicroController microController;
    public MainPage()
    {
        InitializeComponent();
        mainPage_VM = new();
        this.BindingContext = mainPage_VM;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (Option1.IsChecked)
        {
            switch (mainPage_VM.NameMicrocontroller)
            {

                case "Arduino_Uno":
                    {
                        microController = new(mainPage_VM.BaudRate,mainPage_VM.NameMicrocontroller,mainPage_VM.DeviceName,TypeConnection.USB, 5, 10);
                        break;
                    }
                case "Raspberry Pi Pico":
                    {
                        break;
                    }

            }
        }
        if (microController.MicroPort.Connect())
        {
            Navigation.PushAsync(new MenuPage(microController));
        };



    }

    private void Option1_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        mainPage_VM.ArrayPorts = Driver.GetDevicesAvailables(TypeConnection.USB);
    }
}

