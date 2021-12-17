namespace Jgs.UI.GridLayout
{
    public class LayoutLimitCollection : LayoutArgumentCollection
    {
        public LayoutLimitCollection(string arguments)
            : base(arguments)
        {
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
