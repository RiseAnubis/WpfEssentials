using System.Windows;

namespace WpfEssentials.SampleApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            testControl.SomethingExecuted += TestControl_SomethingExecuted;
        }

        void TestControl_SomethingExecuted(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Executed event fired!");
        }
    }
}
