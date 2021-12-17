namespace Jgs.UI.GridLayout
{
    public class LayoutValueCollection : LayoutArgumentCollection
    {
        public LayoutValueCollection(string arguments)
            : base(arguments)
        {
        }

        public int? GetSize(LayoutArgumentType type) => m_arguments[type];
    }
}
