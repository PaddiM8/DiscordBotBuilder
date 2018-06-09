using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordBotBuilder.Management;
using static DiscordBotBuilder.Validation;

namespace DiscordBotBuilder
{
    public static class CommandManagement
    {
        /// <summary>
        ///     Add a command
        /// </summary>
        public static void Add()
        {
            var commandName = Parameters.ParameterList[0]; //Name of the discord bot command
            File.Create(GeneralFunctions.GetCommandPathFromCommandName(commandName)); //Create command file
        }

        /// <summary>
        ///     Remove a command
        /// </summary>
        public static void Remove()
        {
            var nameOfCommand = Parameters.ParameterList[0];
            if (CommandExists(nameOfCommand)) //VALIDATION
                File.Delete(GeneralFunctions.GetCommandPathFromCommandName(nameOfCommand)); //Delete the command file
        }

        /// <summary>
        ///     List all commands for current project
        /// </summary>
        /// <returns></returns>
        public static List<string> ListAll()
        {
            var filePaths =
                Directory.GetFiles(GeneralFunctions.GetProjectDirectory() +
                                   "/Commands"); //List of all the commands file paths
            var fileNames = new List<string>();

            filePaths.ToList()
                .ForEach(x => fileNames.Add(Path.GetFileNameWithoutExtension(x))); //Get file name only, and add to list

            return fileNames;
        }

        /// <summary>
        ///     Edit a command
        /// </summary>
        public static void Edit()
        {
            Parameters.CommandName = Parameters.ParameterList[0]; //Set parameter command name

            if (CommandExists(Parameters.CommandName)) //VALIDATION {                 
                Program.OpenCommand =
                    Parameters
                        .CommandName; //Set openCommand variable to current open command                           
        }

        /// <summary>
        ///     Add trigger to a command
        /// </summary>
        /// <param name="commandString">The string of properties for trigger</param>
        public static void AddTrigger(string commandString)
        {
            File.AppendAllText(GeneralFunctions.GetCommandPathFromCommandName(Program.OpenCommand),
                "Trigger " + commandString + "\n"); //Append Trigger code to file
        }

        /// <summary>
        ///     Add action to a command
        /// </summary>
        /// <param name="commandString">The string of properties for action</param>
        public static void AddAction(string commandString)
        {
            File.AppendAllText(GeneralFunctions.GetCommandPathFromCommandName(Program.OpenCommand),
                "Action " + commandString + "\n"); //Append Action code to file
        }

        /// <summary>
        ///     Set bot token
        /// </summary>
        /// <param name="token">The token you wish you to set</param>
        public static void SetToken(string token)
        {
            Build.Token = token;
            var optionsFilePath = "Projects/" + Program.OpenProject + "/Options.set";
            GeneralFunctions.ChangeLineInFile(optionsFilePath, token, 2);
        }
    }
}