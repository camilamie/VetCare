using Microsoft.EntityFrameworkCore;
using VetCare.Api.Data;
using VetCare.Api.DTOs.Pets;
using VetCare.Api.DTOs.Tutores;
using VetCare.Api.Exceptions;
using VetCare.Api.Mappings;
using VetCare.Api.Models;
using VetCare.Api.Services.Interfaces;

namespace VetCare.Api.Services;

public class TutorService(AppDbContext context) : ITutorService
{
    private const string Recurso = "Tutor";

    public async Task<IReadOnlyList<TutorResponse>> ListarAsync(CancellationToken ct) =>
        await context.Tutores
            .AsNoTracking()
            .OrderBy(t => t.Nome)
            .Select(TutorMappings.ParaResponse)
            .ToListAsync(ct);

    public async Task<TutorResponse> ObterPorIdAsync(int id, CancellationToken ct) =>
        await context.Tutores
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(TutorMappings.ParaResponse)
            .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException(Recurso, id);

    public async Task<IReadOnlyList<PetResponse>> ListarPetsAsync(int tutorId, CancellationToken ct)
    {
        await GarantirQueExisteAsync(tutorId, ct);

        return await context.Pets
            .AsNoTracking()
            .Where(p => p.TutorId == tutorId)
            .OrderBy(p => p.Nome)
            .Select(PetMappings.ParaResponse)
            .ToListAsync(ct);
    }

    public async Task<TutorResponse> CriarAsync(TutorRequest request, CancellationToken ct)
    {
        await ValidarCpfUnicoAsync(request.Cpf, idIgnorado: null, ct);

        var tutor = new Tutor { DataCadastro = DateTime.UtcNow };
        tutor.AplicarDados(request);

        context.Tutores.Add(tutor);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(tutor.Id, ct);
    }

    public async Task<TutorResponse> AtualizarAsync(int id, TutorRequest request, CancellationToken ct)
    {
        var tutor = await context.Tutores.FindAsync([id], ct)
            ?? throw new NotFoundException(Recurso, id);

        await ValidarCpfUnicoAsync(request.Cpf, idIgnorado: id, ct);

        tutor.AplicarDados(request);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(id, ct);
    }

    public async Task RemoverAsync(int id, CancellationToken ct)
    {
        var tutor = await context.Tutores.FindAsync([id], ct)
            ?? throw new NotFoundException(Recurso, id);

        var possuiPets = await context.Pets.AnyAsync(p => p.TutorId == id, ct);
        if (possuiPets)
            throw new ConflictException(
                "Não é possível remover o tutor pois existem pets vinculados a ele. Remova ou transfira os pets antes.");

        context.Tutores.Remove(tutor);
        await context.SaveChangesAsync(ct);
    }

    private async Task GarantirQueExisteAsync(int id, CancellationToken ct)
    {
        if (!await context.Tutores.AnyAsync(t => t.Id == id, ct))
            throw new NotFoundException(Recurso, id);
    }

    private async Task ValidarCpfUnicoAsync(string cpf, int? idIgnorado, CancellationToken ct)
    {
        var cpfEmUso = await context.Tutores
            .AnyAsync(t => t.Cpf == cpf && (idIgnorado == null || t.Id != idIgnorado), ct);

        if (cpfEmUso)
            throw new ConflictException($"Já existe um tutor cadastrado com o CPF {cpf}.");
    }
}
