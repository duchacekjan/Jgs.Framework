using Jgs.RawSQLBuilder.Core.Interfaces;
using System;

namespace Jgs.RawSQLBuilder.Core
{
    internal abstract class ASelectBuilder : ISql
    {
        private readonly string m_fieldName;
        private readonly SelectClauseEx m_parent;

        protected ASelectBuilder(SelectClauseEx parent, string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException(nameof(field));
            }
            m_fieldName = field;
            m_parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public string SQL => GetSql();

        protected string FieldName => m_fieldName;

        protected ISelectBase Parent => GetParent();


        protected virtual string GetFieldName() 
        {
            return m_fieldName;
        }

        protected virtual string GetAlias()
        {
            return null;
        }

        protected virtual string GetSql()
        {
            return Parent.SQL;
        }

        private ISelectBase GetParent()
        {
            var field = GetFieldName();
            var fieldName = GetFieldName(field, GetAlias());
            m_parent.AddFields(fieldName);
            return m_parent;
        }

        private static string GetFieldName(string field, string fieldAlias)
        {
            var alias = string.Empty;
            if (!string.IsNullOrEmpty(fieldAlias))
            {
                alias = $" as {fieldAlias}";
            }

            return $"{field}{alias}";
        }
    }
}
