namespace hoLinqToSql.LinqUtils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Safe Left of string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Prefix(this string value, int length)
        {
            if (value == null) return "";
            if (value.Length > length)
            {
                return value;
            }

            return value.Substring(0, value.Length);
        }
    }
}
