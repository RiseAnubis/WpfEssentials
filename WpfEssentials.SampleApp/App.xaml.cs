using System.Windows;

namespace WpfEssentials.SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var service = new ApplicationService();

            service
                .Register<MainViewModel, MainWindow>()
                .Register<SecondWindowViewModel, SecondWindow>()
                .Register<TestDialogViewModel, TestDialog>();

            service.OpenWindow<MainViewModel>();

            base.OnStartup(e);
        }
    }
}
