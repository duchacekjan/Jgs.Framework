namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IWhere : ISql
    {
        IWhere And(string conditinon);

        IWhere And(ConditionOperator operatorBetweenConditions, params string[] conditions);

        IWhere AndExists(string selectQuery);

        IWhere AndNotExists(string selectQuery);

        IWhere Or(string condition);

        IWhere Or(ConditionOperator operatorBetweenConditions, params string[] conditions);

        IWhere OrExists(string selectQuery);

        IWhere OrNotExists(string selectQuery);
    }
}
