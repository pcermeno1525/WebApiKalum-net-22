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
    public class JornadaController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        private readonly IMapper Mapper;

        public JornadaController(KalumDBContext dbContext, ILogger<JornadaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso consulta jornadas");
            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();
            if(jornadas == null || jornadas.Count==0)
            {
                Logger.LogWarning("No existen jornadas");
                return new NoContentResult();
            }
            List<JornadaListDTO> horarios = Mapper.Map<List<JornadaListDTO>>(jornadas);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(horarios);
        }

        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<JornadaListDTO>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando proceso de busqueda con el id " + id);
            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el id " + id);
                return new NoContentResult();
            }
            JornadaListDTO horario = Mapper.Map<JornadaListDTO>(jornada);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(horario);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion jornada");
            var queryable = DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Jornada>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0) 
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            List<JornadaListDTO> jornadas = Mapper.Map<List<JornadaListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginacion jornada");
            return Ok(jornadas);            
        }

        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] JornadaCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar una jornada nueva");
            Jornada nuevo = Mapper.Map<Jornada>(value);
            nuevo.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una jornada");
            return new CreatedAtRouteResult("GetJornada", new {id = nuevo.JornadaId}, value);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.JornadaId == id);
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No se encontro ninguna jornada con el id {id}");
                return NotFound();
            }
            else if(aspirante != null)
            {
                Logger.LogWarning($"No se puede eliminar la jornada con el id {id}, se encuentra asignada a uno o mas aspirantes");
                return BadRequest();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la jornada con el id {id}");
                return jornada;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la jornada con id {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No se encontro ninguna jornada con el id {id}");
                return NotFound();
            }
            else
            {
                jornada.Nombre = value.Nombre;
                jornada.Descripcion = value.Descripcion;
                DbContext.Entry(jornada).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }
        
    }
}