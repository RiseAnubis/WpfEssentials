using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfEssentials;

public static class EnumExtensions
{
    /// <summary>
    /// Returns the value of the DescriptionAttribute for an enum member if the attribute is defined
    /// </summary>
    /// <param name="EnumObj">The enum member to read the attribute from</param>
    /// <returns>The value of the DescriptionAttribute, otherwise the text of the enum member itself</returns>
    public static string GetEnumDescription(this Enum EnumObj)
    {
        var fieldInfo = EnumObj.GetType().GetField(EnumObj.ToString());
        var attribArray = fieldInfo.GetCustomAttributes(false);

        if (!attribArray.Any())
            return EnumObj.ToString();

        var attr = attribArray.First() as DescriptionAttribute;

        return attr.Description;
    }
}

/// <summary>
/// MarkupExtension to provide the values of an enum to use them as an ItemsSource
/// </summary>
public class EnumValuesExtension : MarkupExtension
{
    readonly Type enumType;

    public EnumValuesExtension(Type EnumType)
    {
        enumType = EnumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(enumType);
    }
}

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((Enum)value).GetEnumDescription();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}