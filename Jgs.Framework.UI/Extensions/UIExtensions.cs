﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jgs.Framework.UI.Extensions
{
    public static class UIExtensions
    {
        /// <summary>
        /// Získá první atribut daného typu, který je umístěn na <see cref="Enum"/> hodnotě
        /// </summary>
        /// <typeparam name="T">Typ hledaného atributu</typeparam>
        /// <param name="enumValue">Zkoumaná hodnota</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);
            return name.GetEnumAttribute<T>(type);
        }

        /// <summary>
        /// Získá první atribut daného typu, který je umístěn na <see cref="Enum"/> hodnotě
        /// </summary>
        /// <typeparam name="T">Typ hledaného atributu</typeparam>
        /// <param name="enumValue">Zkoumaná hodnota</param>
        /// <returns></returns>
        public static T GetEnumAttribute<T>(this string enumValue, Type enumType)
            where T : Attribute
        {
            T result = null;
            if (!string.IsNullOrEmpty(enumValue))
            {
                var prop = enumType.GetMember(enumValue).FirstOrDefault();
                if (prop?.GetCustomAttributes(typeof(T), false).FirstOrDefault() is T attr)
                {
                    result = attr;
                }
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

        public static ImageSource ToImageSource(this string urlImage)
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
