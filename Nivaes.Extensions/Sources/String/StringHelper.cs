namespace Nivaes
{
    using System.Globalization;

    public static class StringHelper
    {
        public static string FirstCharToUpper(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            // Return char and concat substring.
            return char.ToUpper(s[0], CultureInfo.InvariantCulture) + s.Substring(1);
        }
    }
}
