using ApiDevBP.Models;
using ApiDevBP.Response;
using ApiDevBP.Services;
using Microsoft.AspNetCore.Mvc;
/// <summary>
/// Controlador para la gestión de usuarios.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    [HttpPost]
    public IActionResult SaveUser([FromBody] UserModel user)
    {
        try
        {
            var result = _userService.SaveUser(user);
            var response = new Response<UserModel>
            {
                Success = true,
                Message = "Usuario guardado correctamente.",
                Data = user
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar guardar un usuario.");
            return StatusCode(500, new { ErrorMessage = "Error interno al intentar guardar el usuario." });
        }
    }

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    [HttpPut]
    public IActionResult UpdateUser([FromBody] UserModel user)
    {
        try
        {
            var result = _userService.UpdateUser(user);
           if(result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(new { ErrorMessage = $"Usuario con ID {user.id} no encontrado." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar modificar un usuario.");
            return StatusCode(500, new { ErrorMessage = "Error interno al intentar actualizar el usuario." });
        }
    }

    /// <summary>
    /// Obtiene la lista de usuarios.
    /// </summary>
    [HttpGet]
    public IActionResult GetUsers()
    {
        try
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar recuperar los usuarios.");
            return StatusCode(500, new { ErrorMessage = "Error interno al intentar recuperar los usuarios." });
        }
    }

    /// <summary>
    /// Obtiene un usuario por su ID.
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        try
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound(new { ErrorMessage = $"Usuario con ID {id} no encontrado." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al intentar obtener el usuario por ID: {id}.");
            return StatusCode(500, new { ErrorMessage = $"Error interno al intentar obtener el usuario por ID: {id}." });
        }
    }

    /// <summary>
    /// Elimina todos los usuarios.
    /// </summary>
    [HttpDelete]
    public IActionResult DeleteUsers()
    {
        try
        {
            _userService.DeleteUsers();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar borrar los usuarios.");
            return StatusCode(500, new { ErrorMessage = "Error interno al intentar borrar los usuarios." });
        }
    }

    /// <summary>
    /// Elimina un usuario por su ID.
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult DeleteUserById(int id)
    {
        try
        {
            if (_userService.DeleteUserById(id))
            {
                return Ok($"Usuario con ID {id} eliminado correctamente.");
            }
            else
            {
                return NotFound(new { ErrorMessage = $"Usuario no encontrado con ID: {id}" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al intentar eliminar el usuario por ID: {id}.");
            return StatusCode(500, new { ErrorMessage = $"Error interno al intentar eliminar el usuario por ID: {id}." });
        }
    }
}
