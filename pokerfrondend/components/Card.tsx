import { CardRank, CardSuit } from "../table/types/table.types";

const rankCodes = [
  "2", "3", "4", "5", "6", "7", "8", "9", "10",
  "J", "Q", "K", "A"
];

const suitCodes = ["H", "D", "C", "S"];

export default function Card({ rank, suit }: { rank: CardRank; suit: CardSuit }) {
   const svgPath = `/cards/${rankCodes[rank as unknown as number]}${suitCodes[suit as unknown as number]}.svg`;
  return <img src={svgPath} alt={`${CardRank[rank]} of ${CardSuit[suit]}`} />;
}
