using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;
using WebApiKalum.Dtos;
using AutoMapper;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;
        public CarreraTecnicaController(KalumDBContext _DbContext, ILogger<CarreraTecnicaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;            
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            Logger.LogDebug("Iniciando proceso de consulta de carreras técnicas en la DB");

            // Tarea 1
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).ToListAsync();
            
            // Tarea 2
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                Logger.LogWarning("No existen carreras técnicas");
                return new NoContentResult();
            }
            List<CarreraTecnicaListDTO> carreras = Mapper.Map<List<CarreraTecnicaListDTO>>(carrerasTecnicas);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(carreras);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.CarreraTecnica.Include(ct => ct.Aspirantes).Include(ct => ct.Inscripciones).AsQueryable();
            var paginacion = new HttpResponsePaginacion<CarreraTecnica>(queryable,page);
            if(paginacion.Content == null && paginacion.Content.Count ==0)
            {
                return NoContent();
            }
            List<CarreraTecnicaListDTO> carrera = Mapper.Map<List<CarreraTecnicaListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizacion proceso de paginacion carreras tecnicas");
            return Ok(carrera);
        }

        [HttpGet("{id}", Name = "GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnicaListDTO>> GetCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carrera == null)
            {
                Logger.LogWarning("No existe la carrera técnica con el id " + id);
                return new NoContentResult();
            }
            CarreraTecnicaListDTO carreraTecnica = Mapper.Map<CarreraTecnicaListDTO>(carrera);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(carreraTecnica);

        }
        [HttpPost]
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnicaCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar una carrera técnica nueva");

            CarreraTecnica nuevo = Mapper.Map<CarreraTecnica>(value);
            nuevo.CarreraId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.CarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una carrera técnica");
            return new CreatedAtRouteResult("GetCarreraTecnica",new {id = nuevo.CarreraId}, value);
        }
        [HttpDelete("{id}")]        
        public async Task<ActionResult<CarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminación");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.CarreraId == id);
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(i => i.CarreraId == id); 
            if(carreraTecnica == null)
            {
                Logger.LogWarning($"No se encontro ninguna carrera técnica con el id {id}");
                return NotFound();                
            }
            else if(aspirante != null)
            {
                Logger.LogWarning($"No se puede eliminar la carrera tecnica con el id {id}, se encuentra asignada a uno o mas aspirantes");
                return BadRequest();
            }
            else if(inversion != null)
            {
                Logger.LogWarning($"No se puede eliminar la carrera tecnica con el id {id}, se encuentra asignada a una o mas inversiones");
                return BadRequest();
            }
            else
            {
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");
                return carreraTecnica;
            }
        }
        [HttpPut("{id}")]

        public async Task<ActionResult> Put(string id, [FromBody] CarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la carrera técnica con el id {id}");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                Logger.LogWarning($"No se encontro ninguna carrera técnica con el id {id}");
                return BadRequest();                
            }
            else
            {
                carreraTecnica.Nombre = value.Nombre;
                DbContext.Entry(carreraTecnica).State = EntityState.Modified;
                await DbContext.SaveChangesAsync(); 
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }

    }
}