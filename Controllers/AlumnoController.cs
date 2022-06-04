using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDBContext DbContext;

        public AlumnoController(KalumDBContext dbContext)
        {
            this.DbContext = dbContext;
        }
        
        public ActionResult<List<Alumno>> Get()
        {
            List<Alumno> alumnos = null;
            alumnos = DbContext.Alumno.Include(a => a.Inscripciones).ToList();
            if(alumnos == null || alumnos.Count==0)
            {
                return new NoContentResult();
            }
            return Ok(alumnos);
        }

    }
}