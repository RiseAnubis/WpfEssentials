using System.Windows.Media;

namespace WpfEssentials.Extensions;

public static class BrushExtensions
{
    /// <summary>
    /// Returns the color component of a brush
    /// </summary>
    /// <param name="Brush">The brush to get its color from</param>
    /// <returns></returns>
    public static Color ToColor(this Brush Brush)
    {
        return ((SolidColorBrush)Brush).Color;
    }
}