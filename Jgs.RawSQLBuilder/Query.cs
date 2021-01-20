using Jgs.RawSQLBuilder.Core;
using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder
{
    public static class Query
    {
        public static IFrom Select(params string[] fields)
        {
            return new SelectClause(fields);
        }
    }
}
