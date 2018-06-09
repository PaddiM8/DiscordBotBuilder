using System.Collections.Generic;
using System.Linq;
using DiscordBotBuilder.Management;

namespace DiscordBotBuilder.GenerateAndParse
{
    public static class TriggerCodeGenerator
    {
        /// <summary>
        ///     Generate the triggers into a string
        /// </summary>
        /// <param name="triggerCodes">The trigger codes</param>
        /// <returns></returns>
        public static string GenerateTriggers(List<List<string>> triggerCodes)
        {
            return string.Join("", triggerCodes.SelectMany(x =>
                GenerateIfStatementCode(CombineTriggersForIfStatement(x), x).ToList()));
        }

        /// <summary>
        ///     Generate the entire if statement
        /// </summary>
        /// <param name="ifContent">Code inside if statement</param>
        /// <param name="triggerCodeList">The trigger codes</param>
        /// <returns></returns>
        public static string GenerateIfStatementCode(string ifContent, List<string> triggerCodeList)
        {
            if (ifContent != null)
                return $"\tif ({ifContent}) {{\n$_ACTIONS{string.Join(", ", triggerCodeList)}_$\n\t}}\n\n";
            return null;
        }

        /// <summary>
        ///     Create the code inside if statements
        /// </summary>
        /// <param name="triggerCodes">The trigger codes</param>
        /// <returns></returns>
        public static string CombineTriggersForIfStatement(List<string> triggerCodes)
        {
            return string.Join(" || ", triggerCodes.Select(GenerateTriggerCode));
        }

        /// <summary>
        ///     Generate code for trigger
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns></returns>
        public static string GenerateTriggerCode(string commandString)
        {
            Parameters.ParameterList = ParseInput.Parse(commandString); //Update parameters

            switch (Parameters.Type)
            {
                case "message":
                    return CodeGenerator.GenerateOnMessageCode();
            }

            return null;
        }

        /// <summary>
        ///     Add all trigger snippets up for a specific event
        /// </summary>
        /// <param name="evnt">Event</param>
        /// <returns></returns>
        public static string GenerateTriggersForEvent(string evnt)
        {
            return GenerateTriggers(GeneralFunctions.GetCommandTypesInList(
                GeneralFunctions.GroupCommandStringsByEvent(Build.CommandStrings, evnt), "Trigger "));
        }
    }
}