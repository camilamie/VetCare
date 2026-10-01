using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VetCare.Api.DTOs.Consultas;
using VetCare.Api.DTOs.Pets;
using VetCare.Api.Services.Interfaces;

namespace VetCare.Api.Controllers.V1;

/// <summary>
/// Gerenciamento de pets.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pets")]
[Produces("application/json")]
public class PetsController(IPetService petService) : ControllerBase
{
    /// <summary>Lista os pets, com filtro opcional por espécie.</summary>
    /// <param name="especie">Ex.: Cachorro, Gato</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PetResponse>>> Listar([FromQuery] string? especie, CancellationToken ct) =>
        Ok(await petService.ListarAsync(especie, ct));

    /// <summary>Busca um pet pelo Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetResponse>> ObterPorId(int id, CancellationToken ct) =>
        Ok(await petService.ObterPorIdAsync(id, ct));

    /// <summary>Lista o histórico de consultas de um pet.</summary>
    [HttpGet("{id:int}/consultas")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ConsultaResponse>>> ListarConsultas(int id, CancellationToken ct) =>
        Ok(await petService.ListarConsultasAsync(id, ct));

    /// <summary>Cadastra um novo pet.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PetResponse>> Criar([FromBody] PetRequest request, CancellationToken ct)
    {
        var pet = await petService.CriarAsync(request, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = pet.Id, version = "1" }, pet);
    }

    /// <summary>Atualiza os dados de um pet.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetResponse>> Atualizar(int id, [FromBody] PetRequest request, CancellationToken ct) =>
        Ok(await petService.AtualizarAsync(id, request, ct));

    /// <summary>Remove um pet e todas as suas consultas.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await petService.RemoverAsync(id, ct);
        return NoContent();
    }
}
