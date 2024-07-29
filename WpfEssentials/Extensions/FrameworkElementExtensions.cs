using System.Reflection;

namespace WpfEssentials.Extensions;

public static class FrameworkElementExtensions
{
    /// <summary>
    /// Gets an element of the control template.
    /// </summary>
    /// <typeparam name="T">The type of the element to get</typeparam>
    /// <param name="Object">The current FrameworkElement</param>
    /// <param name="ElementName">The name of the element to get</param>
    /// <returns>Returns the desired element if found within the control template, otherwise null</returns>
    public static T GetElement<T>(this FrameworkElement Object, string ElementName) where T : FrameworkElement
    {
        var method = Object.GetType().GetMethod("GetTemplateChild", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("Method 'GetTemplateChild' not found");

        return (T)method.Invoke(Object, new object[] { ElementName });
    }
}