using System.Windows;

namespace Jgs.UI.Core
{
    /// <summary>
    /// Třída pro zjednodušené volání <see cref="FrameworkPropertyMetadata"/> s defaultním bindingem TwoWay
    /// </summary>
    public class TwoWayPropertyMetadata : FrameworkPropertyMetadata
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public TwoWayPropertyMetadata()
        {
            BindsTwoWayByDefault = true;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="propertyChangedCallback"></param>
        public TwoWayPropertyMetadata(PropertyChangedCallback propertyChangedCallback)
            : base(propertyChangedCallback)
        {
            BindsTwoWayByDefault = true;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="propertyChangedCallback"></param>
        public TwoWayPropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback = null)
            : base(defaultValue, propertyChangedCallback)
        {
            BindsTwoWayByDefault = true;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="propertyChangedCallback"></param>
        /// <param name="coerceValueCallback"></param>
        public TwoWayPropertyMetadata(PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback)
            : base(propertyChangedCallback, coerceValueCallback)
        {
            BindsTwoWayByDefault = true;
        }
    }
}
