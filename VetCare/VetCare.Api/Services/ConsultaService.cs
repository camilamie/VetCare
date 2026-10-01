using Microsoft.EntityFrameworkCore;
using VetCare.Api.Data;
using VetCare.Api.DTOs.Consultas;
using VetCare.Api.Exceptions;
using VetCare.Api.Mappings;
using VetCare.Api.Models;
using VetCare.Api.Models.Enums;
using VetCare.Api.Services.Interfaces;

namespace VetCare.Api.Services;

public class ConsultaService(AppDbContext context) : IConsultaService
{
    private const string Recurso = "Consulta";

    public async Task<IReadOnlyList<ConsultaResponse>> ListarAsync(StatusConsulta? status, CancellationToken ct)
    {
        var query = context.Consultas.AsNoTracking();

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        return await query
            .OrderByDescending(c => c.DataHora)
            .Select(ConsultaMappings.ParaResponse)
            .ToListAsync(ct);
    }

    public async Task<ConsultaResponse> ObterPorIdAsync(int id, CancellationToken ct) =>
        await context.Consultas
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(ConsultaMappings.ParaResponse)
            .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException(Recurso, id);

    public async Task<ConsultaResponse> CriarAsync(ConsultaRequest request, CancellationToken ct)
    {
        await ValidarRequestAsync(request, ct);

        var consulta = new Consulta();
        consulta.AplicarDados(request);

        context.Consultas.Add(consulta);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(consulta.Id, ct);
    }

    public async Task<ConsultaResponse> AtualizarAsync(int id, ConsultaRequest request, CancellationToken ct)
    {
        var consulta = await context.Consultas.FindAsync([id], ct)
            ?? throw new NotFoundException(Recurso, id);

        if (consulta.Status == StatusConsulta.Cancelada)
            throw new BusinessRuleException("Consultas canceladas não podem ser alteradas.");

        await ValidarRequestAsync(request, ct);

        consulta.AplicarDados(request);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(id, ct);
    }

    public async Task RemoverAsync(int id, CancellationToken ct)
    {
        var consulta = await context.Consultas.FindAsync([id], ct)
            ?? throw new NotFoundException(Recurso, id);

        context.Consultas.Remove(consulta);
        await context.SaveChangesAsync(ct);
    }

    private async Task ValidarRequestAsync(ConsultaRequest request, CancellationToken ct)
    {
        var dataHora = request.DataHora!.Value;

        if (request.Status == StatusConsulta.Agendada && dataHora < DateTime.Now)
            throw new BusinessRuleException("Uma consulta com status 'Agendada' não pode ter data no passado.");

        if (request.Status == StatusConsulta.Realizada && dataHora > DateTime.Now)
            throw new BusinessRuleException("Uma consulta com status 'Realizada' não pode ter data no futuro.");

        if (!await context.Pets.AnyAsync(p => p.Id == request.PetId, ct))
            throw new BusinessRuleException($"O pet informado (Id {request.PetId}) não existe.");
    }
}
