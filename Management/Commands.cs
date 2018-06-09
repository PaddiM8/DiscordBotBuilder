using System;
using DiscordBotBuilder.Management;
using static DiscordBotBuilder.Validation;

namespace DiscordBotBuilder
{
    public static class Commands
    {
        public static string CommandType { get; set; }

        public static string CommandString { get; set; }

        //public static List<string> Parameters { get; set; }
        public static string Type { get; set; }

        /// <summary>
        ///     Execute a command using a command string
        /// </summary>
        /// <param name="commandString">The command string</param>
        public static void DoCommand(string commandString)
        {
            CommandType = GeneralFunctions.GetCommandNameFromCommandString(commandString); //Get command name (first word in command string)  
            CommandString = SaveFileParser.EscapeSemiColons(commandString); //Escape semicolons
            Parameters.ParameterList = ParseInput.Parse(commandString);

            //if a project is open
            if (Program.OpenProject == null)
            {
                DoCommandNoOpenProject(); //Do a command that won't affect any project, eg. createproject, openproject, etc.
            }
            else
            {
                if (Program.OpenCommand == null)

                    DoCommandForOpenProject(); //Do a command for the open project
                else
                    EditCommand(); //Go in edit command mode
            }
        }

        /// <summary>
        ///     Do command when there's currently no open project
        /// </summary>
        public static void DoCommandNoOpenProject()
        {
            switch (CommandType)
            {
                case "createproject":
                    if (!IsValidCommand(3, 3)) break; //VALIDATE
                    ProjectManagement.Create(); //Create project with the parameters
                    break;
                case "openproject":
                    if (!IsValidCommand(1, 1)) break; //VALIDATE
                    ProjectManagement.Open(Parameters.ParameterList[0]); //Open project, parameters[0] = project name 
                    break;
            }
        }

        /// <summary>
        ///     Do command when a project is open
        /// </summary>
        public static void DoCommandForOpenProject()
        {
            switch (CommandType)
            {
                case "addcommand":
                    if (!IsValidCommand(1, 1)) break;
                    Parameters.CommandName = Parameters.ParameterList[0];
                    CommandManagement.Add(); //Add command
                    Console.WriteLine("Command Added!");
                    break;
                case "listcommands":
                    if (!IsValidCommand(0, 1)) break;
                    CommandManagement.ListAll()
                        .ForEach(x => Console.WriteLine(x)); //List all commands
                    break;
                case "removecommand":
                    if (!IsValidCommand(1, 1)) break;
                    Parameters.CommandName = Parameters.ParameterList[0];
                    CommandManagement.Remove(); //Remove command, parameters[0] = botCommandName
                    break;
                case "editcommand": //Edit command
                    if (!IsValidCommand(1, 1)) break;
                    Parameters.CommandName = Parameters.ParameterList[0];
                    CommandManagement.Edit(); //Edit command, parameters[0] = botCommandName
                    break;
                case "build":
                    Build.Start(); //Build project
                    break;
                case "token":
                    CommandManagement.SetToken(Parameters.Token); //Set token
                    break;
                case "exit":
                    Program.OpenProject = null;
                    break;
            }
        }


        /// <summary>
        ///     Edit a command
        /// </summary>
        public static void EditCommand()
        {
            if (CommandType != "exit")
            {
                CommandString = GeneralFunctions.RemoveFirstWord(CommandString); //Remove first word from command string
                CommandString = SaveFileParser.TakeBackSemiColonEscapes(CommandString); //Add in semicolons again
            }

            switch (CommandType)
            {
                case "addtrigger":
                    if (!IsValidCommand(1, 10)) break;
                    LoadParametersForCommand("trigger");
                    CommandManagement.AddTrigger(CommandString); //Add trigger
                    break;
                case "addaction":
                    if (!IsValidCommand(2, 10)) break;
                    LoadParametersForCommand("action");
                    CommandManagement.AddAction(CommandString); //Add action
                    break;
                case "exit":
                    Program.OpenCommand = null;
                    break;
            }
        }

        public static void LoadParametersForCommand(string type)
        {
            if (type == "action")
            {
                Parameters.Type = Parameters.ParameterList[0];
                Parameters.Value = Parameters.ParameterList[1];
                Parameters.Guild = Parameters.ParameterList[2];
                Parameters.Channel = Parameters.ParameterList[3];
            }
            else if (type == "trigger")
            {
                Parameters.Type = Parameters.ParameterList[0];
                Parameters.Value = Parameters.ParameterList[1];
                Parameters.CaseInsenstitive = Parameters.ParameterList[2];
            }
            
        }

        public static void LoadParametersForProject()
        {
            Parameters.ProjectName = Program.OpenProject;
            Parameters.Token = Build.Token;
        }
    }
}