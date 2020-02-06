using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class ProcessFailedException : Exception
    {
        private string _message;
        
        public override string Message => _message;
        
        public string ProgramName { get; }
        public string ProgramArgument { get; }
        public bool HasExited { get; }
        public int ExitCode { get; }
        public string ProcessOutput { get; }

        public ProcessFailedException(string programName, string programArgument, bool hasExited, int exitCode,
            string processOutput)
        {
            ProgramName = programName;
            ProgramArgument = programArgument;
            HasExited = hasExited;
            ExitCode = exitCode;
            ProcessOutput = processOutput;
            SetMessage(programName, programArgument);
        }

        public ProcessFailedException(string programName, string programArgument, bool hasExited, int exitCode)
        {
            ProgramName = programName;
            ProgramArgument = programArgument;
            HasExited = hasExited;
            ExitCode = exitCode;
            SetMessage(programName, programArgument);
        }

        public ProcessFailedException(string programName, string programArgument, bool hasExited, int exitCode,
            string processOutput, Exception innerException) : base(null, innerException)
        {
            ProgramName = programName;
            ProgramArgument = programArgument;
            HasExited = hasExited;
            ExitCode = exitCode;
            ProcessOutput = processOutput;
            SetMessage(programName, programArgument);
        }

        public ProcessFailedException(string programName, string programArgument, bool hasExited, int exitCode, 
            Exception innerException) : base(null, innerException)
        {
            ProgramName = programName;
            ProgramArgument = programArgument;
            HasExited = hasExited;
            ExitCode = exitCode;
            SetMessage(programName, programArgument);
        }

        private void SetMessage(string programName, string programArgument)
        {
            var fullProgramName = programName;
            if (!string.IsNullOrEmpty(programArgument))
            {
                fullProgramName += " " + programArgument;
            }

            _message = $"Failed to execute '{fullProgramName}'";
        }
    }
}