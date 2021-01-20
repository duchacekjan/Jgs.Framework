using Jgs.RawSQLBuilder.Core.Interfaces;
using System;

namespace Jgs.RawSQLBuilder.Core
{
    internal class WhereBuilder : IWhere
    {
        private readonly IFrom m_from;
        private string[] m_conditions;

        public WhereBuilder(IFrom from, params string[] conditions)
        {
            m_from = from ?? throw new ArgumentNullException(nameof(from));
            m_conditions = conditions;
        }

        public string SQL => GetSql();

        private string GetSql()
        {
            var from = m_from.SQL;
            var conditions = m_conditions ?? new string[] { "1=1" };
            var conditionsSql = string.Join(" AND ", conditions);
            return $"{from} WHERE {conditionsSql}";
        }
    }
}
