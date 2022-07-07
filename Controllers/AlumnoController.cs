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
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        private readonly IMapper Mapper;

        public AlumnoController(KalumDBContext dbContext, ILogger<AlumnoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso de consulta de alumnos");
            alumnos = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentasxCobrar).ToListAsync();
            if(alumnos == null || alumnos.Count==0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            List<AlumnoListDTO> estudiantes = Mapper.Map<List<AlumnoListDTO>>(alumnos);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(estudiantes);
        }

        [HttpGet("{carne}", Name = "GetAlumno")]
        public async Task<ActionResult<AlumnoListDTO>> GetAlumno(string carne)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + carne);
            var alumno = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentasxCobrar).FirstOrDefaultAsync(a => a.Carne == carne);
            if(alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el carne " + carne);
                return new NoContentResult();
            }
            AlumnoListDTO estudiante = Mapper.Map<AlumnoListDTO>(alumno);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(estudiante);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion alumno");
            var queryable = DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentasxCobrar).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Alumno>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0) 
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            List<AlumnoListDTO> alumno = Mapper.Map<List<AlumnoListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginacion alumnos");
            return Ok(alumno);            
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody] AlumnoCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar un alumno nuevo");
            var carne = await DbContext.Alumno.MaxAsync(a => a.Carne);
            carne = (Convert.ToInt32(carne) + 1).ToString();
            Alumno nuevo = Mapper.Map<Alumno>(value);
            nuevo.Carne = carne.ToString();
            await DbContext.Alumno.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar un alumno");
            return new CreatedAtRouteResult("GetAlumno", new {carne = nuevo.Carne}, value);
        }
        
        [HttpDelete("{carne}")]
        public async Task<ActionResult<Alumno>> Delete(string carne)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);
            CuentaxCobrar cuentaxCobrar = await DbContext.CuentaxCobrar.FirstOrDefaultAsync(cxc => cxc.Carne == carne);
            if(alumno == null)
            {
                Logger.LogWarning($"No se encontro ningun alumno con el carne {carne}");
                return NotFound();
            }
            else if(cuentaxCobrar != null)
            {
                Logger.LogWarning($"No se puede eliminar el alumno con carne {carne}, posee cuenta por cobrar");
                return BadRequest();
            }
            else
            {
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el alumno con el carne {carne}");
                return alumno;
            }
        }

        [HttpPut("{carne}")]
        public async Task<ActionResult> Put(string carne, [FromBody] Alumno value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del alumno con carne {carne}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);
            if(alumno == null)
            {
                Logger.LogWarning($"No se encontro ningun alumno con carne {carne}");
                return NotFound();
            }
            else
            {
                alumno.Apellidos = value.Apellidos;
                alumno.Nombres = value.Nombres;
                alumno.Direccion = value.Direccion;
                alumno.Telefono = value.Telefono;
                alumno.Email = value.Email;
                DbContext.Entry(alumno).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }

    }
}