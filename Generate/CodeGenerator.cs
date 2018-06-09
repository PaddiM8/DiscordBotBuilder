using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DiscordBotBuilder.Management;

namespace DiscordBotBuilder
{
    public static class CodeGenerator
    {
        public static string GenerateEvents()
        {
            return string.Join("", Build.GetNeededEvents()
                .Select(GenerateEventCode));
        }

        public static string GenerateOptions(List<string> options)
        {
            return string.Join("\n", options);
        }

        public static string GenerateEventCode(string evnt)
        {
            var eventJavaScriptName = GeneralFunctions.ParameterToJavaScriptName(evnt);
            return $"client.{eventJavaScriptName} => {{\n$_{evnt}_TRIGGERS_$\n}});\n";
        }

        public static string GenerateOnMessageCode()
        {
            var triggerType = Parameters.ParameterList[1];
            var value = Parameters.ParameterList[2];
            var code = "msg.content";

            if (Parameters.ParameterList[3] == "yes")
                code += ".toLowerCase()";

            if (triggerType == "contains")
                code += $".contains('{value}')";
            if (triggerType == "startswith")
                code += $".startsWith('{value}')";
            if (triggerType == "equals")
                code += $" === '{value}'";

            return code;
        }
    }
}