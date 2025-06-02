using BLL;
using DAL.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Collections;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly ServiceEstudiante serviceEstudiante;
        private readonly ServicePago pago;
        public EstudianteController(ServiceEstudiante serviceEstudiante, ServicePago pago)
        {
            this.serviceEstudiante = serviceEstudiante;
            this.pago = pago;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> Leer()
        {
            return Ok(await(serviceEstudiante.Leer()));
        }
        [HttpPost]
        public async Task<ActionResult<Estudiante>> Agregar(Estudiante agregar)
        {
            try
            {
                Estudiante estudiante = await serviceEstudiante.Agregar(agregar);
                if(estudiante!= null)
                {
                    Pago pago = new Pago
                    {
                        IdEstudianteNavigation = estudiante,
                        FechaPago = DateOnly.FromDateTime(DateTime.Now),
                        Estado = "pagado"
                    };
                    await this.pago.AgregarPago(pago);
                    return StatusCode(StatusCodes.Status201Created, estudiante);
                }
                    
                return BadRequest("No se pudo agregar el estudiante.");
            }
            catch (Exception ex)
            {
                 return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<Estudiante>> Actualizar(Estudiante estudiante)
        {
            try
            {
                var actualizado =await serviceEstudiante.Actualizar(estudiante);
                return Ok(actualizado);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(string id)
        {
            try
            {
                await serviceEstudiante.Eliminar(id);
                return Ok("Eliminado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> Buscar(string id)
        {
            var buscado = await serviceEstudiante.Buscar(id);
            if ( buscado== null)
            {
                return NotFound();
            }

            return Ok(buscado);
        }
    }
}
