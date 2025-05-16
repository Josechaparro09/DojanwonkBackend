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
            if (await logica.Agregar(usuario))
            {
                return StatusCode(StatusCodes.Status201Created, usuario);
            }
            return BadRequest();
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
    }
}
