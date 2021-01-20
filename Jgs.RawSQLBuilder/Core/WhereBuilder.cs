using Jgs.RawSQLBuilder.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jgs.RawSQLBuilder.Core
{
    internal class WhereBuilder : IWhere
    {
        private readonly IFrom m_from;
        private readonly List<Condition> m_conditions;

        public WhereBuilder(IFrom from, string condition)
        {
            m_from = from ?? throw new ArgumentNullException(nameof(from));
            m_conditions = new List<Condition>
            {
                new Condition(condition)
            };
        }

        public string SQL => GetSql();

        public IWhere And(string condition)
        {
            Add(null, ConditionOperator.And, condition);
            return this;
        }

        public IWhere And(ConditionOperator operatorBetweenConditions, params string[] conditions)
        {

            Add(operatorBetweenConditions, ConditionOperator.And, conditions);
            return this;
        }

        public IWhere Or(string condition)
        {
            Add(null, ConditionOperator.Or, condition);
            return this;
        }

        public IWhere Or(ConditionOperator operatorBetweenConditions, params string[] conditions)
        {
            Add(operatorBetweenConditions, ConditionOperator.Or, conditions);
            return this;
        }

        public IWhere AndExists(string selectQuery)
        {
            return Exists(ConditionOperator.And, false, selectQuery);
        }

        public IWhere AndNotExists(string selectQuery)
        {
            return Exists(ConditionOperator.And, true, selectQuery);
        }

        public IWhere OrExists(string selectQuery)
        {
            return Exists(ConditionOperator.Or, false, selectQuery);
        }

        public IWhere OrNotExists(string selectQuery)
        {
            return Exists(ConditionOperator.Or, true, selectQuery);
        }

        private IWhere Exists(ConditionOperator conditionOperator, bool negate, string selectQuery)
        {
            if (string.IsNullOrEmpty(selectQuery))
            {
                throw new ArgumentNullException(nameof(selectQuery));
            }
            var not = string.Empty;
            if (negate)
            {
                not = "NOT ";
            }
            var exists = $"({not}EXISTS ({selectQuery}))";
            Add(null, conditionOperator, exists);
            return this;
        }

        private void Add(ConditionOperator? conditionInnerOperator, ConditionOperator? conditionOuterOperator, params string[] conditions)
        {
            m_conditions.Add(new Condition(conditionInnerOperator ?? ConditionOperator.And, conditions, conditionOuterOperator));
        }

        private string GetSql()
        {
            var from = m_from.SQL;
            return $"{from} WHERE {GetConditions()}";
        }

        private string GetConditions()
        {
            return string.Join(" ", m_conditions);
        }

        private class Condition
        {
            private readonly string m_condition;
            private readonly ConditionOperator? m_operator;

            public Condition(ConditionOperator conditionInnerOperator, string[] conditions, ConditionOperator? conditionOuterOperator = null)
                : this(ToCondition(conditionInnerOperator, conditions), conditionOuterOperator)
            {
            }

            public Condition(string condition, ConditionOperator? conditionOperator = null)
            {
                m_condition = GetValidCondition(condition);
                m_operator = conditionOperator;
            }

            private static string GetValidCondition(string condition)
            {
                if (string.IsNullOrEmpty(condition))
                {
                    condition = "1=1";
                }

                return condition;
            }

            private static string ToCondition(ConditionOperator? conditionOperator, params string[] conditions)
            {
                var operatorStr = GetOperator(conditionOperator, " {0} ");

                var conditionsSql = string.Join(operatorStr, conditions.Select(GetValidCondition));
                return conditions?.Length > 1 ? $"({conditionsSql})" : conditionsSql;
            }

            private static string GetOperator(ConditionOperator? conditionOperator, string format)
            {
                var operatorStr = conditionOperator?.ToString().ToUpper();
                if (conditionOperator.HasValue)
                {
                    operatorStr = string.Format(format, operatorStr);
                }

                return operatorStr;
            }

            public override string ToString()
            {
                var operatorStr = GetOperator(m_operator, "{0} ");
                return $"{operatorStr}{m_condition}";
            }
        }
    }
}
