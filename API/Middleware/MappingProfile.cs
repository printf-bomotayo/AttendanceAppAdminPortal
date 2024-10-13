using API.DTOs.AttendanceDTOs;
using API.DTOs.CandidateDTOs;
using API.DTOs.CohortDTOs;
using API.DTOs.TrainingProgramDTOs;
using API.Entities;
using AutoMapper;

namespace API.Middleware
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Entity to DTO
            CreateMap<Candidate, CandidateDto>();

            CreateMap<CandidateDto, Candidate>();



            CreateMap<Cohort, CohortDto>();
            CreateMap<CohortCreateDto, Cohort>();
            CreateMap<AttendanceRecord, AttendanceRecordDto>();
            CreateMap<TrainingProgram, TrainingProgramDto>();
            CreateMap<TrainingProgramDto, TrainingProgram>()
               .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())   // These properties will not be mapped from DTO
               .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
               .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
               .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());


            CreateMap<TrainingProgramUpdateDto, TrainingProgram>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())   // These properties will not be mapped from DTO
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            CreateMap<CohortCreateDto, Cohort>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            CreateMap<CohortUpdateDto, Cohort>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        }
    }
}
