using System;
using System.Linq.Expressions;
using System.Windows;

namespace WpfEssentials
{
    /// <summary>
    /// Helper class to register Dependency Properties
    /// </summary>
    /// <typeparam name="TOwner">The control to register the properties for</typeparam>
    public class DependencyPropertyRegistrator<TOwner> where TOwner : DependencyObject
    {
        DependencyPropertyRegistrator() { }

        /// <summary>
        /// Creates a new registrator to work with
        /// </summary>
        public static DependencyPropertyRegistrator<TOwner> Create()
        {
            return new DependencyPropertyRegistrator<TOwner>();
        }

        /// <summary>
        /// Overrides the default style for the owner control
        /// </summary>
        public DependencyPropertyRegistrator<TOwner> OverrideDefaultStyle()
        {
            new DummyElement();
            return this;
        }

        /// <summary>
        /// Registers a Dependeny Property for the application
        /// </summary>
        /// <typeparam name="T">The datatype of the property</typeparam>
        /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
        /// <param name="DependencyProperty">The actual dependency property</param>
        public DependencyPropertyRegistrator<TOwner> Register<T>(Expression<Func<TOwner, T>> Property, out DependencyProperty DependencyProperty)
        {
            return Register(Property, out DependencyProperty, default);
        }

        /// <summary>
        /// Registers a Dependeny Property for the application
        /// </summary>
        /// <typeparam name="T">The datatype of the property</typeparam>
        /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
        /// <param name="DependencyProperty">The actual dependency property</param>
        /// <param name="DefaultValue">The default value for the property</param>
        public DependencyPropertyRegistrator<TOwner> Register<T>(Expression<Func<TOwner, T>> Property, out DependencyProperty DependencyProperty, T DefaultValue)
        {
            return Register(Property, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, null);
        }

        /// <summary>
        /// Registers a Dependeny Property for the application
        /// </summary>
        /// <typeparam name="T">The datatype of the property</typeparam>
        /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
        /// <param name="DependencyProperty">The actual dependency property</param>
        /// <param name="DefaultValue">The default value for the property</param>
        /// <param name="Flags">The flags that describe the property behavior</param>
        public DependencyPropertyRegistrator<TOwner> Register<T>(Expression<Func<TOwner, T>> Property, out DependencyProperty DependencyProperty, T DefaultValue, FrameworkPropertyMetadataOptions Flags)
        {
            return Register(Property, out DependencyProperty, DefaultValue, Flags, null);
        }

        /// <summary>
        /// Registers a Dependeny Property for the application
        /// </summary>
        /// <typeparam name="T">The datatype of the property</typeparam>
        /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
        /// <param name="DependencyProperty">The actual dependency property</param>
        /// <param name="DefaultValue">The default value for the property</param>
        /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
        public DependencyPropertyRegistrator<TOwner> Register<T>(Expression<Func<TOwner, T>> Property, out DependencyProperty DependencyProperty, T DefaultValue, Action<TOwner, T, T> PropertyChangedCallback)
        {
            return Register(Property, out DependencyProperty, DefaultValue, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback);
        }

        /// <summary>
        /// Registers a Dependeny Property for the application
        /// </summary>
        /// <typeparam name="T">The datatype of the property</typeparam>
        /// <param name="Property">The wrapper property that reads and writes the dependency property</param>
        /// <param name="DependencyProperty">The actual dependency property</param>
        /// <param name="DefaultValue">The default value for the property</param>
        /// <param name="Flags">The flags that describe the property behavior</param>
        /// <param name="PropertyChangedCallback">The method that should be executed when the value of the property has changed</param>
        public DependencyPropertyRegistrator<TOwner> Register<T>(Expression<Func<TOwner, T>> Property, out DependencyProperty DependencyProperty, T DefaultValue, FrameworkPropertyMetadataOptions Flags, Action<TOwner, T, T> PropertyChangedCallback)
        {
            DependencyProperty = DependencyProperty.Register((Property.Body as MemberExpression)?.Member.Name ?? Property.ToString(), typeof(T), typeof(TOwner), new FrameworkPropertyMetadata(DefaultValue, Flags, ActionToCallback(PropertyChangedCallback)));
            return this;
        }

        PropertyChangedCallback ActionToCallback<T>(Action<TOwner, T, T> PropertyChangedCallback)
        {
            return (d, e) => PropertyChangedCallback?.Invoke((TOwner)d, (T)e.OldValue, (T)e.NewValue);
        }

        class DummyElement : FrameworkElement
        {
            static DummyElement()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(TOwner), new FrameworkPropertyMetadata(typeof(TOwner)));
            }
        }
    }
}
