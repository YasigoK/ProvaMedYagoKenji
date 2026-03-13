using DesafioMed.Contatos.Domain.Enums;
using DesafioMed.Contatos.Domain.Exceptions;

namespace DesafioMed.Contatos.Domain.Entities
{
    public class ContatoEntity : Entity
    {
        public string Nome { get; private set; } = default!;
        public DateTime DataNascimento { get; private set; }
        public SexoEnum Sexo { get; private set; }
        public bool Ativo { get; private set; }
        public int Idade => CalcularIdade(DataNascimento);

        protected ContatoEntity(){}

        private ContatoEntity(string nome, DateTime dataNascimento, SexoEnum sexo)
        {
            Nome = nome?.Trim()??string.Empty;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            Ativo = true; 
            Validar();
        }
        public static ContatoEntity Criar(string nome, DateTime dataNascimento, SexoEnum sexo)
        {
            return new ContatoEntity(nome, dataNascimento, sexo);
        }

        public void AtualizarContato(string nome, DateTime dataNascimento, SexoEnum sexo)
        {
            if (!Ativo)
                throw new DomainException("O contato esta desativado, não será possível atualiza-lo."); 

            Nome = nome?.Trim() ?? string.Empty;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            Validar();
        }

        public void Desativar() => Ativo = false;

        private int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;

            if (dataNascimento.Date > hoje.AddYears(-idade)) 
                idade--;

            return idade;
        }
        private void Validar()
        {
            int Idade = CalcularIdade(this.DataNascimento);

            if (Idade == 0)
                throw new DomainException("A idade não pode ser igual a zero.");

            if (this.DataNascimento > DateTime.Today)
                throw new DomainException("A data de nascimento não pode ser no futuro.");

            if (Idade < 18)
                throw new DomainException("O contato não pode ser menor de idade.");

            if (string.IsNullOrEmpty(this.Nome))
                throw new DomainException("O Nome não pode ser vazio ou nulo.");

            if (this.Nome.Length > 255)
                throw new DomainException("Limite de caractéres 255 charactéres atingidos.");

            if (!Enum.IsDefined(typeof(SexoEnum), this.Sexo))
                throw new DomainException("Opção de sexo invalido.");

            
        }

    }
}
