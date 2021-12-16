using System.Linq.Expressions;

namespace WpfEssentials;

/// <summary>
/// Helper class to register Dependency Properties
/// </summary>
/// <typeparam name="T">The control to register the properties for</typeparam>
public class DependencyPropertyRegistrator<T> where T : DependencyObject
{
    DependencyPropertyRegistrator() { }

    /// <summary>
    /// Creates a new registrator to work with
    /// </summary>
    public static DependencyPropertyRegistrator<T> Create()
    {
        return new ();
    }

    /// <summary>
    /// Overrides the default style for the owner control
    /// </summary>
    public DependencyPropertyRegistrator<T> OverrideDefaultStyle()
    {
        new DummyElement();
        return this;
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty)
    {
        return Register(Property, out DependencyProperty, default);
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty, TProperty DefaultValue)
    {
        return Register(Property, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, null, null);
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags)
    {
        return Register(Property, out DependencyProperty, DefaultValue, Flags, null, null);
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags, Action<T, TProperty, TProperty> PropertyChangedCallback)
    {
        return Register(Property, out DependencyProperty, DefaultValue, Flags, PropertyChangedCallback, null);
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty, TProperty DefaultValue, Action<T, TProperty, TProperty> PropertyChangedCallback)
    {
        return Register(Property, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback, null);
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    /// <param name="CoerceValueCallback">The method that should be executed to coerce the value of the property</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty, TProperty DefaultValue, Action<T, TProperty, TProperty> PropertyChangedCallback, Func<T, TProperty, TProperty> CoerceValueCallback)
    {
        return Register(Property, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback, CoerceValueCallback);
    }

    /// <summary>
    /// Registers a Dependeny Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    /// <param name="CoerceValueCallback">The method that should be executed to coerce the value of the property</param>
    public DependencyPropertyRegistrator<T> Register<TProperty>(Expression<Func<T, TProperty>> Property, out DependencyProperty DependencyProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags, Action<T, TProperty, TProperty> PropertyChangedCallback, Func<T, TProperty, TProperty> CoerceValueCallback)
    {
        DependencyProperty = DependencyProperty.Register((Property.Body as MemberExpression)?.Member.Name ?? Property.ToString(), typeof(TProperty), typeof(T), new FrameworkPropertyMetadata(DefaultValue, Flags, ToPropertyChangedCallback(PropertyChangedCallback), ToCoerceCallback(CoerceValueCallback)));
        return this;
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty)
    {
        return RegisterAttached(GetMethod, out DependencyProperty, default);
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty, TProperty DefaultValue)
    {
        return RegisterAttached(GetMethod, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, null, null);
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags)
    {
        return RegisterAttached(GetMethod, out DependencyProperty, DefaultValue, Flags, null, null);
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty, TProperty DefaultValue, Action<TOwner, TProperty, TProperty> PropertyChangedCallback)
    {
        return RegisterAttached(GetMethod, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback, null);
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags, Action<TOwner, TProperty, TProperty> PropertyChangedCallback)
    {
        return RegisterAttached(GetMethod, out DependencyProperty, DefaultValue, Flags, PropertyChangedCallback, null);
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    /// <param name="CoerceValueCallback">The method that should be executed to coerce the value of the property</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty, TProperty DefaultValue, Action<TOwner, TProperty, TProperty> PropertyChangedCallback, Func<TOwner, TProperty, TProperty> CoerceValueCallback)
    {
        return RegisterAttached(GetMethod, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback, CoerceValueCallback);
    }

    /// <summary>
    /// Registers an Attached Property for the application
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <typeparam name="TOwner">The type of the owner of the attached property</typeparam>
    /// <param name="GetMethod">The wrapper get-method that reads the attached property. The set-method is not needed, it will be called automatically</param>
    /// <param name="DependencyProperty">The actual dependency property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    /// <param name="CoerceValueCallback">The method that should be executed to coerce the value of the property</param>
    public DependencyPropertyRegistrator<T> RegisterAttached<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> GetMethod, out DependencyProperty DependencyProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags, Action<TOwner, TProperty, TProperty> PropertyChangedCallback, Func<TOwner, TProperty, TProperty> CoerceValueCallback)
    {
        DependencyProperty = DependencyProperty.RegisterAttached(GetPropertyNameFromMethod(GetMethod), typeof(TProperty), typeof(T), new FrameworkPropertyMetadata(DefaultValue, Flags, ToPropertyChangedCallback(PropertyChangedCallback), ToCoerceCallback(CoerceValueCallback)));
        return this;
    }


    /// <summary>
    /// Registers a Routed Event for the application
    /// </summary>
    /// <typeparam name="TRoutedEvent">The type of the routed event handler</typeparam>
    /// <param name="Property">The wrapper property that reads and writes the routed event</param>
    /// <param name="RoutedEvent">The actual routed event</param>
    /// <param name="Strategy">The routing strategy</param>
    public DependencyPropertyRegistrator<T> RegisterRoutedEvent<TRoutedEvent>(string Property, out RoutedEvent RoutedEvent, RoutingStrategy Strategy)
    {
        RoutedEvent = EventManager.RegisterRoutedEvent(Property, Strategy, typeof(TRoutedEvent), typeof(T));
        return this;
    }

    /// <summary>
    /// Adds another owner for a Dependency Property
    /// </summary>
    /// <param name="DependencyProperty">The property to add another owner for</param>
    /// <param name="OwnerProperty">The new owner property</param>
    public DependencyPropertyRegistrator<T> AddOwner(out DependencyProperty DependencyProperty, DependencyProperty OwnerProperty)
    {
        DependencyProperty = OwnerProperty.AddOwner(typeof(T));
        return this;
    }

    /// <summary>
    /// Adds another owner for a Dependency Property
    /// </summary>
    /// <typeparam name="TProperty">The datatype of the property</typeparam>
    /// <param name="DependencyProperty">The property to add another owner for</param>
    /// <param name="OwnerProperty">The new owner property</param>
    /// <param name="DefaultValue">The default value for the property</param>
    /// <param name="Flags">The flags that describe the property behavior</param>
    /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
    public DependencyPropertyRegistrator<T> AddOwner<TProperty>(out DependencyProperty DependencyProperty, DependencyProperty OwnerProperty, TProperty DefaultValue, FrameworkPropertyMetadataOptions Flags, Action<T, TProperty, TProperty> PropertyChangedCallback)
    {
        DependencyProperty = OwnerProperty.AddOwner(typeof(T), new FrameworkPropertyMetadata(DefaultValue, Flags, ToPropertyChangedCallback(PropertyChangedCallback)));
        return this;
    }

    /// <summary>
    /// Adds another owner for an Routed Event
    /// </summary>
    /// <param name="RoutedEvent">The event to add another owner for</param>
    /// <param name="OwnerEvent">The new owner</param>
    public DependencyPropertyRegistrator<T> AddOwner(out RoutedEvent RoutedEvent, RoutedEvent OwnerEvent)
    {
        RoutedEvent = OwnerEvent.AddOwner(typeof(T));
        return this;
    }

    string GetPropertyNameFromMethod<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> method)
    {
        if (method.Body is not MethodCallExpression methodExpression)
            throw new ArgumentException("Please specify a method to register the attached property.");

        var name = methodExpression.Method.Name;

        if (!name.StartsWith("Get"))
            throw new ArgumentException($"Per convention, a method for an attached property must start with a 'Get', actual method name is '{name}'");

        return name[3..];
    }


    PropertyChangedCallback ToPropertyChangedCallback<TOwner, TProperty>(Action<TOwner, TProperty, TProperty> PropertyChangedCallback)
    {
        return (d, e) => PropertyChangedCallback?.Invoke((TOwner)(object)d, (TProperty)e.OldValue, (TProperty)e.NewValue);
    }

    CoerceValueCallback ToCoerceCallback<TOwner, TProperty>(Func<TOwner, TProperty, TProperty> CoerceValueCallback)
    {
        if (CoerceValueCallback == null)
            return null;

        return (d, e) => CoerceValueCallback.Invoke((TOwner)(object)d, (TProperty)e);
    }

    class DummyElement : FrameworkElement
    {
        static DummyElement()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(T), new FrameworkPropertyMetadata(typeof(T)));
        }
    }
}