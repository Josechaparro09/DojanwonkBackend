using BLL;
using DAL.Modelos;
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
            catch(Exception ex) { 
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("{cc}")]
        public async Task<ActionResult<Usuario>> Buscar(string cc)
        {
            var buscado = await logica.Buscar(cc);
            if (buscado== null)
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
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Actualizar(Usuario usuario)
        {
            try
            {
                await logica.Actualizar(usuario);
                return Ok("Usuario Actualizado con exito");
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
