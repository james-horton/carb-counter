using Microsoft.AspNetCore.Components.Web;

namespace Carb_Counter.Services
{
    public class ErrorBoundaryLogger : IErrorBoundaryLogger
    {
        private readonly ILogger<ErrorBoundaryLogger> _logger;

        public ErrorBoundaryLogger(ILogger<ErrorBoundaryLogger> logger)
        {
            _logger = logger;
        }

        public ValueTask LogErrorAsync(Exception ex)
        {
            // Log errors using the built-in logger
            _logger.LogError(ex, ex.Message);

            return ValueTask.CompletedTask;
        }
    }
}
