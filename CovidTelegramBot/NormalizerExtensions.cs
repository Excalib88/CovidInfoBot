namespace CovidTelegramBot
{
    public static class NormalizerExtensions
    {
        public static int ToInt(this string input)
        {
            input = input
                .Replace(" ", string.Empty)
                .Replace("&nbsp;", string.Empty)
                .Replace("+", string.Empty)
                .Replace("<!-- -->", "");
            return int.Parse(input);
        }
     }
}