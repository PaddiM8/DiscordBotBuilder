using System;
using System.IO;
using DiscordBotBuilder.Management;
using static DiscordBotBuilder.Validation;

namespace DiscordBotBuilder
{
    public static class ProjectManagement
    {
        /// <summary>
        ///     Create a project
        /// </summary>
        public static void Create()
        {
            var projectName = Parameters.ParameterList[0];

            if (!ProjectExists(projectName, false))
            {
                FileManagement.CreateProjectFolder(Parameters.ParameterList);
                FileManagement.AddProjectToList(projectName, "Projects/" + projectName); //projectName, projectPath
                Commands.LoadParametersForProject();
                Console.WriteLine("Project Created!");
            }
        }

        /// <summary>
        ///     Open a project
        /// </summary>
        public static void Open(string projectName)
        {
            if (ProjectExists(projectName, true))
            {
                Program.OpenProject = projectName;
                Build.Token = File.ReadAllLines("Projects/" + Program.OpenProject + "/Options.set")[2];
                Commands.LoadParametersForProject();
            }
        }
    }
}