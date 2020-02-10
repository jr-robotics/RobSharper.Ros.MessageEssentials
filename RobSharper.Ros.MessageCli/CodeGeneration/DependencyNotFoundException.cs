using System;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public class DependencyNotFoundException : Exception
    {
        private readonly string _dependency;

        public string Dependency => _dependency;

        public DependencyNotFoundException() : base()
        {
            
        }
        
        public DependencyNotFoundException(string dependency) : base()
        {
            _dependency = dependency;
        }
        
        public DependencyNotFoundException(string dependency, string message) : base(message)
        {
            _dependency = dependency;
        }
        
        public DependencyNotFoundException(string dependency, string message, Exception innerException) : base(message, innerException)
        {
            _dependency = dependency;
        }
    }
}