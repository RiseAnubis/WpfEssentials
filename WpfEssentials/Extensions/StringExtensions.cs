using System.Windows.Media;

namespace WpfEssentials.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converts a string to a SolidColorBrush
    /// </summary>
    /// <param name="Value">The hex color string to convert</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static SolidColorBrush ToBrush(this string Value)
    {
        return Value.StartsWith('#') ? (SolidColorBrush)new BrushConverter().ConvertFrom(Value) : throw new ArgumentException("The provided string is not a valid hex color", nameof(Value));
    }
}