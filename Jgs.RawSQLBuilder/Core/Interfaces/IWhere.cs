namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface IWhere : ISql
    {
        IWhere And(string conditinon);

        IWhere Or(string condition);
    }
}
