Overview
===
WpfEssentials provides basic functions to create a WPF MVVM application. It features the following:

- RelayCommand and AsyncRelayCommand classes with an optional generic parameter
- View models that can be used for dialogs and models/entities
- A view manager that handles WPF-specific tasks like opening a window with a corresponding view model

The main purpose of this small framework is to help you getting started with your mvvm application without 
much overhead of bigger frameworks. Simple register your windows and view models and you are basically ready to go!

Getting started
===
First of all, you have to register all of your windows with their corresponding view models:
1. Remove the *StartupUri*-property from your app.xaml
2. In the app.xaml.cs, override *OnStartup* and register your views with the *ApplicationService* and open your MainWindow:
```
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
```
3. Fill your view models with logic and use the *ApplicationService* to handle the WPF stuff!

Examples
===
Opening views
---
You can use the *ApplicationService* to open views (modal and non-modal) and initialize them.

Important: View models with contructor arguments are not (yet) supported.
```
var vm = ApplicationService.OpenWindow<TestDialogViewModel>(vm => vm.Initialize(SomeProperty), true);
```

Closing a view within a view model:
```
ApplicationService.CloseWindow(this);
```
Defining properties
---
If you derive your view model from *BaseViewModel* or *BaseDialogViewModel*, you can create properties in the following manner:
```
public string SomeProperty
{
    get => GetProperty<string>();
    set => SetProperty(value);
}
```
You don't have to use backing fields! All of the properties make a call to the *PropertyChanged* event automatically.

Defining commands
---

```
public RelayCommand<string> CommandWithParameter { get; }

public TheViewModel()
{
    CommandWithParameter = new RelayCommand<string>(CommandWithParameterExecute);
}

void CommandWithParameterExecute(string Parameter)
{
    ApplicationService.ShowMessage(DialogType.Information, "Parameter", Parameter.ToString());
}
```

Please refer to the sample project for further details.