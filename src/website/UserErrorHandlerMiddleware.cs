namespace Kcsara.Database.Website
{
  using System;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;
  using Microsoft.AspNet.Builder;
  using Microsoft.AspNet.Http;
  using Microsoft.AspNet.Mvc.ModelBinding;
  using Microsoft.Extensions.Logging;
  using Newtonsoft.Json;

  internal class UserErrorHandlerMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public UserErrorHandlerMiddleware(RequestDelegate next,
                                  ILoggerFactory loggerFactory)
    {
      _next = next;
      _logger = loggerFactory.CreateLogger<UserErrorHandlerMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
      var accept = context.Request.GetTypedHeaders().Accept;

      if (!accept.Any(f => f.MediaType == "application/json"))
      {
        await _next(context);
        return;
      }

      try
      {
        await _next(context);
      }
      catch (BadRequestException ex)
      {
        if (context.Response.HasStarted)
        {
          _logger.LogWarning("The response has already started, the error handler will not be executed.");
          throw;
        }

        PathString originalPath = context.Request.Path;
        try
        {
          context.Response.StatusCode = 400;
          context.Response.Headers.Clear();
          context.Response.Headers.Add("Content-Type", "application/json");
          await context.Response.WriteAsync(JsonConvert.SerializeObject(ex.MessageData ?? new { ex.Message }));
          return;
        }
        catch (Exception ex2)
        {
          _logger.LogError("An exception was thrown attempting to execute the error handler.", ex2);
        }
        finally
        {
          context.Request.Path = originalPath;
        }

        throw; // Re-throw the original if we couldn't handle it
      }
    }
  }

  internal class UserErrorException : Exception
  {
    public object MessageData { get; set; }

    public HttpStatusCode StatusCode { get; private set; }

    public UserErrorException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
      : base(message)
    {
      this.StatusCode = statusCode;
    }

    public UserErrorException(ModelStateDictionary modelState, string message, HttpStatusCode statusCode) : this(message, statusCode)
    {
      var errorList = modelState
                  .Where(f => f.Value.Errors.Any())
                  .ToDictionary(f => f.Key, f => f.Value.Errors.Select(g => new
                  {
                    Message = g.ErrorMessage
                  }));

      MessageData = new
      {
        Errors = errorList.Count > 0 ? errorList : null
      };
    }

  }

  internal class NeedLoginException : UserErrorException
  {
    public NeedLoginException(string message = "please login") : base(message, HttpStatusCode.Unauthorized) { }
  }

  internal class BadRequestException : UserErrorException
  {
    public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest) { }
    public BadRequestException(ModelStateDictionary modelState) : this("bad arguments") { }
  }
}
