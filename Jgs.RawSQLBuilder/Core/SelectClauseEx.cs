using Jgs.RawSQLBuilder.Core.Interfaces;
using Jgs.RawSQLBuilder.Strings;
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
            var distinctField = GetField(field, null);
            return new SelectCountBuilder(this, distinctField);
        }

        public ISelectBase Distinct(string field, params string[] fields)
        {
            var distinctField = GetField(field, fields);
            AddFields(distinctField);
            return this;
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

        private static string GetField(string field, string[] fields)
        {
            field.ValidateNotEmptyString(nameof(field));
            var allFields = new List<string>
            {
                field
            };

            if (fields?.Length > 0)
            {
                allFields.AddRange(fields);
            }

            var result = string.Empty;
            if (allFields.Count > 0)
            {
                result = $"DISTINCT {string.Join(", ", allFields)}";
            }
            return result;
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
