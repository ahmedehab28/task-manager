using Application.Common.Exceptions;
using Domain.Entities.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (ValidationException vex)
            {
                var errors = vex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                var pd = new ValidationProblemDetails(errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Validation Failed",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = ctx.Request.Path
                };
                ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ctx.Response.ContentType = "application/problem+json";
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (NotFoundException nex)
            {
                Log.Error(nex, nex.Message);           // Serilog
                ctx.Response.StatusCode = 404;
                ctx.Response.ContentType = "application/problem+json";
                var pd = new ProblemDetails
                {
                    Title = nex.Message,
                    Status = 404,
                };
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (InvalidOperationException ex)
                when (ex.Message.Contains("No route matches the supplied values"))
                {
                    var pd = new ProblemDetails
                    {
                        Title = "Resource Not Found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = ex.Message,
                        Instance = ctx.Request.Path
                    };

                    ctx.Response.StatusCode = pd.Status.Value;
                    ctx.Response.ContentType = "application/problem+json";
                    await ctx.Response.WriteAsJsonAsync(pd);
                    return;
                }
            catch (DomainException dex)
            {
                Log.Error(dex, dex.Message);           // Serilog
                ctx.Response.StatusCode = dex.StatusCode;
                ctx.Response.ContentType = "application/problem+json";
                var pd = new ProblemDetails
                {
                    Title = dex.Message,
                    Status = dex.StatusCode,
                };
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception");           // Serilog
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/problem+json";
                var pd = new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Status = 500,
                    Detail = ex.Message                  // or hide in Prod
                };
                await ctx.Response.WriteAsJsonAsync(pd);
            }
        }
    }

}
