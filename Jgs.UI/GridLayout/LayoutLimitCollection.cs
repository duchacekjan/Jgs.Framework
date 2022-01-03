using System;
using System.Collections.Generic;
using System.Linq;

namespace Jgs.UI.GridLayout
{
    public class LayoutLimitCollection
    {
#if NET6_0
        private readonly IDictionary<LayoutArgumentType, int?> m_arguments = Enum.GetValues<LayoutArgumentType>()
                .ToDictionary(key => key, value => (int?)null);
#else
        private readonly IDictionary<LayoutArgumentType, int?> m_arguments = Enum.GetValues(typeof(LayoutArgumentType))
            .OfType<LayoutArgumentType>()
            .ToDictionary(key => key, value => (int?)null);
#endif

        public LayoutLimitCollection(string arguments)
        {
            var map = arguments.ToLayoutTypeMap();
            m_arguments.Merge(map);
        }

        private int? CompactSize => m_arguments[LayoutArgumentType.Compact];

        private int? StandardSize => m_arguments[LayoutArgumentType.Standard];

        public LayoutArgumentType? GetLayoutType(double? width)
        {
            LayoutArgumentType? result = null;
            if (width.HasValue)
            {
                if (CompactSize > width)
                {
                    result = LayoutArgumentType.Compact;
                }
                else if (StandardSize < width)
                {
                    result = LayoutArgumentType.Wide;
                }
                else
                {
                    result = LayoutArgumentType.Standard;
                }
            }
            return result;
        }
    }
}
