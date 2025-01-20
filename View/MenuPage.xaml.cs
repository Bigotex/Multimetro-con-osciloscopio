using Multimetro1_0_2.Model;
namespace Multimetro1_0_2.View;

public partial class MenuPage : ContentPage
{
    private readonly MicroController microController;
    public MenuPage(MicroController microController)
    {
        InitializeComponent();
        this.microController = microController;
    }

    private void OscBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new OsciloscopePage(microController));
    }

    private void SerialBtn_Clicked(object sender, EventArgs e)
    {
    }
}

