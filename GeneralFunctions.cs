using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace DiscordBotBuilder
{
    public static class GeneralFunctions
    {
        /// <summary>
        ///     Get directory of project
        /// </summary>
        /// <returns></returns>
        public static string GetProjectDirectory()
        {
            return "Projects/" + Program.OpenProject;
        }

        /// <summary>
        ///     Get command name from command string
        /// </summary>
        /// <returns></returns>
        public static string GetCommandNameFromCommandString(string commandString)
        {
            return commandString.Split(new[] {' '}, 2)[0]; //Split by first space and grab first entry
        }

        /// <summary>
        ///     Get command path from command name
        /// </summary>
        /// <param name="commandName">The name of the command</param>
        /// <returns></returns>
        public static string GetCommandPathFromCommandName(string commandName)
        {
            return GetProjectDirectory() + "/Commands/" + commandName + ".dbc";
        }

        /// <summary>
        ///     Remove the last occurrence of specified character
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="c">Character</param>
        /// <returns></returns>
        public static string RemoveLastOccurrenceOfChar(string input, char c)
        {
            var indexOfChar = input.LastIndexOf(c);
            return input.Remove(indexOfChar);
        }

        /// <summary>
        ///     Remove last occurrence of string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="match">What to remove</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "StringLastIndexOfIsCultureSpecific.1")]
        public static string RemoveLastOccurrenceOfString(string input, string match)
        {
            if (input == null) return null;

            if (input.Contains(match))
                return input.Substring(0, input.LastIndexOf(match));

            return input;
        }

        /// <summary>
        ///     Remove the first word of a string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        public static string RemoveFirstWord(string input)
        {
            return input.Split(new[] {' '}, 2)[1];
        }

        /// <summary>
        ///     Edit a line in file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="input">Input string</param>
        /// <param name="line">Line number to edit</param>
        public static void ChangeLineInFile(string filePath, string input, int line)
        {
            var arrLine = File.ReadAllLines(filePath);
            arrLine[line] = input;
            File.WriteAllLines(filePath, arrLine);
        }

        /// <summary>
        ///     Get the first word in string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        public static string GetFirstWord(string input)
        {
            return input.Split(new[] {' '}, 2)[0];
        }

        /// <summary>
        ///     Remove all spaces, new lines, and tabs
        /// </summary>
        /// <param name="input">Input string to SuperTrim</param>
        /// <returns></returns>
        public static string SuperTrim(string input)
        {
            return input.Replace(" ", "")
                .Replace("\n", "")
                .Replace("\t", "");
        }

        /// <summary>
        ///     Convert a parameter name to JavaScript code
        /// </summary>
        /// <param name="parameter">Parameter(event)</param>
        /// <returns></returns>
        public static string ParameterToJavaScriptName(string parameter)
        {
            switch (SuperTrim(parameter))
            {
                case "message":
                    return "on('message', msg";
                case "memberjoin":
                    return "on('memberjoin', member";
                default:
                    return "error";
            }
        }

        /// <summary>
        ///     Get all triggers/actions in a list
        /// </summary>
        /// <param name="commandStrings">List of command strings</param>
        /// <param name="name">Type. Trigger or action</param>
        /// <returns></returns>
        public static List<List<string>> GetCommandTypesInList(List<List<string>> commandStrings, string name)
        {
            return commandStrings.Select(x => GetCommandTypesInCommandList(x, name)).ToList();
        }

        public static List<string> GetCommandTypesInCommandList(List<string> commandList, string name)
        {
            return commandList.Where(command => command.StartsWith(name)).ToList();
        }

        /// <summary>
        ///     Get command strings from command names
        /// </summary>
        /// <returns></returns>
        public static List<List<string>> GetCommandStringsFromNames()
        {
            Debug.WriteLine(string.Join(", ", CommandManagement.ListAll()));
            return CommandManagement.ListAll() //Command names
                .Select(GetCommandStringsFromName).ToList();
        }

        /// <summary>
        ///     Get command strings from a command name
        /// </summary>
        /// <param name="commandName">Name of command</param>
        /// <returns></returns>
        public static List<string> GetCommandStringsFromName(string commandName)
        {
            return File.ReadAllLines(
                GetCommandPathFromCommandName(commandName)).ToList();
        }

        /// <summary>
        ///     Group all command strings by specific event
        /// </summary>
        /// <param name="commandStrings">Command strings</param>
        /// <param name="evnt">Event to group to</param>
        /// <returns></returns>
        public static List<List<string>> GroupCommandStringsByEvent(List<List<string>> commandStrings, string evnt)
        {
            return commandStrings.Select(x => GroupCommandStringByEvent(x, evnt)).ToList();
        }

        public static List<string> GroupCommandStringByEvent(List<string> commandList, string evnt)
        {
            return commandList.Where(x => ParseInput.Parse(x)[0] == evnt).ToList();
        }

        /// <summary>
        ///     Get triggers/actions from command
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="type">Trigger or action</param>
        /// <returns></returns>
        public static List<string> GetTypesFromCommand(List<string> command, string type)
        {
            return command.Where(x => x.StartsWith(type)).ToList();
        }

        /// <summary>
        ///     Get triggers/actions from command
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="type">Trigger or action</param>
        /// <returns></returns>
        public static string GetTypesFromCommandToString(List<string> command, string type)
        {
            return string.Join(", ",
                GetTypesFromCommand(command, "Trigger "));
        }

        /// <summary>
        ///     Replace first orrurrence in string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="replace">Replace this</param>
        /// <param name="with">With this</param>
        /// <returns></returns>
        public static string ReplaceFirstOccurrence(string input, string replace, string with)
        {
            var index = input.IndexOf(replace, StringComparison.Ordinal);
            return index >= 0 ? input.Insert(index, with) : input;
        }
    }
}