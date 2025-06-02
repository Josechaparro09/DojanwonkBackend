using BLL;
using DAL.Modelos;
using DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ServiceUsuario logica;
        public UsuarioController(ServiceUsuario logica)
        {
            this.logica = logica;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Leer()
        {
            return Ok(await logica.Leer());
        }
        [HttpPost]
        public async Task<ActionResult<Usuario>> Agregar(Usuario usuario)
        {
            try
            {
                if (await logica.Agregar(usuario))
                {
                    return StatusCode(StatusCodes.Status201Created, usuario);
                }
                return BadRequest("No se pudo agregar el usuario");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{cc}")]
        public async Task<ActionResult<Usuario>> Buscar(string cc)
        {
            var buscado = await logica.Buscar(cc);
            if (buscado == null)
            {
                return NotFound();
            }
            return Ok(buscado);
        }
        [HttpDelete("{cc}")]
        public async Task<ActionResult> Eliminar(string cc)
        {
            try
            {
                await logica.Eliminar(cc);
                return Ok("Usuario eliminado con exito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<Usuario>> Actualizar(Usuario usuario)
        {
            try
            {
                var actualizado = await logica.Actualizar(usuario);
                return Ok(actualizado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await logica.Login(usuarioDTO);
                if (usuario == null)
                {
                    return Unauthorized("Credenciales incorrectas");
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
