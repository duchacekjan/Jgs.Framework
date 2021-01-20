namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IFrom : ISql
    {
        IFrom From(string tableName, string alias = null);

        IWhere Where(string condition);

        IWhere WhereExists(string selectQuery);

        IWhere WhereNotExists(string selectQuery);
    }
}
