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
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        private readonly IMapper Mapper;

        public ExamenAdmisionController(KalumDBContext dbContext, ILogger<ExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionDTO>>> Get()
        {
            List<ExamenAdmision> examenes = null;
            Logger.LogDebug("Iniciando proceso de consulta examenes admisión");
            examenes = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();
            if(examenes == null || examenes.Count==0)
            {
                Logger.LogWarning("No existen examenes de admisión");
                return new NoContentResult();
            }
            List<ExamenAdmisionDTO> examenesAdmision = Mapper.Map<List<ExamenAdmisionDTO>>(examenes);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(examenesAdmision); 
        }
        
        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmisionDTO>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var examen = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examen == null)
            {
                Logger.LogWarning("No existe el examen de admisión con el id " + id);
                return new NoContentResult();
            }
            ExamenAdmisionDTO examenAdmision = Mapper.Map<ExamenAdmisionDTO>(examen);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(examenAdmision);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion examen admision");
            var queryable = DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).AsQueryable();
            var paginacion = new HttpResponsePaginacion<ExamenAdmision>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0) 
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            List<ExamenAdmisionDTO> examen = Mapper.Map<List<ExamenAdmisionDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginacion alumnos");
            return Ok(examen);            
        }

        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmisionCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar examen de admision nuevo");
            ExamenAdmision nuevo = Mapper.Map<ExamenAdmision>(value);
            nuevo.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar examen de admisión");
            return new CreatedAtRouteResult("GetExamenAdmision", new {id = nuevo.ExamenId}, value);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            ExamenAdmision examen = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.ExamenId == id);
            if(examen == null)
            {
                Logger.LogWarning($"No se encontro ningun examen de admision con el id {id}");
                return NotFound();
            }
            else if(aspirante != null)
            {
                Logger.LogWarning($"No se puede eliminar el examen de admisión con el id {id}, se encuentra asignado a un aspirante");
                return BadRequest();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examen);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el examen de admision con el id {id}");
                return examen;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del examen de admisión con el id {id}");
            ExamenAdmision examen = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examen == null)
            {
                Logger.LogWarning($"No se encontro ningun examen de admisión con el id {id}");
                return NotFound();
            }
            else
            {
                examen.FechaExamen = value.FechaExamen;
                DbContext.Entry(examen).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }
    }
}


