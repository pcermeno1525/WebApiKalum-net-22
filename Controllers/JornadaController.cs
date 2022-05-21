using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDBContext DbContext;

        public JornadaController(KalumDBContext dbContext)
        {
            this.DbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<List<Jornada>> Get()
        {
            List<Jornada> jornadas = null;
            jornadas = DbContext.Jornada.Include(j => j.Aspirantes).ToList();
            
            if(jornadas == null || jornadas.Count==0)
            {
                return new NoContentResult();
            }
            return Ok(jornadas);

        }
        
    }
}