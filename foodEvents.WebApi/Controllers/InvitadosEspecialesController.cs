using FoodEvents.Biblioteca;
using Microsoft.AspNetCore.Mvc;

namespace foodEvents.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitadosEspecialesController : ControllerBase
{
    private readonly FoodEventsService _service;

    public InvitadosEspecialesController(FoodEventsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvitadoEspecial>>> Get()
    {
        var invitados = await _service.ObtenerInvitadosEspecialesAsync();
        return Ok(invitados);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InvitadoEspecial>> GetById(int id)
    {
        var invitado = await _service.ObtenerInvitadoEspecialPorIdAsync(id);
        if (invitado is null)
        {
            return NotFound();
        }

        return Ok(invitado);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CrearInvitadoEspecialDto dto)
    {
        var invitado = new InvitadoEspecial
        {
            NombreCompleto = dto.NombreCompleto,
            CorreoElectronico = dto.CorreoElectronico,
            Telefono = dto.Telefono,
            TipoInvitado = dto.TipoInvitado,
            DescripcionPerfilPublico = dto.DescripcionPerfilPublico,
            ConfirmoAsistencia = dto.ConfirmoAsistencia
        };

        var resultado = await _service.CrearInvitadoEspecialAsync(invitado);
        if (!resultado.Exito)
        {
            return BadRequest(new { errores = resultado.Errores });
        }

        return CreatedAtAction(nameof(GetById), new { id = resultado.Valor!.Id }, resultado.Valor);
    }
}

public class CrearInvitadoEspecialDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public TipoInvitadoEspecial TipoInvitado { get; set; }
    public string? DescripcionPerfilPublico { get; set; }
    public bool ConfirmoAsistencia { get; set; }
}


