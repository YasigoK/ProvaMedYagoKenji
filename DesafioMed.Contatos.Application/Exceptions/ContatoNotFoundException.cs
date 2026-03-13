namespace DesafioMed.Contatos.Application.Exceptions;

public class ContatoNotFoundException : Exception
{
    public ContatoNotFoundException(string message) : base(message) { }
}