namespace tjdaca.Class
{
    public static class Extensions
    {
        public static string TextShorten(this string text, int maxLength)
        {
            if (text == null) return string.Empty;

            if (text.Length > maxLength)
            {
                return $"{text.Substring(0, maxLength)}...";
            }
            else
                return text ;
        }
    }
}
