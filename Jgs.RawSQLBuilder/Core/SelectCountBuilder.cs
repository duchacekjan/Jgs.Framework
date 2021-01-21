using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder.Core
{
    internal class SelectCountBuilder : ASelectBuilder, ISelectCount
    {
        private string m_alias;

        public SelectCountBuilder(SelectClauseEx parent, string field)
            : base(parent, field)
        {
        }

        public ISelectBase As(string alias)
        {
            m_alias = alias;
            return Parent;
        }

        public ISelectField Field(string field)
        {
            return Parent.Field(field);
        }

        public IFrom Fields(string field, params string[] fields)
        {
            return Parent.Fields(field, fields);
        }

        public IWhere From(string tableName, string alias = null)
        {
            return Parent.From(tableName, alias);
        }

        public ISelectCount Count(string field)
        {
            return Parent.Count(field);
        }
        protected override string GetFieldName()
        {
            return $"COUNT({FieldName})";
        }
        protected override string GetAlias()
        {
            return m_alias;
        }
    }
}
