namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IFrom : ISql
    {
        IWhere From(string tableName, string alias = null);
    }
}
