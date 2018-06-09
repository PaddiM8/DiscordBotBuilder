using System;

namespace DiscordBotBuilder
{
    class Program
    {
        public static string OpenProject { get; set; }
        public static string OpenCommand { get; set; }

        static void Main()
        {
            FileManagement.InitializeFileSystem(); //Create folders and other files

            var exit = false;
            while (!exit) //Infinite loop until exit
            {
                Console.Write(OpenProject + " " + OpenCommand + "> ");
                var input = Console.ReadLine();

                Commands.DoCommand(input);
            }
        }

    }
}