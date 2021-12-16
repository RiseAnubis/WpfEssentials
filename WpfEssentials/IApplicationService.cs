namespace WpfEssentials;

/// <summary>
/// Provides functionality to handle windows and dialogs
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// A filter for file dialogs
    /// </summary>
    string Filter { get; set; }

    /// <summary>
    /// The full path of the selected file
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// The name including the extension of the selected file
    /// </summary>
    string SafeFileName { get; }

    /// <summary>
    /// The path of the folder selected in the folder browser dialog
    /// </summary>
    string FolderPath { get; }

    /// <summary>
    /// The dispatcher that belongs to the running application
    /// </summary>
    Dispatcher CurrentDispatcher { get; }

    /// <summary>
    /// Opens a window with the specified viewmodel logic to execute further logic of the ViewModel
    /// </summary>
    /// <param name="ViewModelInitializer">The action to initialize the viewmodel after it has been created</param>
    /// <param name="IsDialog">Sets whether the window should be opened as dialog</param>
    /// <param name="AfterLoadedAction">The action that should be invoked after the window has been rendered</param>
    TViewModel OpenWindow<TViewModel>(Action<TViewModel> ViewModelInitializer = null, bool IsDialog = false, Action<TViewModel> AfterLoadedAction = null) where TViewModel : BaseDialogViewModel;

    /// <summary>
    /// Opens a window with the specified viewmodel
    /// </summary>
    /// <param name="ViewModelCreator">The function that creates the ViewModel for the window</param>
    /// <param name="IsDialog">Sets whether the window should be opened as dialog</param>
    /// <param name="AfterLoadedAction">The action that should be invoked after the window has been rendered</param>
    TViewModel OpenWindow<TViewModel>(Func<TViewModel> ViewModelCreator, bool IsDialog = false, Action<TViewModel> AfterLoadedAction = null) where TViewModel : BaseDialogViewModel;

    /// <summary>
    /// Closes the window with the specified viewmodel
    /// </summary>
    /// <param name="ViewModel">The viewmodel of the corresponding window</param>
    void CloseWindow(BaseViewModel ViewModel);

    /// <summary>
    /// Hides the window with the specified viewmodel
    /// </summary>
    /// <param name="ViewModel">The viewmodel of the corresponding window</param>
    void HideWindow(BaseViewModel ViewModel);

    /// <summary>
    /// Closes the application
    /// </summary>
    void ExitApplication();

    /// <summary>
    /// Sets the main window for the application
    /// </summary>
    /// <param name="ViewModel">The viewmodel of the corresponding window</param>
    void SetMainWindow(BaseViewModel ViewModel);

    /// <summary>
    /// Shows a modal message box
    /// </summary>
    /// <param name="Type">The type of the message</param>
    /// <param name="Caption">The caption of the MessageBox</param>
    /// <param name="Message">The message that should be displayed</param>
    MessageBoxResult ShowMessage(DialogType Type, string Caption, string Message);

    /// <summary>
    /// Shows a save file dialog
    /// </summary>
    bool ShowSaveFileDialog();

    /// <summary>
    /// Shows an open file dialog
    /// </summary>
    bool ShowOpenFileDialog();

    /// <summary>
    /// Shows a folder browser dialog
    /// </summary>
    /// <param name="Description">The description of the dialog shown in the title</param>
    /// <returns></returns>
    System.Windows.Forms.DialogResult ShowFolderBrowserDialog(string Description);

    /// <summary>
    /// Searches for an application resource
    /// </summary>
    /// <param name="Name">The name of the resource</param>
    /// <returns></returns>
    T FindResource<T>(string Name);
}