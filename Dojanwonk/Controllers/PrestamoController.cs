using BLL;
using DAL.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly ServicePrestamo servicePrestamo;
        public PrestamoController(ServicePrestamo servicePrestamo)
        {
            this.servicePrestamo = servicePrestamo;
        }
        [HttpPost]
        public async Task<ActionResult<Prestamo>> Agregar(Prestamo prestamo)
        {
            try
            {
                if (await servicePrestamo.Alquilar(prestamo))
                {
                    return StatusCode(StatusCodes.Status201Created, prestamo);
                }
                return BadRequest("No se pudo agregar el prestamo.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> Leer()
        {
            return Ok(await servicePrestamo.Leer());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> Buscar(int id)
        {
            var buscado = await servicePrestamo.Buscar(id);
            if (buscado == null)
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
                await servicePrestamo.Eliminar(id);
                return Ok("Prestamo eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Actualizar(Prestamo prestamo)
        {
            try
            {
                await servicePrestamo.Actualizar(prestamo);
                return Ok("Prestamo eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
