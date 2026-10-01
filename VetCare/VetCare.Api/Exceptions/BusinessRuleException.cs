namespace VetCare.Api.Exceptions;

/// <summary>
/// Lançada quando uma regra de negócio é violada. Convertida em HTTP 400.
/// </summary>
public class BusinessRuleException(string mensagem) : Exception(mensagem);
