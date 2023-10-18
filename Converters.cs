using System;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CryNotes
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary> A converter markup extension. </summary>
    ///
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        private static T _converter = null;

        /// <summary> Default constructor. </summary>
        public ConverterMarkupExtension()
        {
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     When implemented in a derived class, returns an object that is provided as the value of the target property for this
        ///     markup extension.
        /// </summary>
        ///
        /// <param name="serviceProvider"> A service provider helper that can provide services for the markup extension. </param>
        ///
        /// <returns> The object value to set on the property where the extension is applied. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value produced by the binding source. </param>
        /// <param name="targetType"> The type of the binding target property. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value that is produced by the binding target. </param>
        /// <param name="targetType"> The type to convert to. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }

    /// <summary> to collaps visibility converter. </summary>
    public class XToWidthConverter : ConverterMarkupExtension<XToWidthConverter>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value produced by the binding source. </param>
        /// <param name="targetType"> The type of the binding target property. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
                return SystemParameters.PrimaryScreenWidth * val;

            return Binding.DoNothing;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value that is produced by the binding target. </param>
        /// <param name="targetType"> The type to convert to. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double val)
                return value;

            return val / SystemParameters.PrimaryScreenWidth;
        }
    }

    /// <summary> to collaps visibility converter. </summary>
    public class YToHeightConverter : ConverterMarkupExtension<YToHeightConverter>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value produced by the binding source. </param>
        /// <param name="targetType"> The type of the binding target property. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
                return SystemParameters.PrimaryScreenHeight * val;

            return Binding.DoNothing;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value that is produced by the binding target. </param>
        /// <param name="targetType"> The type to convert to. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double val)
                return value;

            return val / SystemParameters.PrimaryScreenHeight;
        }
    }

    /// <summary> to collaps visibility converter. </summary>
    public class BoolToNoneWindowStyleConverter : ConverterMarkupExtension<BoolToNoneWindowStyleConverter>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value produced by the binding source. </param>
        /// <param name="targetType"> The type of the binding target property. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val && val)
                return WindowStyle.None;

            return WindowStyle.ToolWindow;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a value. </summary>
        ///
        /// <param name="value">      The value that is produced by the binding target. </param>
        /// <param name="targetType"> The type to convert to. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns> A converted value. If the method returns <see langword="null" />, the valid null value is used. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not WindowStyle val)
                return Binding.DoNothing;

            if (val == WindowStyle.None)
                return true;

            return false;
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary> A multi conver markup extension. </summary>
    ///
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class MultiConverMarkupExtension<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        private static T _converter = null;

        /// <summary> Default constructor. </summary>
        public MultiConverMarkupExtension()
        {
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     When implemented in a derived class, returns an object that is provided as the value of the target property for this
        ///     markup extension.
        /// </summary>
        ///
        /// <param name="serviceProvider"> A service provider helper that can provide services for the markup extension. </param>
        ///
        /// <returns> The object value to set on the property where the extension is applied. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Converts source values to a value for the binding target. The data binding engine calls this method when it propagates
        ///     the values from source bindings to the binding target.
        /// </summary>
        ///
        /// <param name="values">     The array of values that the source bindings in the
        ///                           <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value
        ///                           <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding
        ///                           has no value to provide for conversion. </param>
        /// <param name="targetType"> The type of the binding target property. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns>
        ///     A converted value.  
        ///     
        ///      If the method returns <see langword="null" />, the valid <see langword="null" /> value is used.  
        ///     
        ///      A return value of
        ///      <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" />
        ///      indicates that the converter did not produce a value, and that the binding will use the
        ///      <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.
        ///      
        ///     
        ///      A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" />
        ///      indicates that the binding does not transfer the value or use the
        ///      <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.
        /// </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a binding target value to the source binding values. </summary>
        ///
        /// <param name="value">       The value that the binding target produces. </param>
        /// <param name="targetTypes"> The array of types to convert to. The array length indicates the number and types of values
        ///                            that are suggested for the method to return. </param>
        /// <param name="parameter">   The converter parameter to use. </param>
        /// <param name="culture">     The culture to use in the converter. </param>
        ///
        /// <returns> An array of values that have been converted from the target value back to the source values. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }

    /// <summary> A multi path splitter converter. </summary>
    public class PositionToMarginConverter : MultiConverMarkupExtension<PositionToMarginConverter>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Converts source values to a value for the binding target. The data binding engine calls this method when it propagates
        ///     the values from source bindings to the binding target.
        /// </summary>
        ///
        /// <param name="values">     The array of values that the source bindings in the
        ///                           <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value
        ///                           <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding
        ///                           has no value to provide for conversion. </param>
        /// <param name="targetType"> The type of the binding target property. </param>
        /// <param name="parameter">  The converter parameter to use. </param>
        /// <param name="culture">    The culture to use in the converter. </param>
        ///
        /// <returns>
        ///     A converted value.  
        ///     
        ///      If the method returns <see langword="null" />, the valid <see langword="null" /> value is used.  
        ///     
        ///      A return value of
        ///      <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" />
        ///      indicates that the converter did not produce a value, and that the binding will use the
        ///      <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.
        ///     
        ///     
        ///      A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" />
        ///      indicates that the binding does not transfer the value or use the
        ///      <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.
        /// </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (values is null || values.Length != 2 || values[0] is not double val1 || values[1] is not double val2)
                return Binding.DoNothing;


            Thickness margin = new Thickness(0);
            margin.Left = SystemParameters.PrimaryScreenWidth * val1;
            margin.Top = SystemParameters.PrimaryScreenHeight * val2;

            return margin;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> Converts a binding target value to the source binding values. </summary>
        ///
        /// <param name="value">       The value that the binding target produces. </param>
        /// <param name="targetTypes"> The array of types to convert to. The array length indicates the number and types of values
        ///                            that are suggested for the method to return. </param>
        /// <param name="parameter">   The converter parameter to use. </param>
        /// <param name="culture">     The culture to use in the converter. </param>
        ///
        /// <returns> An array of values that have been converted from the target value back to the source values. </returns>
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is not Thickness margin)
                return new object[] { };

            return new object[] { margin.Left, margin.Top };
        }
    }
}
