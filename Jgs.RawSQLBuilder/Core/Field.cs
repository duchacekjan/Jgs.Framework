using Jgs.RawSQLBuilder.Strings;
using System;
using System.Linq;

namespace Jgs.RawSQLBuilder.Core
{
    public class Field : SqlElement
    {
        private static readonly string[] _notAllowedChars = new[]
        {
             "%","^","&","(",")","{","}","+","-","/","]","[","'","'"," "
        };

        public Field(string fieldName)
            : base()
        {
            Name = string.Empty;
            Alias = string.Empty;
            ParseFieldName(fieldName);
        }

        public string Name { get; private set; }

        public string Alias { get; private set; }

        public static implicit operator Field(string fieldName)
        {
            return new Field(fieldName);
        }

        protected override string GetSql()
        {
            var alias = string.Empty;
            if (!string.IsNullOrEmpty(Alias))
            {
                alias = $" as {Alias}";
            }

            return $"{Name}{alias}";
        }

        private static string EnsureAllowedCharsOnly(string name)
        {
            if (name.HasNonASCIIChars())
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Field name contains illegal characters.");
            }

            foreach (var item in name)
            {
                if (_notAllowedChars.Contains(item.ToString()))
                {
                    throw new ArgumentOutOfRangeException(nameof(name), "Field name contains illegal characters.");
                }
            }

            return name;
        }

        private void ParseFieldName(string fieldName)
        {
            var parts = fieldName.GetParts(" as ");
            switch (parts.Length)
            {
                case 1:
                    AssignName(parts[0]);
                    break;
                case 2:
                    AssignName(parts[0]);
                    Alias = Field.EnsureAllowedCharsOnly(parts[1].Trim());
                    break;
                default:
                    throw new ArgumentException("Field name cannot be empty.", nameof(fieldName));
            }
        }

        private void AssignName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Field name cannot be empty.");
            }

            Name = Field.EnsureAllowedCharsOnly(name);
        }
    }
}
