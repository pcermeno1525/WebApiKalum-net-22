using Microsoft.AspNetCore.Mvc;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    
    public class CargoController : ControllerBase
    {
        private readonly KalumDBContext DbContext;

        public CargoController(KalumDBContext dbContext)
        {
            this.DbContext = dbContext;
        }

        // public ActionResult<List<Cargo>> Get()
        // { 
        //     List<Cargo> cargos = null;
        //     cargos = DbContext.Cargo.Include()
        // }
    }
}