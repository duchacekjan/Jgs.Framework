namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IWhere : ISql
    {
        IWhereBuilder Where(string condition);

        IWhereBuilder WhereExists(string selectQuery);

        IWhereBuilder WhereNotExists(string selectQuery);
    }

    public interface IWhereBuilder : ISql
    {
        IWhereBuilder And(string conditinon);

        IWhereBuilder And(ConditionOperator operatorBetweenConditions, params string[] conditions);

        IWhereBuilder AndExists(string selectQuery);

        IWhereBuilder AndNotExists(string selectQuery);

        IWhereBuilder Or(string condition);

        IWhereBuilder Or(ConditionOperator operatorBetweenConditions, params string[] conditions);

        IWhereBuilder OrExists(string selectQuery);

        IWhereBuilder OrNotExists(string selectQuery);
    }
}
