using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder.Core
{
    internal class SelectFieldsBuilder : ASelectBuilder, ISelectField
    {
        private string m_alias;

        public SelectFieldsBuilder(SelectClauseEx parent, string field)
            :base(parent, field)
        {
        }

        public ISelectBase As(string alias)
        {
            m_alias = alias;
            return Parent;
        }

        public ISelectCount Count(string field)
        {
            return Parent.Count(field);
        }

        public IWhere From(string tableName, string alias = null)
        {
            return Parent.From(tableName, alias);
        }

        protected override string GetAlias()
        {
            return m_alias;
        }
    }
}
