namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IWhere : ISql
    {
        IWhere And(string conditinon);

        IWhere And(ConditionOperator operatorBetweenConditions, params string[] conditions);

        IWhere Or(string condition);

        IWhere Or(ConditionOperator operatorBetweenConditions, params string[] conditions);
    }
}
