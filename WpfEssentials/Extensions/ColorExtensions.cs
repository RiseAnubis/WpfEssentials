using System.Windows.Media;

namespace WpfEssentials.Extensions;

public static class ColorExtensions
{
    /// <summary>
    /// Increases the brightness of a Color by a given factor
    /// </summary>
    /// <param name="Color">The color to increase its brightness</param>
    /// <param name="CorrectionFactor">The brightness correction factor</param>
    /// <returns></returns>
    public static Color Brighten(this Color Color, float CorrectionFactor = 0.5f)
    {
        float red = Color.R;
        float green = Color.G;
        float blue = Color.B;

        if (CorrectionFactor < 0)
        {
            CorrectionFactor = 1 + CorrectionFactor;
            red *= CorrectionFactor;
            green *= CorrectionFactor;
            blue *= CorrectionFactor;
        }
        else
        {
            red = (255 - red) * CorrectionFactor + red;
            green = (255 - green) * CorrectionFactor + green;
            blue = (255 - blue) * CorrectionFactor + blue;
        }

        return Color.FromArgb(Color.A, (byte)red, (byte)green, (byte)blue);
    }
}