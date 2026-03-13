using DesafioMed.Contatos.Domain.Entities;
using DesafioMed.Contatos.Infrastructure.Context;
using DesafioMed.Contatos.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DesafioMed.Contatos.Infrastructure.Repositories;

public class ContatoRepository : IContatoRepository
{
    private readonly ContatoContext _context;

    public ContatoRepository(ContatoContext context) => _context = context;

    public async Task<IEnumerable<ContatoEntity>> ListarTodos()
    {
        return await _context.ContatoProva.AsNoTracking().Where(x=>x.Ativo==true).ToListAsync();
    }

    public async Task<ContatoEntity?> ObterPorId(Guid Id)
    {
        return await _context.ContatoProva.FirstOrDefaultAsync(x => x.Id == Id && x.Ativo);
    }
    public void CriarContato(ContatoEntity contato)
    {
        _context.ContatoProva.Add(contato);
    }

    public void AtualizarContato(ContatoEntity contato)
    {
        _context.ContatoProva.Update(contato);

    }

    public void RemoverContato(ContatoEntity contato)
    {
        _context.ContatoProva.Remove(contato);

    }

    public async Task Salvar()
    {
        await _context.SaveChangesAsync();
    }
}
