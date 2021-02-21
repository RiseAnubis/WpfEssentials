using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfEssentials
{
    /// <summary>
    /// Base implementation of the PropertyChanged interface. It can be used for view models or entities/models to indicate whether a property has changed
    /// </summary>
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        readonly Dictionary<string, object> backingFields = new ();

        /// <summary>
        /// The PropertyChanged event that is invoked every time a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the value of the property the method is called from
        /// </summary>
        /// <typeparam name="T">The data type of the property</typeparam>
        /// <param name="Property">The name of the property. Leave it empty to get the name automatically</param>
        /// <returns>Returns the value of the property (default if null)</returns>
        protected T GetProperty<T>([CallerMemberName] string Property = null)
        {
            if (Property == null)
                throw new ArgumentNullException(nameof(Property));

            return backingFields.TryGetValue(Property, out var value) ? (T)value : default;
        }

        /// <summary>
        /// Sets the value of the property the method is called from
        /// </summary>
        /// <typeparam name="T">The data type of the property</typeparam>
        /// <param name="Value">The new value of the property</param>
        /// <param name="ForcePropertyChanged">Specifies whether the PropertyChanged event should be called even if the value hasn't changed</param>
        /// <param name="Property">The name of the property. Leave it empty to get the name automatically</param>
        protected void SetProperty<T>(T Value, bool ForcePropertyChanged = false, [CallerMemberName] string Property = null)
        {
            if (Property == null)
                throw new ArgumentNullException(nameof(Property));

            if (!backingFields.TryGetValue(Property, out var backingField) || ForcePropertyChanged || !Equals(backingField, Value))
            {
                backingFields[Property] = Value;
                OnPropertyChanged(Property, backingField, Value);
            }
        }

        /// <summary>
        /// Sets the value of the property the method is called from. Use this method if you are working with backing fields.
        /// </summary>
        /// <typeparam name="T">The data type of the property</typeparam>
        /// <param name="Storage">The backing field</param>
        /// <param name="Value">The new value of the property</param>
        /// <param name="ForcePropertyChanged">Specifies whether the PropertyChanged event should be called even if the value hasn't changed</param>
        /// <param name="Property">The name of the property. Leave it empty to get the name automatically</param>
        protected void SetProperty<T>(ref T Storage, T Value, bool ForcePropertyChanged = false, [CallerMemberName] string Property = null)
        {
            if (!ForcePropertyChanged && Equals(Storage, Value))
                return;

            var oldValue = Storage;
            Storage = Value;
            OnPropertyChanged(Property, oldValue, Value);
        }

        /// <summary>
        /// Invokes the PropertyChanged event
        /// </summary>
        /// <param name="Property">The name of the property that has changed</param>
        /// <param name="OldValue">The old value of the property</param>
        /// <param name="NewValue">The new value of the property</param>
        protected void OnPropertyChanged(string Property, object OldValue, object NewValue) => PropertyChanged?.Invoke(this, new PropertyChangedExtendedEventArgs(Property, OldValue, NewValue));
    }
}
