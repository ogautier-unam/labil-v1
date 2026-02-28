using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.GetConfigCatastrophe;

public class GetConfigCatastropheQueryHandler : IRequestHandler<GetConfigCatastropheQuery, ConfigCatastropheDto?>
{
    private readonly IConfigCatastropheRepository _repository;
    private readonly IMapper _mapper;

    public GetConfigCatastropheQueryHandler(IConfigCatastropheRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ConfigCatastropheDto?> Handle(GetConfigCatastropheQuery request, CancellationToken cancellationToken)
    {
        var config = await _repository.GetActiveAsync(cancellationToken);
        return config is null ? null : _mapper.Map<ConfigCatastropheDto>(config);
    }
}
