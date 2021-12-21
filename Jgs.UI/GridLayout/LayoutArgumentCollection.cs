using System;
using System.Collections.Generic;
using System.Linq;

namespace Jgs.UI.GridLayout
{
    public abstract class LayoutArgumentCollection
    {
#if NET6_0
        protected readonly IDictionary<LayoutArgumentType, int?> m_arguments = Enum.GetValues<LayoutArgumentType>()
                .ToDictionary(key => key, value => (int?)null);
#else
        protected readonly IDictionary<LayoutArgumentType, int?> m_arguments = Enum.GetValues(typeof(LayoutArgumentCollection))
            .OfType<LayoutArgumentType>()
            .ToDictionary(key => key, value => (int?)null);
#endif

        public LayoutArgumentCollection(string arguments)
        {
            if (!string.IsNullOrEmpty(arguments))
            {
                Parse(arguments);
            }
        }

        private void Parse(string arguments)
        {
            var parts = arguments.GetParts(".");
            foreach (var part in parts)
            {
                (var type, var value) = ParseArgument(part);
                m_arguments.AddOrUpdate(type, value);
            }
        }

        private static (LayoutArgumentType, int?) ParseArgument(string argument)
        {
            var parts = argument.GetParts("-");

            if (parts.Length > 0 && parts.Length <= 2)
            {
                var type = parts[0].ToLayoutArgumentType();

                var value = parts.Length > 1
                    ? int.Parse(parts[1])
                    : (int?)null;
                return (type, value);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(argument), "Invalid argument format.");
            }
        }
    }
}
