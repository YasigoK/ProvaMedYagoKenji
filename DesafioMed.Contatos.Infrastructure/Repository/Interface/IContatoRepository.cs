using DesafioMed.Contatos.Domain.Entities;

namespace DesafioMed.Contatos.Infrastructure.Repositories.Interface;

public interface  IContatoRepository
{
    Task<IEnumerable<ContatoEntity>> ListarTodos();
    Task<ContatoEntity?> ObterPorId(Guid Id);
    void CriarContato(ContatoEntity contato);
    void AtualizarContato(ContatoEntity contato);
    void RemoverContato(ContatoEntity contato);
    Task Salvar();
}
