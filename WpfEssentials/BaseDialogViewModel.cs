namespace WpfEssentials
{
    /// <summary>
    /// A base view model for WPF windows and dialogs
    /// </summary>
    public abstract class BaseDialogViewModel : BaseViewModel
    {
        /// <summary>
        /// Command that can be used to confirm a dialog
        /// </summary>
        public RelayCommand ConfirmDialogCommand { get; }

        /// <summary>
        /// Command that can be used to cancel a dialog
        /// </summary>
        public RelayCommand CancelDialogCommand { get; }

        /// <summary>
        /// The result if the window is invoked as dialog
        /// </summary>
        public bool DialogResult { get; protected set; }

        /// <summary>
        /// The title of the window
        /// </summary>
        public virtual string Title { get; }

        /// <summary>
        /// The application service that is used to handle WPF-specific tasks
        /// </summary>
        public IApplicationService ApplicationService { get; set; }

        protected BaseDialogViewModel()
        {
            ConfirmDialogCommand = new RelayCommand(ConfirmDialogExecute);
            CancelDialogCommand = new RelayCommand(CancelDialogExecute);
        }

        protected virtual void CancelDialogExecute()
        {
            DialogResult = false;
            ApplicationService.CloseWindow(this);
        }

        protected virtual void ConfirmDialogExecute()
        {
            DialogResult = true;
            ApplicationService.CloseWindow(this);
        }
    }
}
