using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VetCare.Api.DTOs.Pets;
using VetCare.Api.DTOs.Tutores;
using VetCare.Api.Services.Interfaces;

namespace VetCare.Api.Controllers.V1;

/// <summary>
/// Gerenciamento de tutores (donos dos pets).
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tutores")]
[Produces("application/json")]
public class TutoresController(ITutorService tutorService) : ControllerBase
{
    /// <summary>Lista todos os tutores cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TutorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorResponse>>> Listar(CancellationToken ct) =>
        Ok(await tutorService.ListarAsync(ct));

    /// <summary>Busca um tutor pelo Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorResponse>> ObterPorId(int id, CancellationToken ct) =>
        Ok(await tutorService.ObterPorIdAsync(id, ct));

    /// <summary>Lista os pets de um tutor.</summary>
    [HttpGet("{id:int}/pets")]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PetResponse>>> ListarPets(int id, CancellationToken ct) =>
        Ok(await tutorService.ListarPetsAsync(id, ct));

    /// <summary>Cadastra um novo tutor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TutorResponse>> Criar([FromBody] TutorRequest request, CancellationToken ct)
    {
        var tutor = await tutorService.CriarAsync(request, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = tutor.Id, version = "1" }, tutor);
    }

    /// <summary>Atualiza os dados de um tutor.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TutorResponse>> Atualizar(int id, [FromBody] TutorRequest request, CancellationToken ct) =>
        Ok(await tutorService.AtualizarAsync(id, request, ct));

    /// <summary>Remove um tutor (somente se não possuir pets vinculados).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await tutorService.RemoverAsync(id, ct);
        return NoContent();
    }
}
