using System.Windows;

namespace WpfEssentials.SampleApp
{
    public class TestDialogViewModel : BaseDialogViewModel
    {
        public string MyDescription
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public override string Title => "A test dialog";

        public void Initialize(string Description)
        {
            MyDescription = Description;
        }

        protected override void ConfirmDialogExecute()
        {
            var result = ApplicationService.ShowMessage(DialogType.Question, "Confirmation", "Are you sure?");

            if (result == MessageBoxResult.Yes)
                base.ConfirmDialogExecute();
        }
    }
}
