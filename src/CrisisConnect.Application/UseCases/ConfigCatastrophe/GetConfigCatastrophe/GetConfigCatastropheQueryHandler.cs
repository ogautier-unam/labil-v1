using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.GetConfigCatastrophe;

public class GetConfigCatastropheQueryHandler : IRequestHandler<GetConfigCatastropheQuery, ConfigCatastropheDto?>
{
    private readonly IConfigCatastropheRepository _repository;
    private readonly AppMapper _mapper;

    public GetConfigCatastropheQueryHandler(IConfigCatastropheRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<ConfigCatastropheDto?> Handle(GetConfigCatastropheQuery request, CancellationToken cancellationToken)
    {
        var config = await _repository.GetActiveAsync(cancellationToken);
        return config is null ? null : _mapper.ToDto(config);
    }
}
