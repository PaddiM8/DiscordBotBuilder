using System;
using System.IO;
using DiscordBotBuilder.Management;

namespace DiscordBotBuilder
{
    public static class Validation
    {
        /// <summary>
        ///     Check if command entered is valid
        /// </summary>
        /// <param name="minParameters">Minimum allowed parameters</param>
        /// <param name="maxParameters">Maximium allowed parameters</param>
        /// <returns></returns>
        public static bool IsValidCommand(int minParameters, int maxParameters)
        {
            if (Parameters.ParameterList.Count >= minParameters && Parameters.ParameterList.Count <= maxParameters)
                return true;

            Console.WriteLine("Wrong use of command!");
            return false;
        }


        /// <summary>
        ///     Check if project exists
        /// </summary>
        /// <param name="projectName">Name of project</param>
        /// <param name="expectedResult">What you would want it to output</param>
        /// <returns></returns>
        public static bool ProjectExists(string projectName, bool expectedResult)
        {
            if (File.ReadAllText("ProjectsList.set").Contains(projectName + "¤")
            ) //Return true if project exists in ProjectsList
            {
                if (!expectedResult)
                    Console.WriteLine("Project already exist!");
                return true;
            }

            if (expectedResult)
                Console.WriteLine("Project doesn't exist!"); //Write to console if it doesn't exist, and return false
            return false;
        }

        /// <summary>
        ///     Check if command exists
        /// </summary>
        /// <param name="nameOfCommand">The name of the command</param>
        /// <returns></returns>
        public static bool CommandExists(string nameOfCommand)
        {
            if (File.Exists(GeneralFunctions.GetCommandPathFromCommandName(nameOfCommand))
            ) //Return true if command exists
                return true;

            Console.WriteLine("Command doesn't exist!"); //Write to console if it doesn't exist, and return false
            return false;
        }
    }
}