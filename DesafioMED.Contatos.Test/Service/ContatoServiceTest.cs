using DesafioMed.Contatos.Application.Exceptions;
using DesafioMed.Contatos.Application.Service;
using DesafioMed.Contatos.Contracts;
using DesafioMed.Contatos.Domain.Entities;
using DesafioMed.Contatos.Domain.Enums;
using DesafioMed.Contatos.Infrastructure.Repositories.Interface;

namespace DesafioMED.Contatos.Test.Service;

public class FakeContatoRepository : IContatoRepository
{
    public List<ContatoEntity> Contatos = new();
    public bool SalvarChamado = false;
    public bool CriarChamado = false;
    public bool AtualizarChamado = false;
    public bool RemoverChamado = false;

    public Task<IEnumerable<ContatoEntity>> ListarTodos() => Task.FromResult(Contatos.AsEnumerable());
    public Task<ContatoEntity?> ObterPorId(Guid Id) => Task.FromResult(Contatos.FirstOrDefault(x => x.Id == Id));
    public void CriarContato(ContatoEntity contato) { Contatos.Add(contato); CriarChamado = true; }
    public void AtualizarContato(ContatoEntity contato) => AtualizarChamado = true;
    public void RemoverContato(ContatoEntity contato) { Contatos.Remove(contato); RemoverChamado = true; }
    public Task Salvar() { SalvarChamado = true; return Task.CompletedTask; }
}

public class ContatoServiceTests
{
    private readonly FakeContatoRepository _fakeRepository;
    private readonly ContatoService _service;
    private readonly DateTime _dataNascimentoValida = DateTime.Today.AddYears(-20);

    public ContatoServiceTests()
    {
        _fakeRepository = new FakeContatoRepository();
        _service = new ContatoService(_fakeRepository);
    }

    [Fact]
    public async Task ListarTodosContatos_ExistemContatosSalvos_DeveRetornarListaDeResponseDto()
    {
        var contato = ContatoEntity.Criar("João", _dataNascimentoValida, SexoEnum.Masculino);
        _fakeRepository.Contatos.Add(contato);

        var result = await _service.ListarTodosContatos();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task ObterContatoPorId_ContatoExiste_DeveRetornarResponseDto()
    {
        var contato = ContatoEntity.Criar("Maria", _dataNascimentoValida, SexoEnum.Feminino);
        _fakeRepository.Contatos.Add(contato);

        var result = await _service.ObterContatoPorId(contato.Id);

        Assert.NotNull(result);
        Assert.Equal(contato.Id, result.Id);
    }

    [Fact]
    public async Task ObterContatoPorId_ContatoNaoExiste_DeveLancarContatoNotFoundException()
    {
        var idInexistente = Guid.NewGuid();

        Func<Task> acao = async () => await _service.ObterContatoPorId(idInexistente);

        await Assert.ThrowsAsync<ContatoNotFoundException>(acao);
    }

    [Fact]
    public async Task CriarContato_DadosValidos_DeveChamarRepositorioERetornarResponseDto()
    {
        var request = new ContatoRequestDto("Pedro", _dataNascimentoValida, SexoEnum.Masculino);

        var result = await _service.CriarContato(request);

        Assert.NotNull(result);
        Assert.True(_fakeRepository.CriarChamado);
        Assert.True(_fakeRepository.SalvarChamado);
    }

    [Fact]
    public async Task CriarContato_DadosNulos_DeveLancarNullReferenceExceptionOuDomainException()
    {
        ContatoRequestDto requestNulo = null!;

        Func<Task> acao = async () => await _service.CriarContato(requestNulo);

        await Assert.ThrowsAsync<NullReferenceException>(acao);
    }

    [Fact]
    public async Task AtualizarContato_ContatoExisteEDadosValidos_DeveAtualizarSalvarERetornarResponseDto()
    {
        var contatoOriginal = ContatoEntity.Criar("Original", _dataNascimentoValida, SexoEnum.Masculino);
        _fakeRepository.Contatos.Add(contatoOriginal);
        var requestAtualizacao = new ContatoRequestDto("Atualizado", _dataNascimentoValida, SexoEnum.Outro);

        var result = await _service.AtualizarContato(contatoOriginal.Id, requestAtualizacao);

        Assert.Equal("Atualizado", result.Nome);
        Assert.True(_fakeRepository.AtualizarChamado);
    }

    [Fact]
    public async Task AtualizarContato_ContatoNaoExiste_DeveLancarContatoNotFoundException()
    {
        var request = new ContatoRequestDto("Nome", _dataNascimentoValida, SexoEnum.Masculino);

        Func<Task> acao = async () => await _service.AtualizarContato(Guid.NewGuid(), request);

        await Assert.ThrowsAsync<ContatoNotFoundException>(acao);
    }

    [Fact]
    public async Task RemoverContato_ContatoExiste_DeveRemoverESalvar()
    {
        var contato = ContatoEntity.Criar("Nome", _dataNascimentoValida, SexoEnum.Masculino);
        _fakeRepository.Contatos.Add(contato);

        await _service.RemoverContato(contato.Id);

        Assert.True(_fakeRepository.RemoverChamado);
        Assert.Empty(_fakeRepository.Contatos);
    }
    
    [Fact]
    public async Task RemoverContato_ContatoNaoExiste_DeveLancarContatoNotFoundException()
    {
        var contato = ContatoEntity.Criar("Nome", _dataNascimentoValida, SexoEnum.Masculino);
        _fakeRepository.Contatos.Add(contato);

        Func<Task> acao = async () => await _service.RemoverContato(Guid.NewGuid());

        await Assert.ThrowsAsync<ContatoNotFoundException>(acao);
    }

    [Fact]
    public async Task DesativarContato_ContatoExiste_DeveChamarDesativarDaEntidadeESalvar()
    {
        var contato = ContatoEntity.Criar("Nome", _dataNascimentoValida, SexoEnum.Masculino);
        _fakeRepository.Contatos.Add(contato);

        await _service.DesativarContato(contato.Id);

        Assert.False(contato.Ativo);
        Assert.True(_fakeRepository.SalvarChamado);
    }
}