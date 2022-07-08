using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class InversionCarreraTecnicaController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<InversionCarreraTecnicaController> Logger;
        private readonly IMapper Mapper;

        public InversionCarreraTecnicaController(KalumDBContext _DbContext, ILogger<InversionCarreraTecnicaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnicaListDTO>>> Get()
        {
            List<InversionCarreraTecnica> inversionesCarreras = null;
            Logger.LogDebug("Iniciando consulta de inversione carrera tecnica");
            inversionesCarreras = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).ToListAsync();
            if(inversionesCarreras == null)
            {
                Logger.LogWarning("No existen inversiones de carreras tecnicas");
                return new NoContentResult();
            }
            else
            {
                List<InversionCarreraTecnicaListDTO> inversiones = Mapper.Map<List<InversionCarreraTecnicaListDTO>>(inversionesCarreras);
                Logger.LogInformation("Se ejecutó la petición de forma exitosa");
                return Ok(inversiones);
            }
        }

        [HttpGet("{id}", Name = "GetInversion")]
        public async Task<ActionResult<InversionCarreraTecnicaListDTO>> GetInversion(string id)
        {
            Logger.LogDebug($"Iniciando proceso de busqueda con el id {id}");
            var inversionCarrera = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).FirstOrDefaultAsync(ict => ict.InversionId == id);
            if(inversionCarrera == null)
            {
                Logger.LogWarning($"No existe la inversion con el id {id}");
                return new NoContentResult();
            }
            else
            {
                InversionCarreraTecnicaListDTO inversion = Mapper.Map<InversionCarreraTecnicaListDTO>(inversionCarrera);
                Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
                return Ok(inversion);
            }

        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnicaListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion inversion carrera tecnica");
            var queryable = DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).AsQueryable();
            var paginacion = new HttpResponsePaginacion<InversionCarreraTecnica>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0) 
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            List<InversionCarreraTecnicaListDTO> inversiones = Mapper.Map<List<InversionCarreraTecnicaListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginacion inversion carrera tecnica");
            return Ok(inversiones);            
        }

        [HttpPost]
        public async Task<ActionResult<InversionCarreraTecnica>> Post([FromBody] InversionCarreraTecnicaCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar una nueva inversion");
            CarreraTecnica carrera = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carrera == null)
            {
                Logger.LogWarning($"No existe carrera tecnica con el id {value.CarreraId}");
                return BadRequest();
            }
            InversionCarreraTecnica nuevo = Mapper.Map<InversionCarreraTecnica>(value);
            nuevo.InversionId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.InversionCarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una inversion");
            return new CreatedAtRouteResult("GetInversion", new {id = nuevo.InversionId}, value);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<InversionCarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if(inversion == null)
            {
                Logger.LogWarning($"No se encontro ninguna inversion con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.InversionCarreraTecnica.Remove(inversion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la inversion con el id {id}");
                return inversion;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la inversion con id {id}");
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if(inversion == null)
            {
                Logger.LogWarning($"No se encontro ninguna inversion con el id {id}");
                return NotFound();
            }
            else
            {
                inversion.MontoInscripcion = value.MontoInscripcion;
                inversion.NumeroPagos = value.NumeroPagos;
                inversion.MontoPago = value.MontoPago;
                DbContext.Entry(inversion).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }


        
    }
}