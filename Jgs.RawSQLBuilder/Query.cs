using Jgs.RawSQLBuilder.Core;
using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder
{
    public static class Query
    {
        public static IFrom Select(string field, params string[] fields)
        {
            return Select().Fields(field, fields);
        }

        public static ISelect Select()
        {
            return new SelectClauseEx();
        }
    }
}
