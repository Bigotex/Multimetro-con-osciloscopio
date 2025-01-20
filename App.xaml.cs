namespace Multimetro1_0_2
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzY4MjM5NkAzMjM4MmUzMDJlMzBlOFRPVVZsQnZ1T3RDVWpOZ3JSNTV2UzFkYXlLZ3BiOVZPQkVNZFEzL1QwPQ==");
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}