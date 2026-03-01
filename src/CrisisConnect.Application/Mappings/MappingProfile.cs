using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Proposition, PropositionDto>();
        CreateMap<Offre, OffreDto>();
        CreateMap<Demande, DemandeDto>()
            .ForMember(dest => dest.Urgence, opt => opt.MapFrom(src => src.Urgence));
        CreateMap<Transaction, TransactionDto>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<ConfigCatastrophe, ConfigCatastropheDto>();
        CreateMap<Panier, PanierDto>()
            .ForMember(dest => dest.Offres, opt => opt.MapFrom(src => src.Offres));
        CreateMap<EntreeJournal, EntreeJournalDto>();
        CreateMap<SuggestionAppariement, SuggestionAppariementDto>();
        CreateMap<Message, MessageDto>();
        CreateMap<AttributionRole, AttributionRoleDto>()
            .ForMember(dest => dest.TypeRole, opt => opt.MapFrom(src => src.TypeRole.ToString()))
            .ForMember(dest => dest.Statut,   opt => opt.MapFrom(src => src.Statut.ToString()));
        CreateMap<Mandat, MandatDto>()
            .ForMember(dest => dest.Portee,   opt => opt.MapFrom(src => src.Portee.ToString()))
            .ForMember(dest => dest.EstActif, opt => opt.MapFrom(src => src.EstActif));
    }
}
