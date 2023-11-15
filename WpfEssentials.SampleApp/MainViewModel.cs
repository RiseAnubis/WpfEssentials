using System.ComponentModel;

namespace WpfEssentials.SampleApp;

public class MainViewModel : BaseDialogViewModel
{ 
    public RelayCommand OpenParameterWindowCommand { get; }
        
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

    public string SomeText
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string SomeInformation
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public SomeEnum EnumValue
    {
        get => GetProperty<SomeEnum>();
        set => SetProperty(value);
    }

    public override string Title => "WPF Essentials Demo Application";

    public MainViewModel()
    {
        OpenParameterWindowCommand = new RelayCommand(OpenParameterWindowExecute);
        OpenSecondWindowCommand = new RelayCommand(OpenSecondWindowExecute);
        OpenDialogWindowCommand = new RelayCommand(OpenDialogWindowExecute);
        OpenFileCommand = new RelayCommand(OpenFileExecute);
        OpenDirectoryCommand = new RelayCommand(OpenDirectoryExecute);
        CommandWithParameter = new RelayCommand<string>(CommandWithParameterExecute);
        SomeProperty = "Initial value";
    }

    void OpenParameterWindowExecute()
    {
        ApplicationService.OpenWindow(() => new ParameterViewModel("Initial text", 11));
    }

    void OpenDirectoryExecute()
    {
        if (ApplicationService.ShowFolderBrowserDialog("Open a Directory"))
            ApplicationService.ShowMessage(DialogType.Information, "Folder", ApplicationService.FolderPath);
    }

    void OpenFileExecute()
    {
        if (ApplicationService.ShowOpenFileDialog())
            ApplicationService.ShowMessage(DialogType.Information, "File", ApplicationService.FileName);
    }

    void CommandWithParameterExecute(string Parameter)
    {
        ApplicationService.ShowMessage(DialogType.Information, "Parameter", Parameter);
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

    protected override void OnPropertyChanged(string Property, object OldValue, object NewValue)
    {
        base.OnPropertyChanged(Property, OldValue, NewValue);

        if (Property == nameof(SomeText))
            SomeInformation = "Property changed";
    }
}

public enum SomeEnum
{
    [Description("Value 1")]
    Val1,

    [Description("Another Value")]
    Val2,

    Val3,
}