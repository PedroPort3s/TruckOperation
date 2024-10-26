using Models.Responses;
using System.Net;

namespace WebApi.Handlers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HandleException(ex, context);
            }
        }

        private void HandleException(Exception ex, HttpContext context)
        {
            var response = new TrucksApiErrorResponse();
            
            switch (ex)
            {
                case BadHttpRequestException httpEx:
                    response.Status = httpEx.StatusCode;
                    response.Title = httpEx.Message;
                    break;
                default:
                    response.Status = (int)HttpStatusCode.InternalServerError;
                    response.Title = "An error has ocurred.";
                    _logger.LogError(ex, "An unknown error has ocurred");
                    break;
            }

            context.Response.StatusCode = response.Status;
            context.Response.WriteAsJsonAsync(response);
        }
    }
}
