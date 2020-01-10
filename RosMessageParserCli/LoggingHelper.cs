using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Joanneum.Robotics.Ros.MessageParser.Cli
{
    public static class LoggingHelper
    {
        private static ILoggerFactory _factory = NullLoggerFactory.Instance;

        public static ILoggerFactory Factory
        {
            get => _factory;
            set => _factory = value ?? NullLoggerFactory.Instance;
        }
    }
}