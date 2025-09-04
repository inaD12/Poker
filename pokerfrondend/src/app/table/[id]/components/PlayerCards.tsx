import Card from "../../../../../components/Card";
import { CardDto } from "../../../../../table/types/table.types";

interface PlayerCardsProps {
  cards?: CardDto[];
  winnerShown: boolean;
}

export default function PlayerCards({ cards, winnerShown }: PlayerCardsProps) {
  return (
    <div className="flex gap-[2%] w-[55%]">
      {cards && winnerShown
        ? cards.map((card, i) => <div key={i}><Card rank={card.rank} suit={card.suit} /></div>)
        : [0, 1].map((_, i) => <div key={i}><Card back /></div>)}
    </div>
  );
}
