using System.Collections.Generic;

namespace DiscordBotBuilder.Management
{
    public static class Parameters
    {
        public static List<string> ParameterList { get; set; }
        public static string Type { get; set; }
        public static string Value { get; set; }
        public static string Guild { get; set; }
        public static string Channel { get; set; }
        public static string Token { get; set; }
        public static string ProjectName { get; set; }
        public static string CommandName { get; set; }
        public static string CaseInsenstitive { get; set; }
    }
}