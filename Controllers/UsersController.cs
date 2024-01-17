using ApiDevBP.Entities;
using ApiDevBP.Models;
using ApiDevBP.Services;
using Microsoft.AspNetCore.Mvc;
using SQLite;
using System.Reflection;

namespace ApiDevBP.Controllers
{
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
        /// <param name="user">Información del nuevo usuario.</param>
        /// <returns>El nuevo usuario creado.</returns>
        [HttpPost]
        public async Task<IActionResult> SaveUser(UserModel user)
        {
            try
            {
                var result = _userService.SaveUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Puedes registrar la excepción para tener información sobre el error
                _logger.LogError(ex, "Error al intentar guardar un usuario.");

                // Puedes devolver un error 500 (Internal Server Error) u otra respuesta adecuada
                return StatusCode(500, "Error interno del servidor al intentar guardar el usuario.");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            try
            {
                var result = _userService.UpdateUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Puedes registrar la excepción para tener información sobre el error
                _logger.LogError(ex, "Error al intentar modificar un usuario.");

                // Puedes devolver un error 500 (Internal Server Error) u otra respuesta adecuada
                return StatusCode(500, "Error interno del servidor al intentar guardar el usuario.");
            }
        }
        /// <summary>
        /// Obtiene la lista de usuarios.
        /// </summary>
        /// <returns>Una lista de usuarios.</returns>

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Puedes registrar la excepción para tener información sobre el error
                _logger.LogError(ex, "Error al intentar recuperar los usuarios.");

                // Puedes devolver un error 500 (Internal Server Error) u otra respuesta adecuada
                return StatusCode(500, "Error interno del servidor al intentar recuperar los usuarios.");
            }

        }
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
                    return NotFound($"Usuario con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de manera adecuada
                return StatusCode(500, "Error interno del servidor al intentar obtener el usuario por ID.");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUsers()
        {
            try
            {
                _userService.DeleteUsers();
                return Ok();
            }
            catch (Exception ex)
            {
                // Puedes registrar la excepción para tener información sobre el error
                _logger.LogError(ex, "Error al intentar borrar los usuarios.");

                // Puedes devolver un error 500 (Internal Server Error) u otra respuesta adecuada
                return StatusCode(500, "Error interno del servidor al intentar borrar los usuarios.");
            }
        }


    }
}
