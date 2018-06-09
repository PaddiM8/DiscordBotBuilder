using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiscordBotBuilder
{
    public static class ParseInput
    {
        public static List<string> Parse(string input)
        {
            if (input.Contains(" ")) //If command has arguments
                input = GeneralFunctions
                    .RemoveFirstWord(input); //Split by space on first occurrence and grab second part of split

            return Regex.Split(input, @"\s*;\s*").ToList(); //Split by ;
        }
    }
}