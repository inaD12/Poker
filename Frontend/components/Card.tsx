import { CardRank, CardSuit } from "../table/types/table.types";

const rankCodes = [
  "2", "3", "4", "5", "6", "7", "8", "9", "10",
  "J", "Q", "K", "A"
];

const suitCodes = ["H", "D", "C", "S"];

type CardProps =
  | { back: true }
  | { back?: false; rank: CardRank; suit: CardSuit };

export default function Card(props: CardProps) {
  let svgPath: string;
  let alt: string;

  if ("back" in props && props.back) {
    svgPath = "/cards/2B.svg";
    alt = "Card back";
  } else {
    const { rank, suit } = props;
    svgPath = `/cards/${rankCodes[rank as unknown as number]}${suitCodes[suit as unknown as number]}.svg`;
    alt = `${CardRank[rank]} of ${CardSuit[suit]}`;
  }

  return <img src={svgPath} alt={alt} />;
}
