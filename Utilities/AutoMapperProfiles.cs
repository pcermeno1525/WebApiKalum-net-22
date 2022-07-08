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
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<Cargo, CargoListDTO>();
            CreateMap<Cargo, CargoCreateDTO>();
            CreateMap<CargoCreateDTO, Cargo>();
            CreateMap<CuentaxCobrar, CuentaxCobrarCreateDTO>();
            CreateMap<CuentaxCobrarCreateDTO, CuentaxCobrar>();
            CreateMap<CarreraTecnica, InversionCarreraTecnicaCarreraTecnicaListDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraTecnicaListDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraTecnicaCreateDTO>();
            CreateMap<InversionCarreraTecnicaCreateDTO, InversionCarreraTecnica>();
            CreateMap<ResultadoExamenAdmision, ResultadoExamenAdmisionListDTO>();
            CreateMap<InscripcionPago, InscripcionPagoListDTO>();
        }
    }
}