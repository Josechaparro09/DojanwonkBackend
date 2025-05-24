using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenController : ControllerBase
    {
        private readonly ServiceExamen serviceExamen;
        public ExamenController(ServiceExamen serviceExamen)
        {
            this.serviceExamen = serviceExamen;
        }
        [HttpPost]
        public async Task<ActionResult<Examen>> Registrar(Examen examen)
        {
            try
            {
                await serviceExamen.Registrar(examen);
                return StatusCode(StatusCodes.Status201Created, examen);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Examen>>> Leer()
        {
            return Ok(await serviceExamen.Leer());
        }
        [HttpPut]
        public async Task<ActionResult> Actualizar(Examen examen)
        {
            try
            {
                await serviceExamen.Actualizar(examen);
                return Ok("Actualizado con exito");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
