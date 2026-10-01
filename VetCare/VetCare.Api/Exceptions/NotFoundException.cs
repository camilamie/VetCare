namespace VetCare.Api.Exceptions;

/// <summary>
/// Lançada quando um recurso solicitado não existe. Convertida em HTTP 404.
/// </summary>
public class NotFoundException(string recurso, int id)
    : Exception($"{recurso} com Id {id} não foi encontrado(a).");
