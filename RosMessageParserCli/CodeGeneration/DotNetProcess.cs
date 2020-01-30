using System;
using System.Diagnostics;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class DotNetProcess
    {
        const string ProgramName = "dotnet";
        
        public static Process Execute(string command)
        {
            var proc = new Process
            {
                StartInfo =
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = ProgramName,
                    Arguments = command
                }
            };
            
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            proc.WaitForExit();
            return proc;
        }
        
        public static Process Build(string projectFilePath)
        {
            var command = $"build \"{projectFilePath}\" -c Release -v normal";
            return Execute(command);
        }
    }
}