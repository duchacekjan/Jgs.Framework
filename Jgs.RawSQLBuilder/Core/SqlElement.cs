using Jgs.RawSQLBuilder.Core.Interfaces;

namespace Jgs.RawSQLBuilder.Core
{
    public abstract class SqlElement : ISql
    {
        public string SQL => GetSqlCore();

        protected abstract string GetSql();

        protected virtual void ValidateArguments()
        {
        }

        private string GetSqlCore()
        {
            ValidateArguments();
            return GetSql();
        }

        public override string ToString()
        {
            return SQL;
        }
    }
}
