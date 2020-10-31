namespace WpfEssentials.SampleApp
{
    public class MainViewModel : BaseDialogViewModel
    { 
        public RelayCommand OpenSecondWindowCommand { get; }
        
        public RelayCommand OpenDialogWindowCommand { get; }
        
        public RelayCommand OpenFileCommand { get; }
        
        public RelayCommand OpenDirectoryCommand { get; }
        
        public RelayCommand<string> CommandWithParameter { get; }

        public string SomeProperty
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public override string Title => "WPF Essentials Demo Application";

        public MainViewModel()
        {
            OpenSecondWindowCommand = new RelayCommand(OpenSecondWindowExecute);
            OpenDialogWindowCommand = new RelayCommand(OpenDialogWindowExecute);
            OpenFileCommand = new RelayCommand(OpenFileExecute);
            OpenDirectoryCommand = new RelayCommand(OpenDirectoryExecute);
            CommandWithParameter = new RelayCommand<string>(CommandWithParameterExecute);
            SomeProperty = "Initial value";
        }

        void OpenDirectoryExecute()
        {
            if (ApplicationService.ShowFolderBrowserDialog("Open a Directory") == System.Windows.Forms.DialogResult.OK)
                ApplicationService.ShowMessage(DialogType.Information, "Folder", ApplicationService.FolderPath);
        }

        void OpenFileExecute()
        {
            if (ApplicationService.ShowOpenFileDialog())
                ApplicationService.ShowMessage(DialogType.Information, "File", ApplicationService.FileName);
        }

        void CommandWithParameterExecute(string Parameter)
        {
            ApplicationService.ShowMessage(DialogType.Information, "Parameter", Parameter.ToString());
        }

        void OpenDialogWindowExecute()
        {
            var vm = ApplicationService.OpenWindow<TestDialogViewModel>(vm => vm.Initialize(SomeProperty), true);

            // Or as an alternative:
            //var vm = ApplicationService.OpenWindow<TestDialogViewModel>(vm => vm.MyDescription = SomeProperty, true);

            if (vm.DialogResult)
                SomeProperty = vm.MyDescription;
        }

        void OpenSecondWindowExecute()
        {
            ApplicationService.OpenWindow<SecondWindowViewModel>();
        }
    }
}
