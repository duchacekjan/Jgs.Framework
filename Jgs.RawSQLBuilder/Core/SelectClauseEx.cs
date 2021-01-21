using Jgs.RawSQLBuilder.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Jgs.RawSQLBuilder.Core
{
    internal class SelectClauseEx : ISelect
    {
        private readonly List<string> m_fields;
        private string m_tableName;
        private string m_tableAlias;

        public SelectClauseEx()
        {
            m_fields = new List<string>();
        }

        public string SQL => GetSql();

        public IFrom All()
        {
            m_fields.Add("*");
            return this;
        }

        public ISelectCount Count(string field)
        {
            return new SelectCountBuilder(this, field);
        }

        public ISelectCount CountDistinct(string field)
        {
            throw new System.NotImplementedException();
        }

        public ISelectBase Distinct(string field, params string[] fields)
        {
            throw new System.NotImplementedException();
        }

        public ISelectField Field(string field)
        {
            return new SelectFieldsBuilder(this, field);
        }

        public IFrom Fields(string field, params string[] fields)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException(nameof(field));
            }

            AddFields(field);
            AddFields(fields);

            return this;
        }

        public IWhere From(string tableName, string alias = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            m_tableName = tableName;
            m_tableAlias = alias;
            return new WhereBuilder(this);
        }

        internal void AddFields(params string[] fields)
        {
            if (fields?.Length > 0)
            {
                m_fields.AddRange(fields);
            }
        }

        private string GetSql()
        {
            //TODO validation

            var tableName = GetTableName();
            var fields = string.Join(", ", m_fields);
            return $"SELECT {fields} FROM {tableName}";
        }

        private string GetTableName()
        {
            var alias = string.Empty;
            if (!string.IsNullOrEmpty(m_tableAlias))
            {
                alias = $" as {m_tableAlias}";
            }

            return $"{m_tableName}{alias}";
        }
    }
}
