using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Carb_Counter.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;
        private readonly IExceptionHandlerPathFeature _exceptionHandlerPathFeature;

        public ErrorModel(ILogger<ErrorModel> logger, IExceptionHandlerPathFeature exceptionHandlerPathFeature)
        {
            _logger = logger;
            _exceptionHandlerPathFeature = exceptionHandlerPathFeature;
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            if (_exceptionHandlerPathFeature?.Error != null)
            {
                var ex = _exceptionHandlerPathFeature.Error;
                _logger.LogError(ex, ex.Message);
            }
        }

        public void OnPost()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            if (_exceptionHandlerPathFeature?.Error != null)
            {
                var ex = _exceptionHandlerPathFeature.Error;
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}