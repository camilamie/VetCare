using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VetCare.Api.DTOs.Consultas;
using VetCare.Api.Models.Enums;
using VetCare.Api.Services.Interfaces;

namespace VetCare.Api.Controllers.V1;

/// <summary>
/// Gerenciamento de consultas veterinárias.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consultas")]
[Produces("application/json")]
public class ConsultasController(IConsultaService consultaService) : ControllerBase
{
    /// <summary>Lista as consultas, com filtro opcional por status.</summary>
    /// <param name="status">Agendada, Realizada ou Cancelada</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConsultaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ConsultaResponse>>> Listar([FromQuery] StatusConsulta? status, CancellationToken ct) =>
        Ok(await consultaService.ListarAsync(status, ct));

    /// <summary>Busca uma consulta pelo Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ConsultaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConsultaResponse>> ObterPorId(int id, CancellationToken ct) =>
        Ok(await consultaService.ObterPorIdAsync(id, ct));

    /// <summary>Agenda (cadastra) uma nova consulta.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConsultaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConsultaResponse>> Criar([FromBody] ConsultaRequest request, CancellationToken ct)
    {
        var consulta = await consultaService.CriarAsync(request, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = consulta.Id, version = "1" }, consulta);
    }

    /// <summary>Atualiza uma consulta (ex.: registrar diagnóstico ou alterar status).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ConsultaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConsultaResponse>> Atualizar(int id, [FromBody] ConsultaRequest request, CancellationToken ct) =>
        Ok(await consultaService.AtualizarAsync(id, request, ct));

    /// <summary>Remove uma consulta.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await consultaService.RemoverAsync(id, ct);
        return NoContent();
    }
}
