using Autenticacion.WebApi.Aplicacion.Interfaces;
using Autenticacion.WebApi.Aplicacion.Validadores;
using Autenticacion.WebApi.Dominio.DTOs;
using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Dominio.Interfaces;
using Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;
using Autenticacion.WebApi.Dominio.Persistencia.Modelos;
using Autenticacion.WebApi.Transversal.Excepciones;
using Autenticacion.WebApi.Transversal.Interfaces;
using Autenticacion.WebApi.Transversal.Modelos;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Autenticacion.WebApi.Aplicacion.Servicios;

public class UsuarioServicio : IUsuarioServicio
{
    private readonly IUsuarioRepositorio _UsuarioRepositorio;
    private readonly UsuarioDtoValidador _UsuarioDtoValidador;
    private readonly UsuarioLoginDtoValidador _UsuarioLoginDtoValidador;
    private readonly IMenuRepositorio _MenuRepositorio;
    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly IAppLogger<UsuarioServicio> _logger;


    public UsuarioServicio(IMapper mapper, IAppLogger<UsuarioServicio> logger,IMenuRepositorio menuRepositorio, IOptions<AppSettings> appSettings, 
                            IUsuarioRepositorio usuarioRepositorio, UsuarioDtoValidador usuarioDtoValidador, UsuarioLoginDtoValidador usuarioLoginDtoValidador)
    {
        _mapper = mapper;
        _logger = logger;
        _MenuRepositorio = menuRepositorio;
        _appSettings = appSettings.Value;
        _UsuarioRepositorio = usuarioRepositorio;
        _UsuarioDtoValidador = usuarioDtoValidador;
        _UsuarioLoginDtoValidador = usuarioLoginDtoValidador;

    }

    public Task<Response<bool>> Actualizar(UsuarioDto modelo)
    {
        throw new NotImplementedException();
    }

   public async Task<Response<TokenDto>> IniciarSesion(UsuarioLoginDto modelo)
   {
        var response = new Response<TokenDto>();
        var validation = _UsuarioLoginDtoValidador.Validate(new UsuarioLoginDto() { Correo = modelo.Correo, Contraseña = modelo.Contraseña });


        if (!validation.IsValid)
        {
            response.Message = "Errores de validación encontrados";
            response.Errors = validation.Errors;
            return response;
        }

        try
            {
                var usuarioValidado = await _UsuarioRepositorio.ValidarUsuario(modelo);

                if (usuarioValidado != null)
                {
                    var menus = await _MenuRepositorio.ObtenerMenusPorRol(usuarioValidado.IdRol);

                    //  Console.WriteLine(JsonConvert.SerializeObject(menus));
                    if (usuarioValidado.NombreRol != null && usuarioValidado.Correo != null)
                    {

                        string token = GenerateJwtToken(usuarioValidado.IdUsuario, usuarioValidado.NombreRol, usuarioValidado.Correo, menus);
                        TokenDto TokenDto = new TokenDto { Token = token };

                        response.Data = TokenDto;
                        response.IsSuccess = true;
                        response.Message = "Autenticacion exitosa";
                        _logger.LogInformation("Autenticacion exitosa!!");
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "El nombre del rol y el correo no pueden ser nulos ni vacio";
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario o Contraseña Incorrectos";
                }

        }catch (TokenGenerationException ex)
        { 
                    response.IsSuccess = false;
                    response.Message = $"Error al generar el token. {ex.Message}" ;
        }
        catch (InvalidOperationException ex)
         {
                    response.IsSuccess = false;
                    response.Message = $"Usuario no existe. {ex.Message}";
         }


                 return response;
   }
    public Task<Response<bool>> Eliminar(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<bool>> Guardar(UsuarioDto modelo)
    {
        var response = new Response<bool>();

        try
        {
            var validation = _UsuarioDtoValidador.Validate(new UsuarioDto()
            {

                IdIndicativo = modelo.IdIndicativo,
                IdCiudad = modelo.IdCiudad,
                PrimerNombre = modelo.PrimerNombre,
                SegundoNombre = modelo.SegundoNombre,
                PrimerApellido = modelo.PrimerApellido,
                SegundoApellido = modelo.SegundoApellido,
                Telefono = modelo.Telefono,
                UsuarioQueRegistra = modelo.UsuarioQueRegistra,
                Correo = modelo.Correo,
                Contraseña = modelo.Contraseña,
                IpDeRegistro = modelo.IpDeRegistro
            });

            if (!validation.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Errores de validación";
                response.Errors = validation.Errors;
                return response;
            }



            var usuarioExistente = await _UsuarioRepositorio.ObtenerPorCorreo(modelo.Correo);

            if (usuarioExistente is not null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario ya existe";
                return response;
            }

            var usuario = _mapper.Map<Usuario>(modelo);

            var Usuario = await _UsuarioRepositorio.Guardar(usuario);

            if (Usuario is {})
            {
                response.IsSuccess = true;
                response.Message = "Registro exitoso!";
                _logger.LogInformation("Registro exitosa!!");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Hubo un error al crear el registro";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error: {ex.Message}";
            _logger.LogError(ex.Message);
        }

        return response;
    }

  
    private string GenerateJwtToken(long idUsuario, string nombreRol, string correo, List<MenuDto> menus)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Clave para firmar el token
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, idUsuario.ToString()),
            new Claim("Correo", correo),
            new Claim("Rol", nombreRol)
        };

        // Convertir los menús a JSON
        var menuJson = JsonConvert.SerializeObject(menus);

        // Añadir el menú encriptado al token como un nuevo claim
        claims.Add(new Claim("Menus", menuJson));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.Audience
        };

        try
        {
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el token: {ex.Message}");
            throw new TokenGenerationException();
        }
    }

}
