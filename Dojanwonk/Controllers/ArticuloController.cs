using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly ServiceArticulo serviceArticulo;
        public ArticuloController(ServiceArticulo serviceArticulo)
        {
            this.serviceArticulo = serviceArticulo;
        }
        [HttpPost]
        public async Task<ActionResult<Articulo>> Agregar(Articulo articulo)
        {
            try
            {
                if (await serviceArticulo.Agregar(articulo))
                {
                    return StatusCode(StatusCodes.Status201Created, articulo);
                }
                return BadRequest("No se pudo agregar el estudiante.");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Articulo>>> Leer()
        {
            return Ok(await  serviceArticulo.Leer());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Articulo>> Buscar(int id)
        {
            var buscado = await serviceArticulo.Buscar(id);
            if(buscado == null)
            {
                return NotFound();
            }
            return Ok(buscado);
        }
        [HttpDelete]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await serviceArticulo.Eliminar(id);
                return Ok("Articulo eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Actualizar(Articulo articulo)
        {
            try
            {
                await serviceArticulo.Actualizar(articulo
                    );
                return Ok("Articulo eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
