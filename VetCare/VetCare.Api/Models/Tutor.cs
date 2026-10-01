namespace VetCare.Api.Models;

/// <summary>
/// Responsável (dono) por um ou mais pets atendidos na clínica.
/// </summary>
public class Tutor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
