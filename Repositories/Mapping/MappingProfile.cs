using AutoMapper;
using Core.Models;
using Repositories.Entities;

namespace Repositories.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentEntity, Student>(MemberList.Destination).ReverseMap();
        CreateMap<GroupEntity, Group>(MemberList.Destination).ReverseMap();
        CreateMap<ProfessorEntity, Professor>(MemberList.Destination).ReverseMap();
        CreateMap<UserEntity, User>(MemberList.Destination).ReverseMap();
        CreateMap<PointEntity, Points>(MemberList.Destination).ReverseMap();
        CreateMap<StudentEntity_PointEntity, Student_Point>(MemberList.Destination).ReverseMap();
    }
    
}