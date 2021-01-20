using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder.Core
{
    internal class SelectClause : IFrom
    {
        private readonly bool m_isDistinct;
        private readonly string[] m_fields;
        private string m_tableName;
        private string m_tableAlias;

        public SelectClause(bool isDistinct, params string[] fields)
        {
            m_isDistinct = isDistinct;
            m_fields = fields;
        }

        public string SQL => GetSql();

        public static implicit operator string(SelectClause clause) => clause?.SQL;

        public IFrom From(string tableName, string alias = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new System.ArgumentNullException(nameof(tableName));
            }

            m_tableName = tableName;
            m_tableAlias = alias;
            return this;
        }

        public IWhere Where(string condition)
        {
            return new WhereBuilder(this, condition);
        }

        public IWhere WhereExists(string selectQuery)
        {
            return WhereExists(false, selectQuery);
        }

        public IWhere WhereNotExists(string selectQuery)
        {
            return WhereExists(true, selectQuery);
        }

        private IWhere WhereExists(bool negate, string selectQuery)
        {
            if (string.IsNullOrEmpty(selectQuery))
            {
                throw new System.ArgumentNullException(nameof(selectQuery));
            }
            var not = string.Empty;
            if (negate)
            {
                not = "NOT ";
            }
            var exists = $"{not}EXISTS ({selectQuery})";
            return new WhereBuilder(this, exists);
        }

        private string GetSql()
        {
            var fields = m_fields ?? new string[] { "*" };
            var fieldsSql = string.Join(", ", fields);
            var tableName = m_tableName;
            if (!string.IsNullOrEmpty(m_tableAlias))
            {
                tableName += $" as {m_tableAlias}";
            }

            var distinct = "";
            if (m_isDistinct)
            {
                distinct = " DISTINCT";
            }

            return $"SELECT{distinct} {fieldsSql} FROM {tableName}";
        }
    }
}
