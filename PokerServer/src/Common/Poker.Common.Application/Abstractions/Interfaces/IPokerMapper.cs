namespace Poker.Common.Application.Abstractions.Interfaces;

public interface IPokerMapper
{
	void Map(object source, object destination);

	T Map<T>(object source);
}
