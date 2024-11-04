namespace Autenticacion.WebApi.Transversal.Excepciones;

public class TokenGenerationException : Exception
{
    public TokenGenerationException() { }
    public TokenGenerationException(string message) : base(message) { }
}
