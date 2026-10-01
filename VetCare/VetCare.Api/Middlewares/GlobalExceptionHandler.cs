using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VetCare.Api.Exceptions;

namespace VetCare.Api.Middlewares;

/// <summary>
/// Captura todas as exceções não tratadas e devolve uma resposta padronizada
/// no formato ProblemDetails (RFC 9457), com o status code adequado.
/// </summary>
public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, titulo) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            BusinessRuleException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflito"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Erro não tratado ao processar {Metodo} {Caminho}",
                httpContext.Request.Method, httpContext.Request.Path);
        else
            logger.LogWarning("{Titulo}: {Mensagem}", titulo, exception.Message);

        var detalhe = statusCode == StatusCodes.Status500InternalServerError
            ? "Ocorreu um erro inesperado. Tente novamente mais tarde."
            : exception.Message;

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = titulo,
                Detail = detalhe,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }
        });
    }
}
