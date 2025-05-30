using BLL;
using DAL.Modelos;
using DTOS;
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
        public async Task<ActionResult> Agregar(Prestamo prestamo)
        {
            try
            {
                if (await servicePrestamo.Alquilar(prestamo))
                {
                    // Devuelve el DTO y no la entidad directamente
                    var dto = servicePrestamo.MapearPrestamoADTO(prestamo);
                    return StatusCode(StatusCodes.Status201Created, dto);
                }
                return BadRequest("No se pudo agregar el prestamo.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoDTO>>> Leer()
        {
            var prestamos = await servicePrestamo.Leer();

            // Mapeo cada prestamo a DTO
            var prestamosDTO = prestamos.Select(p => servicePrestamo.MapearPrestamoADTO(p)).ToList();

            return Ok(prestamosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoDTO>> Buscar(int id)
        {
            var buscado = await servicePrestamo.Buscar(id);

            if (buscado == null)
            {
                return NotFound();
            }

            var dto = servicePrestamo.MapearPrestamoADTO(buscado);
            return Ok(dto);
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

        [HttpPut("devolver/{id}")]
        public async Task<ActionResult> Devolver(int id)
        {
            try
            {
                bool devuelto = await servicePrestamo.Devolver(id);
                if (!devuelto)
                    return BadRequest("No se pudo devolver el préstamo o ya fue devuelto.");

                return Ok("Préstamo devuelto con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
