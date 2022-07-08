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
    public class ResultadoExamenAdmisionController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<ResultadoExamenAdmisionController> Logger;
        private readonly IMapper Mapper;
        public ResultadoExamenAdmisionController(KalumDBContext _DbContext, ILogger<ResultadoExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmisionListDTO>>> Get()
        {
            List<ResultadoExamenAdmision> resultados = null;
            Logger.LogDebug("Iniciando proceso de consulta de resultados de examen de admisión");
            resultados = await DbContext.ResultadoExamenAdmision.Include(re => re.Aspirante).ToListAsync();
            if(resultados == null)
            {
                Logger.LogWarning("No existen resultados de examen de admision");
                return new NoContentResult();
            }
            List<ResultadoExamenAdmisionListDTO> resultadosExamenes = Mapper.Map<List<ResultadoExamenAdmisionListDTO>>(resultados);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(resultadosExamenes);
        }

        [HttpGet("{noExpediente}/{anio}", Name ="GetResultadoExamenAdmision")]
        public async Task<ActionResult<ResultadoExamenAdmisionListDTO>> GetResultadoExamenAdmision(string noExpediente, string anio)
        {
            Logger.LogDebug($"Iniciando proceo de busqueda con el No. Expediente {noExpediente} y año {anio}");
            var examen = await DbContext.ResultadoExamenAdmision.Include(re => re.Aspirante).FirstOrDefaultAsync(re => re.NoExpediente == noExpediente && re.Anio == anio);
            if (examen == null)
            {
                Logger.LogWarning($"No existe resultado de examen de adimcion con el No. Expediente {noExpediente} en el año {anio}");
                return new NoContentResult();
            }
            ResultadoExamenAdmisionListDTO resultado = Mapper.Map<ResultadoExamenAdmisionListDTO>(examen);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(resultado);

        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmisionListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion resultados de exmanes de admisión");
            var queryable = DbContext.ResultadoExamenAdmision.Include(re => re.Aspirante).AsQueryable();
            var paginacion = new HttpResponsePaginacion<ResultadoExamenAdmision>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen regisros para paginar");
                return NoContent();
            }
            List<ResultadoExamenAdmisionListDTO> resultado = Mapper.Map<List<ResultadoExamenAdmisionListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginación de resultados de examen de admisión");
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<ResultadoExamenAdmisionListDTO>> Post([FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug("Iniciando proceso de agregar un resultado nuevo");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if(aspirante == null)
            {
                Logger.LogInformation($"No existe el aspirante con el No. Expediente {value.NoExpediente}");
                return BadRequest();
            }
            await DbContext.ResultadoExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar un resultado de examen de admisión");
            return new CreatedAtRouteResult("GetResultadoExamenAdmision", new {noExpediente = value.NoExpediente, anio = value.Anio}, value);
        }
        
        [HttpDelete("{noExpediente}/{anio}")]
        public async Task<ActionResult<ResultadoExamenAdmision>> Delete(string noExpediente, string anio)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            ResultadoExamenAdmision resultado = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(re => re.NoExpediente == noExpediente && re.Anio == anio);
            if(resultado == null)
            {
                Logger.LogWarning($"No se encontro ningun resultado de examen de admision con el No. Expediente {noExpediente} y año: {anio}");
                return NotFound();
            }
            else
            {
                DbContext.ResultadoExamenAdmision.Remove(resultado);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el resultado de examen de admision con el No Expediente {noExpediente} y año: {anio}");
                return resultado;
            }
        }

        [HttpPut("{noExpediente}/{anio}")]
        public async Task<ActionResult> Put(string noExpediente, string anio, [FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del resultado de examen de admision con el No Expediente {noExpediente} y año: {anio}");
            ResultadoExamenAdmision resultado = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(re => re.NoExpediente == noExpediente && re.Anio == anio);
            if(resultado == null)
            {
                Logger.LogWarning($"No se encontro ningun resultado de examen de admision con No Expediente {noExpediente} y año: {anio}");
                return NotFound();
            }
            else
            {
                resultado.Descripcion = value.Descripcion;
                resultado.Nota = value.Nota;
                DbContext.Entry(resultado).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }

    }
}