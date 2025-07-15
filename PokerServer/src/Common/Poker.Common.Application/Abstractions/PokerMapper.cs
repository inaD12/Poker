using AutoMapper;

namespace Poker.Common.Application.Abstractions;

public class PokerMapper: IPokerMapper
{
    private readonly IMapper _mapper;

    public PokerMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public T Map<T>(object source)
    {
        return _mapper.Map<T>(source);
    }

    public void Map(object source, object destination)
    {
        _mapper.Map(source, destination);
    }
}