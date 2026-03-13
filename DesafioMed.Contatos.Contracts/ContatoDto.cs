using DesafioMed.Contatos.Domain.Entities;
using DesafioMed.Contatos.Domain.Enums;

namespace DesafioMed.Contatos.Contracts;

public sealed record ContatoRequestDto(string Nome, DateTime dataNascimento, SexoEnum sexo)
{
    public static implicit operator ContatoEntity(ContatoRequestDto dto)
        => ContatoEntity.Criar(dto.Nome, dto.dataNascimento, dto.sexo);
}

public sealed record ContatoResponseDto(Guid Id, string Nome, DateTime DataNascimento, SexoEnum Sexo, bool Ativo, int Idade)
{
    public static implicit operator ContatoResponseDto(ContatoEntity e)
        => new(e.Id, e.Nome, e.DataNascimento, e.Sexo, e.Ativo, e.Idade);
}