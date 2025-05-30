using BLL;
using DAL.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {

    private readonly ServicePago servicePago;
        public PagoController(ServicePago servicePago)
        {
            this.servicePago = servicePago;
        }
        [HttpPut]
        public async Task<IActionResult> ActualizarPago(Pago pago)
        {
            var actualizado = await servicePago.ActualizarPago(pago);
            return Ok(actualizado);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> LeerPagos()
        {
            return Ok(await servicePago.Leer());
        }
        [HttpPost("generarPagos")]
        public async Task<IActionResult> GenerarPagos()
        {
            await servicePago.GenerarPagos();
            return Ok("Pagos generados.");
        }
    }
}
