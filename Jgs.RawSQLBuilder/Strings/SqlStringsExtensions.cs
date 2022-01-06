namespace Jgs.RawSQLBuilder.Strings
{
    public static class SqlStringExtensions
    {
        private enum LikeWildCardPosition
        {
            Start,
            End,
            Both
        }

        public static string ToSql(this string text, bool allowNullOrEmpty = true)
        {
            if (!allowNullOrEmpty)
            {
                text.ValidateNotEmptyString(nameof(text));
            }
            return $"'{text}'";
        }

        public static string SqlContains(this string fieldName, string value, string wildCard = "%")
        {
            return fieldName.SqlLike(value, LikeWildCardPosition.Both, wildCard);
        }

        public static string SqlStartsWith(this string fieldName, string value, string wildCard = "%")
        {
            return fieldName.SqlLike(value, LikeWildCardPosition.Start, wildCard);
        }

        public static string SqlEndsWith(this string fieldName, string value, string wildCard = "%")
        {
            return fieldName.SqlLike(value, LikeWildCardPosition.End, wildCard);
        }

        public static void ValidateNotEmptyString(this string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value?.Trim()))
            {
                throw new System.ArgumentException("Argument is null or empty", fieldName);
            }
        }

        private static string SqlLike(this string fieldName, string value, LikeWildCardPosition position, string wildCard)
        {
            fieldName.ValidateNotEmptyString(nameof(fieldName));
            value.ValidateNotEmptyString(nameof(value));
            wildCard.ValidateNotEmptyString(nameof(wildCard));
            var likeFormat = GetLikeFormat(position, value, wildCard);
            return $"{fieldName} LIKE '{likeFormat}'";
        }

        private static object GetLikeFormat(LikeWildCardPosition position, string value, string wildCard)
        {
            string format;
            switch (position)
            {
                case LikeWildCardPosition.Start:
                    format = "{0}{1}";
                    break;
                case LikeWildCardPosition.End:
                    format = "{1}{0}";
                    break;
                case LikeWildCardPosition.Both:
                    format = "{0}{1}{0}";
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(position));
            }

            return string.Format(format, wildCard, value);
        }
    }
}
