using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using DiscordBotBuilder.Management;

#pragma warning disable 219

namespace DiscordBotBuilder.GenerateAndParse
{
    public static class ActionCodeGenerator
    {
        /// <summary>
        ///     Generate actions into one big string
        /// </summary>
        /// <param name="actionStrings">The action strings</param>
        /// <returns></returns>
        public static string GenerateActions(List<string> actionStrings)
        {
            return string.Join("\n",
                actionStrings.Select(GenerateActionCode).ToList());
        }

        /// <summary>
        ///     Generate the code from action string
        /// </summary>
        /// <param name="actionString">Action string</param>
        /// <returns></returns>
        public static string GenerateActionCode(string actionString)
        {
            Parameters.ParameterList = ParseInput.Parse(actionString); //Update the parameters
            Commands.LoadParametersForCommand("action"); //Update the parameters
            ConvertVariablesToJavaScript();

            var typeID = -1;
            var valueID = -1;

            switch (Parameters.Type)
            {
                case "message":
                    typeID = 0;
                    if (Parameters.Channel == "message") valueID = 0; //Channel by message
                    if (Parameters.Channel == "default") valueID = 1; //Channel by default channel
                    if (Parameters.Channel.Replace(" ", "").StartsWith("#")) valueID = 2; //Channel by name
                    if (valueID == -1) valueID = 3; //Channel by ID
                    break;
            }

            return GetActionCodeString(typeID, valueID, GetGuildAndChannelCodes(Parameters.Guild, Parameters.Channel),
                Parameters.Value);
        }

        /// <summary>
        ///     Convert variables ($blabla) from string in command string to javascript
        /// </summary>
        /// <returns></returns>
        public static void ConvertVariablesToJavaScript()
        {
            MatchCollection matches = Regex.Matches(Parameters.Value, @"\$[A-z+\.]+");

            foreach (var match in matches)
                Parameters.Value = Parameters.Value.Replace(match.ToString(), match.ToString().Replace("$", "' + ") + " + '");
        }

        /// <summary>
        ///     Grab correct string to use for action code string
        /// </summary>
        /// <param name="typeID">Type ID</param>
        /// <param name="valueID">Value ID</param>
        /// <param name="guildAndChannelCodes">Guild and channel codes array</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static string GetActionCodeString(int typeID, int valueID, string[] guildAndChannelCodes, string value)
        {
            var guildAndChannelCodeString =
                string.Join("", guildAndChannelCodes); //Add guild code and channel code to one string
            string[,] actionCodes = //The different action codes
            {
                {
                    $"\t\tmsg.channel.send('{value}');",
                    $"\t\t{guildAndChannelCodes[0]}.defaultChannel.send('{value}');",
                    $"\t\t{guildAndChannelCodeString}.send('{value}');",
                    $"\t\t{guildAndChannelCodeString}.send('{value}');"
                }
            };

            return actionCodes[typeID, valueID];
        }

        public static string[] GetGuildAndChannelCodes(string guildString, string channelString)
        {
            string[] guildAndChannelCodes = { $"client.guilds.find('id', '{guildString}')", $".channels.find('name', '{channelString.Replace("#", "")}')"};

            if (guildString == "message.server")
                guildAndChannelCodes[0] = "msg.guild";
            if (channelString == "message.channel")
                return new[] {"msg.channel", ""};

            return guildAndChannelCodes;
        }
    }
}