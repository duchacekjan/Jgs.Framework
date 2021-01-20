using Jgs.RawSQLBuilder.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jgs.RawSQLBuilder.Core
{
    internal class WhereBuilder : IWhere
    {
        private readonly IFrom m_from;
        private List<ICondition> m_conditions;

        public WhereBuilder(IFrom from, params string[] conditions)
        {
            m_from = from ?? throw new ArgumentNullException(nameof(from));
            m_conditions = new List<ICondition>
            {
                new ConditionBuilder(ConditionOperator.And, conditions) 
            };
        }

        public string SQL => GetSql();

        private string GetSql()
        {
            var from = m_from.SQL;
            return $"{from} WHERE {m_conditions.FirstOrDefault()}";
        }
    }
}
