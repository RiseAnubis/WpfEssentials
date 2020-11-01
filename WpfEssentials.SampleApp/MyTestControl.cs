using System.Windows;
using System.Windows.Controls;

namespace WpfEssentials.SampleApp
{
    public class MyTestControl : Control
    {
        public static readonly DependencyProperty SomeNumberProperty;

        public int SomeNumber
        {
            get => (int)GetValue(SomeNumberProperty);
            set => SetValue(SomeNumberProperty, value);
        }

        static MyTestControl()
        {
            DependencyPropertyRegistrator<MyTestControl>.Create()
                .OverrideDefaultStyle() // override the default style to use your own
                .Register(x => x.SomeNumber, out SomeNumberProperty, 16, OnSomeNumberChanged);
        }

        static void OnSomeNumberChanged(MyTestControl Sender, int OldValue, int NewValue)
        {
            // Some logic when the property has changed
        }
    }
}
