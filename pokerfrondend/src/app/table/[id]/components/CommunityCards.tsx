import Card from "../../../../../components/Card";
import { CardDto } from "../../../../../table/types/table.types";

interface CommunityCardsProps {
  cards: CardDto[];
}

export default function CommunityCards({ cards }: CommunityCardsProps) {
  return (
    <div className="absolute top-[38.9%] left-[50%] -translate-x-1/2 flex gap-[1.9%] w-[39%]">
      {cards.map((card, index) => (
        <div key={index} className="w-[18%]">
          <Card rank={card.rank} suit={card.suit} />
        </div>
      ))}
    </div>
  );
}
