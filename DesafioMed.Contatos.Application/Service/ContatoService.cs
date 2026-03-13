using DesafioMed.Contatos.Application.Exceptions;
using DesafioMed.Contatos.Application.Service.Interface;
using DesafioMed.Contatos.Contracts;
using DesafioMed.Contatos.Domain.Entities;
using DesafioMed.Contatos.Infrastructure.Repositories.Interface;
namespace DesafioMed.Contatos.Application.Service;

public class ContatoService : IContatoService
{
    private readonly IContatoRepository _Repository;
    public ContatoService(IContatoRepository repository)=>_Repository=repository;
    public async Task<IEnumerable<ContatoResponseDto>> ListarTodosContatos()
    {
        var contato = await _Repository.ListarTodos();
        var result = contato.Select(x=>(ContatoResponseDto?)x).ToList();
        return result!;
    }

    public async Task<ContatoResponseDto> ObterContatoPorId(Guid Id)
    {
        var contato = await _Repository.ObterPorId(Id);
        if (contato is null)
            throw new ContatoNotFoundException("Contato não foi encontado.");

        return contato!;
    }
    public async Task<ContatoResponseDto> CriarContato(ContatoRequestDto Request)
    {
        ContatoEntity? entity = Request;

        _Repository.CriarContato(entity);
        await _Repository.Salvar();

        return entity!;
    }

    public async Task<ContatoResponseDto> AtualizarContato(Guid Id, ContatoRequestDto Request)
    {
        var entity = await _Repository.ObterPorId(Id);

        if (entity is null)
            throw new ContatoNotFoundException("Contato não foi encontrado.");

        entity.AtualizarContato(Request.Nome, Request.dataNascimento, Request.sexo);

        _Repository.AtualizarContato(entity);
        await _Repository.Salvar();

        return entity!;
    }

    public async Task RemoverContato(Guid Id)
    {
        var entity = await _Repository.ObterPorId(Id);

        if (entity is null)
            throw new ContatoNotFoundException("Contato não encontrado.");

        _Repository.RemoverContato(entity);
        await _Repository.Salvar();
    }

    public async Task DesativarContato(Guid Id)
    {
        var entity = await _Repository.ObterPorId(Id);

        if (entity is null)
            throw new ContatoNotFoundException("Contato não encontrado.");

        entity.Desativar();
        await _Repository.Salvar();
    }
}
