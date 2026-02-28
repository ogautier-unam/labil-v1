using CrisisConnect.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CrisisConnect.API.Middleware;

/// <summary>
/// Convertit les exceptions domaine en réponses ProblemDetails standardisées (RFC 7807).
/// NotFoundException   → 404 Not Found
/// DomainException     → 400 Bad Request
/// ValidationException → 400 Bad Request (avec liste d'erreurs)
/// Toute autre         → 500 Internal Server Error
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Ressource introuvable : {Message}", ex.Message);
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Ressource introuvable", ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Erreur de validation");
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            var problem = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Erreur de validation",
                Instance = context.Request.Path
            };
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Règle métier violée : {Message}", ex.Message);
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Règle métier violée", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur inattendue");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError,
                "Erreur interne", "Une erreur inattendue s'est produite.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
