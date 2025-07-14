using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.Services;

public static class HandEvaluator
{
	public static int EvaluateHand(List<Card> sCards)
	{

		if (sCards.Count < 5) return 0;
		sCards.Sort((x, y) => y.Rank.CompareTo(x.Rank));
		return GetHandValueList(sCards);

	}
	private static int GetHandValueList(List<Card> sCards)
	{

		int count = sCards.Count;
		if (count == 5) return GetHandValue(sCards);

		int highestValue = 0;
		Card missingOne;
		int tempValue;

		for (int i = 0; i < count - 1; i++)
		{

			missingOne = sCards[i];
			sCards.RemoveAt(i);

			tempValue = GetHandValueList(sCards);
			if (tempValue > highestValue) highestValue = tempValue;

			sCards.Insert(i, missingOne);

		}

		missingOne = sCards[count - 1];
		sCards.RemoveAt(count - 1);

		tempValue = GetHandValueList(sCards);
		if (tempValue > highestValue) highestValue = tempValue;

		sCards.Add(missingOne);
		return highestValue;

	}
	private static int GetHandValue(List<Card> sortedCards)
	{
		// --- Check for Straight or Straight Flush ---
		if ((sortedCards[0].Rank - 1 == sortedCards[1].Rank) &&
			(sortedCards[1].Rank - 1 == sortedCards[2].Rank) &&
			(sortedCards[2].Rank - 1 == sortedCards[3].Rank) &&
			(sortedCards[3].Rank - 1 == sortedCards[4].Rank))
		{
			// Straight Flush
			if (sortedCards.All(c => c.Suit == sortedCards[0].Suit))
				return (8 << 20) + (byte)sortedCards[0].Rank;

			// Straight
			return (4 << 20) + (byte)sortedCards[0].Rank;
		}

		// --- Special case: Wheel (A-2-3-4-5) ---
		if (sortedCards[4].Rank == CardRank.Two &&
			sortedCards[3].Rank == CardRank.Three &&
			sortedCards[2].Rank == CardRank.Four &&
			sortedCards[1].Rank == CardRank.Five &&
			sortedCards[0].Rank == CardRank.Ace)
		{
			// Wheel Flush
			if (sortedCards.All(c => c.Suit == sortedCards[0].Suit))
				return (8 << 20) + (byte)sortedCards[1].Rank;

			// Wheel Straight
			return (4 << 20) + (byte)sortedCards[1].Rank;
		}

		// --- Check for Flush ---
		if (sortedCards.All(c => c.Suit == sortedCards[0].Suit))
			return (5 << 20) +
				   ((byte)sortedCards[0].Rank << 16) +
				   ((byte)sortedCards[1].Rank << 12) +
				   ((byte)sortedCards[2].Rank << 8) +
				   ((byte)sortedCards[3].Rank << 4) +
				   (byte)sortedCards[4].Rank;

		// --- Check for Four of a Kind ---
		if ((sortedCards[0].Rank == sortedCards[1].Rank &&
			 sortedCards[1].Rank == sortedCards[2].Rank &&
			 sortedCards[2].Rank == sortedCards[3].Rank) ||
			(sortedCards[1].Rank == sortedCards[2].Rank &&
			 sortedCards[2].Rank == sortedCards[3].Rank &&
			 sortedCards[3].Rank == sortedCards[4].Rank))
		{
			return (7 << 20) + (byte)sortedCards[1].Rank;
		}

		// --- Check for Full House or Three of a Kind ---
		// Three of a Kind in the front
		if (sortedCards[0].Rank == sortedCards[1].Rank &&
			sortedCards[1].Rank == sortedCards[2].Rank)
		{
			if (sortedCards[3].Rank == sortedCards[4].Rank)
				return (6 << 20) + ((byte)sortedCards[0].Rank << 4) + (byte)sortedCards[3].Rank;

			return (3 << 20) +
				   ((byte)sortedCards[0].Rank << 8) +
				   ((byte)sortedCards[3].Rank << 4) +
				   (byte)sortedCards[4].Rank;
		}

		// Three of a Kind at the end
		if (sortedCards[2].Rank == sortedCards[3].Rank &&
			sortedCards[3].Rank == sortedCards[4].Rank)
		{
			if (sortedCards[0].Rank == sortedCards[1].Rank)
				return (6 << 20) + ((byte)sortedCards[2].Rank << 4) + (byte)sortedCards[0].Rank;

			return (3 << 20) +
				   ((byte)sortedCards[2].Rank << 8) +
				   ((byte)sortedCards[0].Rank << 4) +
				   (byte)sortedCards[1].Rank;
		}

		// Three of a Kind in the middle
		if (sortedCards[1].Rank == sortedCards[2].Rank &&
			sortedCards[2].Rank == sortedCards[3].Rank)
		{
			return (3 << 20) +
				   ((byte)sortedCards[1].Rank << 8) +
				   ((byte)sortedCards[0].Rank << 4) +
				   (byte)sortedCards[4].Rank;
		}

		// --- Check for Two Pair ---
		if (sortedCards[0].Rank == sortedCards[1].Rank &&
			sortedCards[2].Rank == sortedCards[3].Rank)
			return (2 << 20) +
				   ((byte)sortedCards[0].Rank << 8) +
				   ((byte)sortedCards[2].Rank << 4) +
				   (byte)sortedCards[4].Rank;

		if (sortedCards[0].Rank == sortedCards[1].Rank &&
			sortedCards[3].Rank == sortedCards[4].Rank)
			return (2 << 20) +
				   ((byte)sortedCards[0].Rank << 8) +
				   ((byte)sortedCards[3].Rank << 4) +
				   (byte)sortedCards[2].Rank;

		if (sortedCards[1].Rank == sortedCards[2].Rank &&
			sortedCards[3].Rank == sortedCards[4].Rank)
			return (2 << 20) +
				   ((byte)sortedCards[1].Rank << 8) +
				   ((byte)sortedCards[3].Rank << 4) +
				   (byte)sortedCards[0].Rank;

		// --- Check for One Pair ---
		if (sortedCards[0].Rank == sortedCards[1].Rank)
			return (1 << 20) +
				   ((byte)sortedCards[0].Rank << 12) +
				   ((byte)sortedCards[2].Rank << 8) +
				   ((byte)sortedCards[3].Rank << 4) +
				   (byte)sortedCards[4].Rank;

		if (sortedCards[1].Rank == sortedCards[2].Rank)
			return (1 << 20) +
				   ((byte)sortedCards[1].Rank << 12) +
				   ((byte)sortedCards[0].Rank << 8) +
				   ((byte)sortedCards[3].Rank << 4) +
				   (byte)sortedCards[4].Rank;

		if (sortedCards[2].Rank == sortedCards[3].Rank)
			return (1 << 20) +
				   ((byte)sortedCards[2].Rank << 12) +
				   ((byte)sortedCards[0].Rank << 8) +
				   ((byte)sortedCards[1].Rank << 4) +
				   (byte)sortedCards[4].Rank;

		if (sortedCards[3].Rank == sortedCards[4].Rank)
			return (1 << 20) +
				   ((byte)sortedCards[3].Rank << 12) +
				   ((byte)sortedCards[0].Rank << 8) +
				   ((byte)sortedCards[1].Rank << 4) +
				   (byte)sortedCards[2].Rank;

		// --- High Card ---
		return ((byte)sortedCards[0].Rank << 16) +
			   ((byte)sortedCards[1].Rank << 12) +
			   ((byte)sortedCards[2].Rank << 8) +
			   ((byte)sortedCards[3].Rank << 4) +
			   (byte)sortedCards[4].Rank;
	}

}
