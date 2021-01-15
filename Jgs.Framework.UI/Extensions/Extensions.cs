using System;
using System.Linq;

namespace Jgs.Framework.UI.Extensions
{
    public static class Extensions

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
    }
}
