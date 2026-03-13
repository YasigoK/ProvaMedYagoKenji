using DesafioMed.Contatos.Contracts;

namespace DesafioMed.Contatos.Application.Service.Interface;

public interface IContatoService
{
    Task<IEnumerable<ContatoResponseDto>> ListarTodosContatos();
    Task<ContatoResponseDto> ObterContatoPorId(Guid id);
    Task<ContatoResponseDto> CriarContato(ContatoRequestDto Request);
    Task<ContatoResponseDto> AtualizarContato(Guid Id, ContatoRequestDto Request);
    Task RemoverContato(Guid Id);
    Task DesativarContato(Guid Id);

}
