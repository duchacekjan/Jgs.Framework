using Jgs.RawSQLBuilder.Core.Interfaces;
using System.Collections.Generic;

namespace Jgs.RawSQLBuilder.Core
{
    internal class SelectClause : IFrom
    {
        private readonly List<string> m_fields;
        private string m_tableName;
        private string m_tableAlias;

        public SelectClause(string field, params string[] fields)
        {
            if (string.IsNullOrEmpty(field))
            {
                field = "*";
            }

            m_fields = new List<string>
            {
                field
            };

            if (fields?.Length > 0)
            {
                m_fields.AddRange(fields);
            }
        }

        public string SQL => GetSql();

        public static implicit operator string(SelectClause clause) => clause?.SQL;

        public IWhere From(string tableName, string alias = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new System.ArgumentNullException(nameof(tableName));
            }

            m_tableName = tableName;
            m_tableAlias = alias;
            return new WhereBuilder(this);
        }

        private string GetSql()
        {
            var fields = string.Join(", ", m_fields);
            var tableName = m_tableName;
            if (!string.IsNullOrEmpty(m_tableAlias))
            {
                tableName += $" as {m_tableAlias}";
            }

            return $"SELECT {fields} FROM {tableName}";
        }
    }
}
