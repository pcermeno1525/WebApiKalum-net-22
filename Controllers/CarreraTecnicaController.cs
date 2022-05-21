using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;


namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDBContext DbContext;
        public CarreraTecnicaController(KalumDBContext _DbContext)
        {
            this.DbContext = _DbContext;            
        }

        [HttpGet]
        public ActionResult<List<CarreraTecnica>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            carrerasTecnicas = DbContext.CarreraTecnica.Include(c => c.Aspirantes).ToList();
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(carrerasTecnicas);
        }

    }
}