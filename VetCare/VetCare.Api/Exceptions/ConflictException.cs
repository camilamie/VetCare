namespace VetCare.Api.Exceptions;

/// <summary>
/// Lançada quando a operação conflita com o estado atual do recurso
/// (ex.: CPF duplicado). Convertida em HTTP 409.
/// </summary>
public class ConflictException(string mensagem) : Exception(mensagem);
