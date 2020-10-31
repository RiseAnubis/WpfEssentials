namespace WpfEssentials.SampleApp
{
    public class SecondWindowViewModel : BaseDialogViewModel
    {
        public RelayCommand<string> ChangeContentCommand { get; }
        
        public RelayCommand CustomCloseCommand { get; }

        public string Content
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public override string Title => "The second window";

        public SecondWindowViewModel()
        {
            ChangeContentCommand = new RelayCommand<string>(ChangeContentExecute);
            CustomCloseCommand = new RelayCommand(CustomCloseExecute);

            Content = "Default Content";
        }

        void CustomCloseExecute()
        {
            ApplicationService.CloseWindow(this);
        }

        void ChangeContentExecute(string Parameter)
        {
            Content = Parameter;
        }
    }
}
