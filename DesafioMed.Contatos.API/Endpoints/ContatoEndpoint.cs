using Asp.Versioning;
using DesafioMed.Contatos.Application.Service.Interface;
using DesafioMed.Contatos.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMed.Contatos.API.Endpoints;

public static class ContatoEndpoints
{
    public static IEndpointRouteBuilder MapContatoEndpoints(this IEndpointRouteBuilder app)
    {
        var v1 = new ApiVersion(1, 0);
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(v1)
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/contatos")
            .WithTags("Contatos")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(v1);

        group.MapGet("/", async (IContatoService service) =>
        {
            var contatos = await service.ListarTodosContatos();
            return TypedResults.Ok(contatos);
        })
        .Produces<IEnumerable<ContatoResponseDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id:guid}", async (IContatoService service, Guid id) =>
        {
            var contato = await service.ObterContatoPorId(id);
            return TypedResults.Ok(contato);
        })
        .WithName("ObterContatoPorId")
        .Produces<ContatoResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("/", async (IContatoService service, ContatoRequestDto request) =>
        {
            var contato = await service.CriarContato(request);
            return TypedResults.CreatedAtRoute(contato, "ObterContatoPorId", new { id = contato.Id });
        })
        .Produces<ContatoResponseDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPut("/{id:guid}", async (IContatoService service, Guid id, ContatoRequestDto request) =>
        {
            var contato = await service.AtualizarContato(id, request);
            return TypedResults.Ok(contato);
        })
        .Produces<ContatoResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("/{id:guid}", async ( IContatoService service, Guid id) =>
        {
            await service.RemoverContato(id);
            return TypedResults.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPatch("/{id:guid}/inativar", async (IContatoService service, Guid id) =>
        {
            await service.DesativarContato(id);
            return TypedResults.Ok(new { message = "Contato desativado." });
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}