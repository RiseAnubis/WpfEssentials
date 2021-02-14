namespace WpfEssentials.SampleApp
{
    public class ParameterViewModel : BaseDialogViewModel
    {
        public string Text
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        
        public int Value
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        public ParameterViewModel(string Text, int Value)
        {
            this.Text = Text;
            this.Value = Value;
        }
    }
}
