using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionDTO>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing(e => new AspiranteListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Aspirante, CarreraTecnicaAspiranteListDTO>().ConstructUsing(e => new CarreraTecnicaAspiranteListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Inscripcion, InscripcionListDTO>();            
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<Inscripcion, AlumnoInscripcionDTO>();
            CreateMap<CuentaxCobrar, CuentaxCobrarListDTO>();
            CreateMap<Alumno, AlumnoListDTO>().ConstructUsing(e => new AlumnoListDTO {NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Alumno, AlumnoCreateDTO>();
            CreateMap<AlumnoCreateDTO, Alumno>();
            
        }
    }
}