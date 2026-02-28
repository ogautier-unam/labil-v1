using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Proposition, PropositionDto>();
        CreateMap<Mission, MissionDto>();
        CreateMap<Matching, MatchingDto>();
        CreateMap<Notification, NotificationDto>();
    }
}
