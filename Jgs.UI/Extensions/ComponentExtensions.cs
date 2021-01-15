using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Jgs.UI.Extensions
{
    /// <summary>
    /// Extension pro komponenty
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Doplní gradient stop jako <paramref name="offset"/> z <paramref name="color"/>
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="color"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static LinearGradientBrush AddGradient(this LinearGradientBrush brush, Color color, double offset = 1)
        {
            if (brush != null)
            {
                var gradient = new GradientStop(color, offset);
                brush.GradientStops.Add(gradient);
            }
            return brush;
        }

        /// <summary>
        /// Vrátí HWND na okno, ve kterém se nachází element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IntPtr GetHWND(this DependencyObject element)
        {
            var window = Window.GetWindow(element);
            var result = IntPtr.Zero;
            if (window != null)
            {
                result = new WindowInteropHelper(window).EnsureHandle();
            }
            return result;
        }

        /// <summary>
        /// Nastaví hodnotu property na jiném vlákně, aby došlo ke správnému propsání zpět do modelu.
        /// Hlavní využití je pro coerce nastavované hodnoty z modelu
        /// </summary>
        /// <typeparam name="TClass">Třída, na které se nachází DependencyProperty</typeparam>
        /// <typeparam name="TProperty">DependencyProperty (její hodnota)</typeparam>
        /// <param name="source"></param>
        /// <param name="expr"></param>
        /// <param name="value"></param>
        public static void SetPropertyBackToModel<TClass, TProperty>(this TClass source, Expression<Func<TClass, TProperty>> expr, TProperty value)
            where TClass : DependencyObject
        {
            source?.Dispatcher?.InvokeOnBackground(() =>
            {
                if (expr.Body is MemberExpression member)
                {
                    if (member.Member is PropertyInfo propInfo)
                    {
                        propInfo.SetValue(source, value);
                    }
                }
            });
        }

        /// <summary>
        /// Vyvolání akce na UI vlákně
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        private static void InvokeOnBackground(this Dispatcher dispatcher, Action action)
        {
            Task.Run(async () =>
            {
                await Task.CompletedTask;
                dispatcher?.Invoke(action);
            });
        }

        /// <summary>
        /// Nahradí první nalezený substring
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            return text.ReplaceFirst(search, replace, 0);
        }

        /// <summary>
        /// Nahradí první nalezený substring
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace, int offset)
        {
            var result = text;
            const string pattern = "^(.*?){0}";
            if (!string.IsNullOrEmpty(replace))
            {
                var regex = new System.Text.RegularExpressions.Regex(string.Format(pattern, search));
                result = regex.Replace(text, replace, 1, offset);
            }

            return result;
        }

        /// <summary>
        /// Finds part of Template with given name and given type
        /// </summary>
        /// <typeparam name="T">Type of searched part</typeparam>
        /// <param name="source">Control in which Template should be searched part</param>
        /// <param name="partName">PartName</param>
        /// <returns></returns>
        public static T FindTemplatePart<T>(this Control source, string partName)
            where T : DependencyObject
        {
            var part = source?.Template.FindName(partName, source);
            return part as T;
        }

        /// <summary>
        /// Finds part of Control with given name and given type
        /// </summary>
        /// <typeparam name="T">Type of searched part</typeparam>
        /// <param name="source">Control in which should be searched part</param>
        /// <param name="partName">PartName</param>
        /// <returns></returns>
        public static T FindName<T>(this Control source, string partName)
            where T : DependencyObject
        {
            var part = source?.FindName(partName);
            return part as T;
        }

        /// <summary>
        /// Sets property(<typeparamref name="TProperty"/>) value, when parent (<typeparamref name="TSource"/> <paramref name="source"/>) is not <see langword="null"/> 
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TProperty">Type of property</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="propertyExpression">Expression which selects property</param>
        /// <param name="value">New value of property</param>
        /// <remarks>Only <see cref="MemberExpression"/> supported<para>Main use is on CustomControl parts</para></remarks>
        /// <exception cref="NotSupportedException">Occurs when <see cref="Expression{TDelegate}"/> is not <see cref="MemberExpression"/></exception>
        public static void SetValueSafe<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyExpression, TProperty value)
        {
            if (source != null && propertyExpression != null)
            {
                var propertyInfo = propertyExpression.GetPropertyInfo();
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(source, value);
                }
            }
        }

        /// <summary>
        /// Invokes <paramref name="action"/> when <paramref name="source"/> is not null
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="source">Object for check is is not <see langword="null"/></param>
        /// <param name="action">Action to be invoked</param>
        /// <remarks>For fluent api use <see cref="AndIfNotNull{T}(T, Action{T})"/></remarks>
        public static void IfNotNull<T>(this T source, Action<T> action)
        {
            if (source != null)
            {
                action?.Invoke(source);
            }
        }

        /// <summary>
        /// Invokes <paramref name="action"/> when <paramref name="source"/> is not null.
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="source">Object for check is is not <see langword="null"/></param>
        /// <param name="action">Action to be invoked</param>
        /// <remarks>For fluent api</remarks>
        /// <returns>Returns <paramref name="source"/></returns>
        public static T AndIfNotNull<T>(this T source, Action<T> action)
        {
            if (source != null)
            {
                action?.Invoke(source);
            }

            return source;
        }

        /// <summary>
        /// Calls <see cref="ICommand.CanExecute(object)"/> and if it returns <see langword="true"/>
        /// then calls <see cref="ICommand.Execute(object)"/>
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="command">Command</param>
        /// <param name="parameter">Parameter</param>
        public static void ExecuteCmd<T>(this ICommand command, T parameter)
        {
            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }
        /// <summary>
        /// Calls <see cref="ICommand.CanExecute(null)"/> and if it returns <see langword="true"/>
        /// then calls <see cref="ICommand.Execute(null)"/>
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="parameter">Parameter</param>
        public static void ExecuteCmd(this ICommand command)
        {
            command.ExecuteCmd<object>(null);
        }

        /// <summary>
        /// Převede url na ImageSource
        /// </summary>
        /// <param name="urlImage"></param>
        /// <returns></returns>
        public static ImageSource UrlAsImageSource(this string urlImage)
        {
            ImageSource result = null;
            if (!string.IsNullOrEmpty(urlImage))
            {
                var bitmap = new BitmapImage();
                try
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(urlImage, UriKind.Absolute);
                    bitmap.EndInit();
                    result = bitmap;
                }
                catch { }
            }

            return result;
        }

        /// <summary>
        /// Vrátí barvu <paramref name="dark"/> nebo <paramref name="light"/>, podle toho, která je více kontrastní k <paramref name="brush"/>.
        /// Výchozí hodnoty jsou <paramref name="dark"/>=<see cref="Colors.Black"/> a <paramref name="light"/>=<see cref="Colors.White"/>
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="dark"></param>
        /// <param name="light"></param>
        /// <returns></returns>
        public static Color ContrastColor(this Brush brush, Color? dark = null, Color? light = null)
        {
            var original = (SolidColorBrush)brush;
            return ContrastColor(original.Color, dark, light);
        }

        /// <summary>
        /// Vrátí barvu <paramref name="dark"/> nebo <paramref name="light"/>, podle toho, která je více kontrastní k <paramref name="color"/>.
        /// Výchozí hodnoty jsou <paramref name="dark"/>=<see cref="Colors.Black"/> a <paramref name="light"/>=<see cref="Colors.White"/>
        /// </summary>
        /// <param name="color"></param>
        /// <param name="dark"></param>
        /// <param name="light"></param>
        /// <returns></returns>
        public static Color ContrastColor(this Color color, Color? dark = null, Color? light = null)
        {
            var luma = ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255;
#if NET48
            dark = dark ?? Colors.Black;
            light = light ?? Colors.White;
#else
            dark ??= Colors.Black;
            light ??= Colors.White;
#endif
            return luma > 0.5 ? dark.Value : light.Value;
        }

        /// <summary>
        /// Extract <see cref="PropertyInfo"/> from given <see cref="Expression{TDelegate}"/>
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TProperty">Type of property</typeparam>
        /// <param name="propertyExpression">Expression which selects property</param>
        /// <returns></returns>
        /// <remarks>Only <see cref="MemberExpression"/> supported</remarks>
        /// <exception cref="NotSupportedException">Occurs when <see cref="Expression{TDelegate}"/> is not <see cref="MemberExpression"/></exception>
        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyExpression)
        {
            PropertyInfo result;
            if (propertyExpression?.Body is MemberExpression memberExpression)
            {
                var propertyName = memberExpression.Member.Name;
                result = typeof(TSource).GetProperty(propertyName);
            }
            else
            {
                throw new NotSupportedException("Only member expression supported");
            }

            return result;
        }
    }
}
