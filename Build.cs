using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordBotBuilder.GenerateAndParse;
using DiscordBotBuilder.Management;
using DiscordBotBuilder.Properties;

namespace DiscordBotBuilder
{
    public static class Build
    {
        public static string Token { get; set; }
        public static List<List<string>> CommandStrings { get; set; }

        /// <summary>
        ///     Build
        /// </summary>
        public static void Start()
        {
            var neededEvents = GetNeededEvents();
            var code = Resources.CodeTemplate;
            CommandStrings = GeneralFunctions.GetCommandStringsFromNames(); //Strings of command stuff like: Trigger message; contains; content; yes

            code = AddEventsToCode(code); //Add events
            code = AddTriggersToCode(code, neededEvents); //Add triggers
            code = AddActionsToCode(code);
            code = AddTokenToCode(code);

            WriteCodeFile(code);
        }

        /// <summary>
        ///     Add the events to the final code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string AddEventsToCode(string code)
        {
            return code.Replace("$_EVENT_$", CodeGenerator.GenerateEvents());
        }

        /// <summary>
        ///     Add the triggers to the final code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="neededEvents">Events used</param>
        /// <returns></returns>
        public static string AddTriggersToCode(string code, List<string> neededEvents)
        {
            return string.Join("", neededEvents.Select(x => ReplaceTriggersPlaceholder(code, x))
                .ToList()).Replace("if () {", "if (true) {");
        }

        public static string ReplaceTriggersPlaceholder(string code, string evnt)
        {
            return code.Replace("$_" + evnt + "_TRIGGERS_$",
                TriggerCodeGenerator.GenerateTriggersForEvent(evnt));
        }

        /// <summary>
        ///     Add actions to final code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns></returns>
        public static string AddActionsToCode(string code)
        {
            return string.Join("", CommandStrings.Select(x => ReplaceActionPlaceholder(code, x)).ToList());
        }

        /// <summary>
        ///     Replace the action placeholder
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="command">Command</param>
        /// <returns></returns>
        public static string ReplaceActionPlaceholder(string code, List<string> command)
        {
            return code.Replace($"$_ACTIONS{GeneralFunctions.GetTypesFromCommandToString(command, "Trigger ")}_$",
                ActionCodeGenerator.GenerateActions(GeneralFunctions.GetTypesFromCommand(command, "Action ")));
        }

        /// <summary>
        ///     Add the token to code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string AddTokenToCode(string code)
        {
            return code.Replace("$_TOKEN_$", Token);
        }

        /// <summary>
        ///     Write the code to the code file
        /// </summary>
        /// <param name="code">Code</param>
        public static void WriteCodeFile(string code)
        {
            File.WriteAllText(GeneralFunctions.GetProjectDirectory() + "/Code.js", code);
        }

        /// <summary>
        ///     Get the events needed, in a list
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNeededEvents()
        {
            CommandStrings = GeneralFunctions.GetCommandStringsFromNames(); //Update command strings
            return CommandStrings.SelectMany(GetNeededEventList).ToList(); //List with the events we need
        }

        /// <summary>
        ///     Get the needed events from the list
        /// </summary>
        /// <param name="commandList">List of command strings</param>
        /// <returns></returns>
        public static List<string> GetNeededEventList(List<string> commandList)
        {
            var eventList = new List<string>();
            foreach (var commandString in commandList)
                eventList = GetNeededEventsFromList(eventList, commandString); //Add events needed to list

            return eventList;
        }

        /// <summary>
        ///     Get the needed events from list
        /// </summary>
        /// <param name="functions">The list with the events</param>
        /// <param name="commandString">Command string</param>
        /// <returns></returns>
        public static List<string> GetNeededEventsFromList(List<string> functions, string commandString)
        {
            UpdateParameters(commandString, "trigger");

            if (!functions.Contains(Parameters.Type)) //if list already has current command, don't add it.
                functions.Add(Parameters.Type.Replace("\n", ""));
            return functions;
        }

        /// <summary>
        ///     Update the parameters using command string
        /// </summary>
        /// <param name="commandString"></param>
        public static void UpdateParameters(string commandString, string type)
        {
            Parameters.ParameterList = ParseInput.Parse(commandString); //Parse command
            Commands.LoadParametersForCommand(type); //Update parameters
        }
    }
}