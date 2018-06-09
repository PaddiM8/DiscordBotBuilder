namespace DiscordBotBuilder
{
    public static class SaveFileParser
    {
        /// <summary>
        ///     Generate save file value
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static string Generate(string value1, string value2)
        {
            value1 = value1.Replace("¤", @"\¤"); //Escape character ¤
            value2 = value2.Replace("¤", @"\¤"); //Escape character ¤

            return value1 + "¤" + value2;
        }

        /// <summary>
        ///     Replace all semicolons (add back later on) so they don't get affected
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        public static string EscapeSemiColons(string input)
        {
            return input.Replace(@"\;", "_SEMCOL_");
        }


        /// <summary>
        ///     Bring the semicolons back again
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        public static string TakeBackSemiColonEscapes(string input)
        {
            return input.Replace("_SEMCOL_", @"\;");
        }
    }
}