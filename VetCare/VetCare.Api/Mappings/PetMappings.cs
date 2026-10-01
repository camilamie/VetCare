using System.Linq.Expressions;
using VetCare.Api.DTOs.Pets;
using VetCare.Api.Models;

namespace VetCare.Api.Mappings;

public static class PetMappings
{
    public static readonly Expression<Func<Pet, PetResponse>> ParaResponse = p =>
        new PetResponse(p.Id, p.Nome, p.Especie, p.Raca, p.DataNascimento, p.PesoKg, p.TutorId, p.Tutor.Nome);

    public static void AplicarDados(this Pet pet, PetRequest request)
    {
        pet.Nome = request.Nome.Trim();
        pet.Especie = request.Especie.Trim();
        pet.Raca = string.IsNullOrWhiteSpace(request.Raca) ? null : request.Raca.Trim();
        pet.DataNascimento = request.DataNascimento;
        pet.PesoKg = request.PesoKg;
        pet.TutorId = request.TutorId;
    }
}
