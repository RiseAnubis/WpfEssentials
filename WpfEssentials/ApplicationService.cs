namespace WpfEssentials;

/// <inheritdoc />
public class ApplicationService : IApplicationService
{
    readonly Dictionary<Type, Type> windowCollection = [];
    readonly List<Window> openWindows = [];

    /// <inheritdoc />
    public string Filter { get; set; }

    /// <inheritdoc />
    public string FileName { get; private set; }

    /// <inheritdoc />
    public string SafeFileName { get; private set; }

    /// <inheritdoc />
    public string FolderPath { get; private set; }

    /// <inheritdoc />
    public Dispatcher CurrentDispatcher => Application.Current.Dispatcher;

    /// <summary>
    /// Registers a window with a corresponding viewmodel to be used from the service
    /// </summary>
    /// <typeparam name="TViewModel">The viewmodel of the window that should be opened</typeparam>
    /// <typeparam name="TWindow">The corresponding window of the viewmodel</typeparam>
    public ApplicationService Register<TViewModel, TWindow>()
        where TViewModel : BaseDialogViewModel
        where TWindow : Window
    {
        windowCollection.Add(typeof(TViewModel), typeof(TWindow));

        return this;
    }

    /// <inheritdoc />
    public TViewModel OpenWindow<TViewModel>(Action<TViewModel> ViewModelInitializer = null, bool IsDialog = false, Action<TViewModel> AfterLoadedAction = null) where TViewModel : BaseDialogViewModel
    {
        var (key, value) = windowCollection.First(x => typeof(TViewModel) == x.Key);

        if (Activator.CreateInstance(value) is not Window window)
            throw new InvalidOperationException("The created object is not a Window");
        if (Activator.CreateInstance(key) is not TViewModel viewModel)
            throw new InvalidOperationException("The created object is not a ViewModel");

        openWindows.Add(window);
        viewModel.ApplicationService = this;
        ViewModelInitializer?.Invoke(viewModel);
        window.DataContext = viewModel;

        window.Closed += (_, _) => RemoveWindowAndCloseApp(window);

        if (AfterLoadedAction != null)
            window.ContentRendered += (_, _) => AfterLoadedAction(viewModel);

        if (window != Application.Current.MainWindow)
            window.Owner = Application.Current.MainWindow;

        if (IsDialog)
            window.ShowDialog();
        else
            window.Show();

        return viewModel;
    }

    /// <inheritdoc />
    public TViewModel OpenWindow<TViewModel>(Func<TViewModel> ViewModelCreator, bool IsDialog = false, Action<TViewModel> AfterLoadedAction = null) where TViewModel : BaseDialogViewModel
    {
        var (_, value) = windowCollection.First(x => x.Key == typeof(TViewModel));
        var viewModel = ViewModelCreator();

        if (Activator.CreateInstance(value) is not Window window)
            throw new InvalidOperationException("The created object is not a Window");

        openWindows.Add(window);
        viewModel.ApplicationService = this;
        window.DataContext = viewModel;
        window.Closed += (_, _) => RemoveWindowAndCloseApp(window);

        if (AfterLoadedAction != null)
            window.ContentRendered += (_, _) => AfterLoadedAction(viewModel);

        if (window != Application.Current.MainWindow)
            window.Owner = Application.Current.MainWindow;

        if (IsDialog)
            window.ShowDialog();
        else
            window.Show();

        return viewModel;
    }

    /// <inheritdoc />
    public void CloseWindow(BaseViewModel ViewModel)
    {
        var pair = windowCollection.First(x => ViewModel.GetType() == x.Key);
        var openWindow = openWindows.FirstOrDefault(x => x.DataContext.GetType() == pair.Key);
        openWindow?.Close();
    }

    /// <inheritdoc />
    public void HideWindow(BaseViewModel ViewModel)
    {
        var pair = windowCollection.First(x => ViewModel.GetType() == x.Key);
        var openWindow = openWindows.FirstOrDefault(x => x.DataContext.GetType() == pair.Key);
        openWindow?.Hide();
    }

    /// <inheritdoc />
    public void ExitApplication()
    {
        var windows = openWindows.OrderBy(x => x == Application.Current.MainWindow);

        foreach (var window in windows)
            if (window != Application.Current.MainWindow)
                openWindows.FirstOrDefault(x => x == window)?.Close();

        if (openWindows.Count == 1)
            openWindows[0].Close();
        else
            throw new InvalidOperationException("Not all windows have been closed!");
    }

    /// <inheritdoc />
    public void SetMainWindow(BaseViewModel ViewModel)
    {
        Application.Current.MainWindow = GetWindow(ViewModel);
    }

    /// <inheritdoc />
    public MessageBoxResult ShowMessage(DialogType Type, string Caption, string Message)
    {
        var buttons = MessageBoxButton.OK;
        var icon = MessageBoxImage.None;

        switch (Type)
        {
            case DialogType.Error:
                icon = MessageBoxImage.Error;
                break;
            case DialogType.Information:
                icon = MessageBoxImage.Information;
                break;
            case DialogType.Question:
                buttons = MessageBoxButton.YesNoCancel;
                icon = MessageBoxImage.Question;
                break;
            case DialogType.Warning:
                buttons = MessageBoxButton.OKCancel;
                icon = MessageBoxImage.Warning;
                break;
        }

        return MessageBox.Show(Application.Current.MainWindow, Message, Caption, buttons, icon);
    }

    /// <inheritdoc />
    public bool ShowSaveFileDialog()
    {
        var sfd = new SaveFileDialog { Filter = Filter };
        var result = sfd.ShowDialog(Application.Current.MainWindow).Value;
        FileName = sfd.FileName;
        SafeFileName = sfd.SafeFileName;

        return result;
    }

    /// <inheritdoc />
    public bool ShowOpenFileDialog()
    {
        var ofd = new OpenFileDialog { Filter = Filter };
        var result = ofd.ShowDialog(Application.Current.MainWindow).Value;
        FileName = ofd.FileName;
        SafeFileName = ofd.SafeFileName;

        return result;
    }

    /// <inheritdoc />
    public bool ShowFolderBrowserDialog(string Description)
    {
        var ofd = new OpenFolderDialog()
        {
            Title = Description,
        };

        var result = ofd.ShowDialog(Application.Current.MainWindow).Value;
        FolderPath = ofd.FolderName;

        return result;
    }

    /// <inheritdoc />
    public T FindResource<T>(string Name)
    {
        return (T)Application.Current.Resources[Name];
    }

    Window GetWindow(BaseViewModel ViewModel)
    {
        var pair = windowCollection.First(x => ViewModel.GetType() == x.Key);
        return openWindows.First(x => x.DataContext.GetType() == pair.Key);
    }

    void RemoveWindowAndCloseApp(Window Window)
    {
        openWindows.Remove(Window);

        if (openWindows.TrueForAll(x => !x.IsVisible))
            Application.Current.Shutdown();
    }
}