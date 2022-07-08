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
    
    public class CargoController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        private readonly ILogger<CargoController> Logger;
        private readonly IMapper Mapper;

        public CargoController(KalumDBContext dbContext, ILogger<CargoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> Get()
        { 
            Logger.LogDebug("Iniciando el proceso de consulta de cargos");
            var cargos = await DbContext.Cargo.Include(c => c.CuentasxCobrar).ToListAsync();
            if(cargos == null)
            {
                Logger.LogWarning("No existen cargos");
                return new NoContentResult();
            }
            List<CargoListDTO> tipoCargos = Mapper.Map<List<CargoListDTO>>(cargos);
            Logger.LogInformation("Se ejecutio la petición de forma existosa");
            return Ok(tipoCargos);
        }

        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<CargoListDTO>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentasxCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning("Mo existe el cargo con el id " + id);
                return new NoContentResult();
            }
            CargoListDTO tipoCargo = Mapper.Map<CargoListDTO>(cargo);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(tipoCargo);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.Cargo.Include(c => c.CuentasxCobrar).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Cargo>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        } 

        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] CargoCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar cargo");
            Cargo nuevo = Mapper.Map<Cargo>(value);
            nuevo.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar un cargo");
            return new CreatedAtRouteResult("GetCargo", new {id = nuevo.CargoId}, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cargo>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            CuentaxCobrar cuentaxCobrar = await DbContext.CuentaxCobrar.FirstOrDefaultAsync(cxc => cxc.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"No se encontro el cargo con el id {id}");
                return NoContent();
            }
            else if(cuentaxCobrar != null)
            {
                Logger.LogWarning($"No se puede eliminar el cargo con el id {id}, está asociado a una o mas cuentas por cobrar");
                return BadRequest();
            }
            else
            {
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el cargo con el id {id}");
                return cargo;
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Cargo>> Put(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del cargo con el id {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"No se encontro el cargo con el id {id}");
                return BadRequest();
            }
            else
            {
                cargo.Descripcion = value.Descripcion;
                cargo.Prefijo = value.Prefijo;
                cargo.Monto = value.Monto; 
                cargo.GeneraMora = value.GeneraMora;
                cargo.PorcentajeMora = value.PorcentajeMora;
                DbContext.Entry(cargo).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }
    }
}