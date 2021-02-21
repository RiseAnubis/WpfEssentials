using System.ComponentModel;

namespace WpfEssentials
{
    /// <summary>
    /// Advanced PropertyChanged class that also contains the previous and the new value of the property whose value has changed
    /// </summary>
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// The previous value of the property
        /// </summary>
        public object OldValue { get; }

        /// <summary>
        /// The new value of the property
        /// </summary>
        public object NewValue { get; }

        /// <summary>
        /// Creates a new instance of PropertyChangedExtendedEventArgs
        /// </summary>
        /// <param name="Property">The name of the property that has changed</param>
        /// <param name="OldValue">The previous value of the property</param>
        /// <param name="NewValue">The new value of the property</param>
        public PropertyChangedExtendedEventArgs(string Property, object OldValue, object NewValue) : base(Property)
        {
            this.OldValue = OldValue;
            this.NewValue = NewValue;
        }
    }
}
