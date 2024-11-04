using Autenticacion.WebApi.Aplicacion.Interfaces;
using Autenticacion.WebApi.Aplicacion.Validadores;
using Autenticacion.WebApi.Dominio.DTOs;
using Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;
using Autenticacion.WebApi.Dominio.Interfaces;
using Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;
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
            _logger.LogWarning("Se Encontraron errores de validación en modelo");
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
                       _logger.LogWarning("El Metodo de generar token recibio parametros vacio y eso lanza este error");
                }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario o Contraseña Incorrectos";
                    _logger.LogWarning("El usuario o la contraseña son incorrectos");
            }

        }catch (TokenGenerationException ex)
        { 
            response.IsSuccess = false;
            response.Message = $"Error al generar el token. {ex.Message}" ;
            _logger.LogError($"Ocurrio un error al intentar generar el token => {ex.Message} ***");
        }
        catch (InvalidOperationException ex)
        {
             response.IsSuccess = false;
             response.Message = $"Usuario no existe. {ex.Message}";
             _logger.LogError($"El usuario que intenta iniciar sesion no existe => {ex.Message} ***");
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error: {ex.Message}";
            _logger.LogError($"Ocurrio un error de servidor y se lanza una excepcion => {ex.Message} ***");
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
            // Validar el modelo
            var validation = _UsuarioDtoValidador.Validate(modelo);
            if (!validation.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Errores de validación";
                response.Errors = validation.Errors;
                _logger.LogWarning("Errores de validación en el modelo de usuario");
                return response;
            }

            // Verificar si ya existe un usuario con el correo proporcionado
            if (!string.IsNullOrEmpty(modelo.Correo))
            {
                var usuarioExistente = await _UsuarioRepositorio.ObtenerPorCorreo(modelo.Correo);
                if (usuarioExistente != null)
                {
                    response.IsSuccess = false;
                    response.Message = "El usuario ya existe";
                    _logger.LogWarning("El usuario ya existe en la base de datos");
                    return response;
                }
            }

            // Mapear el modelo DTO a la entidad de usuario
            var usuario = _mapper.Map<Usuario>(modelo);

            // Intentar guardar el usuario
            response.Data = await _UsuarioRepositorio.Guardar(usuario);

            Console.WriteLine(JsonConvert.SerializeObject(response.Data));

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Registro exitoso!";
                _logger.LogInformation("Usuario registrado exitosamente");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Hubo un error al crear el registro";
                _logger.LogWarning("Error al guardar el usuario en el repositorio");
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Ocurrió un error de servidor: {ex.Message}";
            _logger.LogError($"Ocurrió un error en el servidor: {ex.Message}");
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
