using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDBContext DbContext;

        public ExamenAdmisionController(KalumDBContext dbContext)
        {
            this.DbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<List<ExamenAdmision>> Get()
        {
            List<ExamenAdmision> examenes = null;
            examenes = DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToList();
            if(examenes == null || examenes.Count==0)
            {
                return new NoContentResult();
            }

            return Ok(examenes);
        }
    }
}


