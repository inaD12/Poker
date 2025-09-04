import { PlayerActionNotification, PlayerStateDto } from "../../../../../table/types/table.types";
import { formatAction } from "../../../../../utilities/table";
import PlayerCards from "./PlayerCards";

interface PlayerProps {
  player: PlayerStateDto;
  index: number;
  seatPosition: string;
  playerActions: Record<string, PlayerActionNotification | null>;
  currentTurn: string | null;
  winnerNames: string[] | null;
}

export default function Player({ player, index, seatPosition, playerActions, currentTurn, winnerNames }: PlayerProps) {
  const showWinnerCards = !!winnerNames;

  return (
    <div className={`${seatPosition} -translate-x-1/2 flex flex-col gap-[4%] w-[25%] items-center`}>

      {player.isFolded && <div className="absolute -top-8 left-1/2 -translate-x-1/2 bg-red-600 text-white px-3 py-1 rounded-full text-sm shadow-md">Folded</div>}
      {player.isDisconnected && <div className="absolute -top-8 left-1/2 -translate-x-1/2 bg-gray-500 text-white px-3 py-1 rounded-full text-sm shadow-md">Disconnected</div>}
      {player.isAllIn && <div className="absolute -top-8 left-1/2 -translate-x-1/2 bg-purple-600 text-white px-3 py-1 rounded-full text-sm shadow-md">All-In!</div>}
      {playerActions[player.id] && <div className="absolute -top-16 left-1/2 -translate-x-1/2 bg-black/70 text-white px-3 py-1 rounded-full text-sm shadow-md">{formatAction(playerActions[player.id]!)}</div>}
      {currentTurn === player.id && !player.isDisconnected && !player.isFolded && !player.isAllIn && <div className="absolute -top-8 left-1/2 -translate-x-1/2 bg-yellow-600 text-white px-3 py-1 rounded-full text-sm shadow-md animate-pulse">Thinking...</div>}

      <PlayerCards cards={player.cards} winnerShown={showWinnerCards} />

      <div className="bg-black/50 text-white px-2 mt-2 rounded text-center">{player.username}</div>
    </div>
  );
}
