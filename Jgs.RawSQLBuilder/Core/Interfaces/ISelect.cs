namespace Jgs.RawSQLBuilder.Core.Interfaces
{
    public interface ISelect : ISelectCore, ISelectBase
    {
    }

    public interface ISelectCore
    {
        IFrom All();

        ISelectBase Distinct(string field, params string[] fields);

        ISelectCount Count(string field);

        ISelectCount CountDistinct(string field);
    }

    public interface ISelectBase : IFrom
    {
        ISelectField Field(string field);

        IFrom Fields(string field, params string[] fields);
    }

    public interface ISelectField : ISelectAlias, IFrom
    {
        ISelectCount Count(string field, params string[] fields);
    }

    public interface ISelectCount : ISelectBase, ISelectAlias
    {
    }

    public interface ISelectAlias : IFrom
    {
        ISelectBase As(string alias);
    }
}
