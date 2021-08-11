using System.Windows;
using System.Windows.Controls;

namespace WpfEssentials.SampleApp
{
    public class MyTestControl : Control
    {
        public static readonly DependencyProperty SomeNumberProperty;
        public static readonly RoutedEvent SomethingExecutedEvent;

        public int SomeNumber
        {
            get => (int)GetValue(SomeNumberProperty);
            set => SetValue(SomeNumberProperty, value);
        }

        public event RoutedEventHandler SomethingExecuted
        {
            add => AddHandler(SomethingExecutedEvent, value);
            remove => RemoveHandler(SomethingExecutedEvent, value);
        }

        static MyTestControl()
        {
            DependencyPropertyRegistrator<MyTestControl>.Create()
                .OverrideDefaultStyle() // override the default style to use your own
                .Register(x => x.SomeNumber, out SomeNumberProperty, 16, FrameworkPropertyMetadataOptions.None, OnSomeNumberChanged, OnSomeNumberCoerceCallback)
                .RegisterRoutedEvent<RoutedEventHandler>(nameof(SomethingExecuted), out SomethingExecutedEvent, RoutingStrategy.Bubble);
        }

        static int OnSomeNumberCoerceCallback(MyTestControl Sender, int Value)
        {
            return Value > 5 ? 5 : Value;
        }

        static void OnSomeNumberChanged(MyTestControl Sender, int OldValue, int NewValue)
        {
            // Some logic when the property has changed
            Sender.RaiseEvent(new RoutedEventArgs(SomethingExecutedEvent, Sender));
        }
    }

    public class AttachedProperties : DependencyObject
    {
        public static readonly DependencyProperty SomeAttachedValueProperty;

        static AttachedProperties()
        {
            DependencyPropertyRegistrator<AttachedProperties>.Create()
                .RegisterAttached((Control Sender) => GetAttachedTooltip(Sender), out SomeAttachedValueProperty, "A Tooltip", FrameworkPropertyMetadataOptions.None, OnSomeAttachedValuePropertyChanged, null);
        }

        public static string GetAttachedTooltip(Control Element) => (string)Element.GetValue(SomeAttachedValueProperty);

        public static void SetAttachedTooltip(Control Element, string Value) => Element.SetValue(SomeAttachedValueProperty, Value);

        static void OnSomeAttachedValuePropertyChanged(Control Element, string OldValue, string NewValue)
        {
            Element.ToolTip = NewValue;
        }
    }
}
