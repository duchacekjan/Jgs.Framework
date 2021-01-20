using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder.Core
{
    public class SelectClause : IFrom
    {
        private readonly string[] m_fields;
        private string m_tableName;
        private string m_tableAlias;

        public SelectClause(params string[] fields)
        {
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

        private string GetSql()
        {
            var fields = m_fields ?? new string[] { "*" };
            var fieldsSql = string.Join(", ", fields);
            var tableName = m_tableName;
            if (!string.IsNullOrEmpty(m_tableAlias))
            {
                tableName += $" as {m_tableAlias}";
            }

            return $"SELECT {fieldsSql} FROM {tableName}";
        }
    }
}
