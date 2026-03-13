using DesafioMed.Contatos.Domain.Entities;
using DesafioMed.Contatos.Domain.Enums;
using DesafioMed.Contatos.Domain.Exceptions;
using System;
using Xunit;

namespace DesafioMED.Contatos.Test.Domain;

public class ContatoEntityTests
{
    private readonly DateTime _dataNascimentoValida = DateTime.Today.AddYears(-20);

    [Fact]
    public void Criar_DadosValidos_DeveRetornarInstanciaAtivaEComDadosCorretos()
    {
        var nome = "   nome teste  ";
        var sexo = SexoEnum.Masculino;

        var contato = ContatoEntity.Criar(nome, _dataNascimentoValida, sexo);

        Assert.NotNull(contato);
        Assert.Equal(nome.Trim(), contato.Nome);
        Assert.Equal(_dataNascimentoValida, contato.DataNascimento);
        Assert.Equal(sexo, contato.Sexo);
        Assert.True(contato.Ativo);
        Assert.True(contato.Idade >= 20);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Criar_NomeVazioOuNulo_DeveLancarDomainException(string? nomeInvalido)
    {
        Action acao = () => ContatoEntity.Criar(nomeInvalido!, _dataNascimentoValida, SexoEnum.Feminino);

        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("O Nome não pode ser vazio ou nulo.", exception.Message);
    }

    [Fact]
    public void Criar_DataDeNascimentoNoFuturo_DeveLancarDomainException()
    {
        var dataFutura = DateTime.Today.AddDays(1); 

        Action acao = () => ContatoEntity.Criar("Teste", dataFutura, SexoEnum.Outro);

        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("A data de nascimento não pode ser no futuro.", exception.Message);
    }

    [Fact]
    public void Criar_IdadeIgualAZero_DeveLancarDomainException()
    {
        var dataRecemNascido = DateTime.Today.AddDays(-350);

        Action acao = () => ContatoEntity.Criar("Teste", dataRecemNascido, SexoEnum.Masculino);

        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("A idade não pode ser igual a zero.", exception.Message);
    }

    [Fact]
    public void Criar_MenorDeIdade_DeveLancarDomainException()
    {
        var dataMenorIdade = DateTime.Today.AddYears(-17);

        Action acao = () => ContatoEntity.Criar("Teste", dataMenorIdade, SexoEnum.Feminino);

        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("O contato não pode ser menor de idade.", exception.Message);
    }

    [Fact]
    public void Criar_SexoInvalido_DeveLancarDomainException()
    {
        var sexoInvalido = (SexoEnum)99;

        Action acao = () => ContatoEntity.Criar("Teste", _dataNascimentoValida, sexoInvalido);

        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("Opção de sexo invalido.", exception.Message);
    }

    [Fact]
    public void Criar_ComNomeUltrapassandoLimiteDeCharacteres_DeveLancarDomainException()
    {
        var nome256 = (new string('N', 256));
        var sexo = SexoEnum.Feminino;

        Action acao = () => ContatoEntity.Criar(nome256, _dataNascimentoValida, sexo);
        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("Limite de caractéres 255 charactéres atingidos.", exception.Message);
    }

    [Fact]
    public void AtualizarContato_DadosValidos_DeveModificarPropriedades()
    {
        var contato = ContatoEntity.Criar("Original", _dataNascimentoValida, SexoEnum.Masculino);
        var novoNome = "   Alterado   ";
        var novaData = DateTime.Today.AddYears(-25);

        contato.AtualizarContato(novoNome, novaData, SexoEnum.Outro);

        Assert.Equal(novoNome.Trim(), contato.Nome);
        Assert.Equal(novaData, contato.DataNascimento);
        Assert.Equal(SexoEnum.Outro, contato.Sexo);
    }

    [Fact]
    public void AtualizarContato_TentarAtualizarContatoInativo_DeveLancarDomainException()
    {
        var contato = ContatoEntity.Criar("Original", _dataNascimentoValida, SexoEnum.Masculino);
        contato.Desativar();

        var acao = () => contato.AtualizarContato("Novo", _dataNascimentoValida, SexoEnum.Masculino);

        var exception = Assert.Throws<DomainException>(acao);
        Assert.Equal("O contato esta desativado, não será possível atualiza-lo.", exception.Message);

    }

    [Fact]
    public void Desativar_ContatoAtivo_DeveAlterarPropriedadeAtivoParaFalse()
    {
        var contato = ContatoEntity.Criar("Original", _dataNascimentoValida, SexoEnum.Masculino);

        contato.Desativar();

        Assert.False(contato.Ativo);
    }
}