using Microsoft.Extensions.Logging;

namespace Bannerlord.DiscordRichPresence.Utils
{
    internal class MicrosoftLogger : DiscordRPC.Logging.ILogger
    {
        private readonly ILogger _logger;

        public MicrosoftLogger(ILogger logger) => _logger = logger;

        public DiscordRPC.Logging.LogLevel Level { get; set; }

        public void Trace(string message, params object[] args) => _logger.LogTrace(message, args);
        public void Info(string message, params object[] args) => _logger.LogInformation(message, args);
        public void Warning(string message, params object[] args) => _logger.LogError(message, args);
        public void Error(string message, params object[] args) => _logger.LogError(message, args);
    }
}