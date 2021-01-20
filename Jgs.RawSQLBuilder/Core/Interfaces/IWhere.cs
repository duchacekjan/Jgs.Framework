namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IWhere : ISql
    {
        IWhereBuilder Where(string condition);

        IWhereBuilder WhereExists(string selectQuery);

        IWhereBuilder WhereNotExists(string selectQuery);
    }
}
