using Microsoft.EntityFrameworkCore;
using VetCare.Api.Data;
using VetCare.Api.DTOs.Consultas;
using VetCare.Api.DTOs.Pets;
using VetCare.Api.Exceptions;
using VetCare.Api.Mappings;
using VetCare.Api.Models;
using VetCare.Api.Services.Interfaces;

namespace VetCare.Api.Services;

public class PetService(AppDbContext context) : IPetService
{
    private const string Recurso = "Pet";

    public async Task<IReadOnlyList<PetResponse>> ListarAsync(string? especie, CancellationToken ct)
    {
        var query = context.Pets.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(especie))
        {
            var filtro = especie.Trim().ToLower();
            query = query.Where(p => p.Especie.ToLower() == filtro);
        }

        return await query
            .OrderBy(p => p.Nome)
            .Select(PetMappings.ParaResponse)
            .ToListAsync(ct);
    }

    public async Task<PetResponse> ObterPorIdAsync(int id, CancellationToken ct) =>
        await context.Pets
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(PetMappings.ParaResponse)
            .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException(Recurso, id);

    public async Task<IReadOnlyList<ConsultaResponse>> ListarConsultasAsync(int petId, CancellationToken ct)
    {
        if (!await context.Pets.AnyAsync(p => p.Id == petId, ct))
            throw new NotFoundException(Recurso, petId);

        return await context.Consultas
            .AsNoTracking()
            .Where(c => c.PetId == petId)
            .OrderByDescending(c => c.DataHora)
            .Select(ConsultaMappings.ParaResponse)
            .ToListAsync(ct);
    }

    public async Task<PetResponse> CriarAsync(PetRequest request, CancellationToken ct)
    {
        await ValidarRequestAsync(request, ct);

        var pet = new Pet();
        pet.AplicarDados(request);

        context.Pets.Add(pet);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(pet.Id, ct);
    }

    public async Task<PetResponse> AtualizarAsync(int id, PetRequest request, CancellationToken ct)
    {
        var pet = await context.Pets.FindAsync([id], ct)
            ?? throw new NotFoundException(Recurso, id);

        await ValidarRequestAsync(request, ct);

        pet.AplicarDados(request);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(id, ct);
    }

    public async Task RemoverAsync(int id, CancellationToken ct)
    {
        var pet = await context.Pets.FindAsync([id], ct)
            ?? throw new NotFoundException(Recurso, id);

        // As consultas do pet são removidas em cascata (configurado no PetConfiguration)
        context.Pets.Remove(pet);
        await context.SaveChangesAsync(ct);
    }

    private async Task ValidarRequestAsync(PetRequest request, CancellationToken ct)
    {
        if (request.DataNascimento.HasValue && request.DataNascimento.Value.Date > DateTime.Today)
            throw new BusinessRuleException("A data de nascimento do pet não pode estar no futuro.");

        if (!await context.Tutores.AnyAsync(t => t.Id == request.TutorId, ct))
            throw new BusinessRuleException($"O tutor informado (Id {request.TutorId}) não existe.");
    }
}
