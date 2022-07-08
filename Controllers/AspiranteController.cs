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
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;

        public AspiranteController(KalumDBContext dbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Aspirante>> Post([FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando proceso para almacenar un registro de aspirante");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                Logger.LogInformation($"No existe la carrera técnica con el id {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if(jornada == null)
            {
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
                return BadRequest();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if(examenAdmision == null)
            {
                Logger.LogInformation($"No existe el examen de admisión con el id {value.ExamenId}");
                return BadRequest();
            }
            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha creado el aspirante con exito");
            return Ok(value);
        }

        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta aspirante");
            List<Aspirante> lista = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).ToListAsync();
            if(lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen registro en la base de datos");
                return new NoContentResult();
            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("La consulta se ejecuto con exito");
            return Ok(aspirantes);
        }

        [HttpGet("{noExpediente}", Name = "GetAspirante")]
        public async Task<ActionResult<AspiranteListDTO>> GetAspirante(string noExpediente)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el número de expediente " + noExpediente);
            var aspirante = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).FirstOrDefaultAsync(a => a.NoExpediente == noExpediente);
            if(aspirante == null)
            {
                Logger.LogWarning($"No existe el alumno con el carne {noExpediente}");
                return new NoContentResult();
            }
            AspiranteListDTO candidato = Mapper.Map<AspiranteListDTO>(aspirante);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(candidato);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion aspirante");
            var queryable = DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Aspirante>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0) 
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            List<AspiranteListDTO> aspirante = Mapper.Map<List<AspiranteListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginacion alumnos");
            return Ok(aspirante);            
        }

        [HttpDelete("{noExpediente}")]
        public async Task<ActionResult<Aspirante>> Delete(string noExpediente)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == noExpediente);
            ResultadoExamenAdmision resultadoExamen = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(re => re.NoExpediente == noExpediente);
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.NoExpediente == noExpediente);
            if(aspirante == null)
            {
                Logger.LogWarning($"No se encontro ningun aspirante con el número de expediente {noExpediente}");
                return NotFound();
            }
            else if(resultadoExamen != null)
            {
                Logger.LogWarning($"No se puede eliminar el aspirante con número de expediente {noExpediente}, cuenta con uno o más resultados de examen de admisión asignados");
                return BadRequest();
            }
            else if(inscripcionPago != null)
            {
                Logger.LogWarning($"No se puede eliminar el aspirante con número de expediente {noExpediente}, cuenta con uno o más pagos de inscripción asignados");
                return BadRequest();
            }
            else
            {
                DbContext.Aspirante.Remove(aspirante);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el aspirante con el número de expediente {noExpediente}");
                return aspirante;
            }
        }

        [HttpPut("{noExpediente}")]
        public async Task<ActionResult> Put(string noExpediente, [FromBody] Aspirante value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del aspirante con el número de expediente {noExpediente}");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == noExpediente);
            if(aspirante == null)
            {
                Logger.LogWarning($"No se encontro ningun aspirante con el número de expediente {noExpediente}");
                return NotFound();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                Logger.LogInformation($"No existe la carrera técnica con el id {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if(jornada == null)
            {
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
                return BadRequest();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if(examenAdmision == null)
            {
                Logger.LogInformation($"No existe el examen de admisión con el id {value.ExamenId}");
                return BadRequest();
            }
            aspirante.NoExpediente = value.NoExpediente;
            aspirante.Apellidos = value.Apellidos;
            aspirante.Nombres = value.Nombres;
            aspirante.Direccion = value.Direccion;
            aspirante.Telefono = value.Telefono;
            aspirante.Email = value.Email;
            aspirante.Estatus = value.Estatus;
            aspirante.CarreraId = value.CarreraId;
            aspirante.JornadaId = value.JornadaId;
            aspirante.ExamenId = value.ExamenId;
            DbContext.Entry(aspirante).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
            
        }


    }
}

    
    