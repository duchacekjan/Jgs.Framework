using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder.Core
{
    internal class ConditionBuilder : ICondition
    {
        private readonly ConditionOperator m_conditionOperator;
        private string[] m_conditions;

        public ConditionBuilder(ConditionOperator conditionOperator, params string[] conditions)
        {
            m_conditionOperator = conditionOperator;
            m_conditions = conditions;
        }

        public override string ToString()
        {
            var conditions = m_conditions ?? new string[] { "1=1" };
            var conditionOperator = $" {m_conditionOperator.ToString().ToUpper()} ";
            return string.Join(conditionOperator, conditions);
        }
    }
}
