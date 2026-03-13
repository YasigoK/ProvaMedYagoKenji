using System.Net;
using DesafioMed.Contatos.Domain.Exceptions;
using DesafioMed.Contatos.Application.Exceptions;

namespace DesafioMed.Contatos.API.Middleware;

public record ErrorResponse(int StatusCode, string Mensagem);

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)=>_next=next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException e)
        {
            context.Response.ContentType= "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorResponse(400, e.Message));
        }
        catch (ContatoNotFoundException e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorResponse(404, e.Message));
        }
        catch (Exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse(500, "Ocorreu um erro interno no servidor."));
        }
    }
}
   