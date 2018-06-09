using System.Collections.Generic;
using System.IO;

namespace DiscordBotBuilder
{
    public static class FileManagement
    {
        /// <summary>
        ///     Create the project folder and add basic files
        /// </summary>
        /// <param name="options"></param>
        public static void CreateProjectFolder(List<string> options)
        {
            var projectFolderDirectory = "Projects/" + options[0];

            //Create project directory unless it already exists
            if (!Directory.Exists(projectFolderDirectory))
                Directory.CreateDirectory(projectFolderDirectory);

            //Create project directory and options file
            Directory.CreateDirectory(projectFolderDirectory + "/Commands");
            File.AppendAllText(projectFolderDirectory + "/Options.set",
                CodeGenerator.GenerateOptions(options)); //Write options file
        }

        /// <summary>
        ///     Add project to the list of all projects
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectPath"></param>
        public static void AddProjectToList(string projectName, string projectPath)
        {
            File.AppendAllText("ProjectsList.set", SaveFileParser.Generate(projectName, projectPath) + "\n");
        }

        /// <summary>
        ///     Add files needed before doing anything else
        /// </summary>
        public static void InitializeFileSystem()
        {
            if (!Directory.Exists("Projects"))
                Directory.CreateDirectory("Projects");

            if (!File.Exists("ProjectsList.set"))
                File.Create("ProjectsList.Set");
        }
    }
}