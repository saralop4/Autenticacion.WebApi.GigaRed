using Autenticacion.WebApi.Aplicacion.Interfaces;
using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Autenticacion.WebApi.Controllers.V1;

[Route("Api/V{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioServicio _IUsuarioServicio;
    public UsuarioController(IUsuarioServicio UsuarioServicio)
    {
        _IUsuarioServicio = UsuarioServicio;
    }


    [HttpPost("IniciarSesion")]
    public async Task<IActionResult> IniciarSesion([FromBody] UsuarioLoginDto dto)
    {
        var response = await _IUsuarioServicio.IniciarSesion(dto);

        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);

    }

    [HttpPost("GuardarUsuario")]
    public async Task<IActionResult> GuardarUsuario([FromBody] UsuarioDto usuarioDto)
    {
        if (usuarioDto == null)
        {
            return BadRequest(new { Mensaje = "El objeto usuarioDto es requerido." });
        }

        var ipDeRegistro = HttpContext.Connection.RemoteIpAddress?.ToString();

        if (ipDeRegistro != null)
        {
            usuarioDto.IpDeRegistro = ipDeRegistro;
        }

        //Console.WriteLine(JsonConvert.SerializeObject(usuarioDto));

        var response = await _IUsuarioServicio.Guardar(usuarioDto);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}
