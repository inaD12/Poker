'use client';

import { PlayerInfoDto } from '../../../../../lobby/types/lobby.types';
import PlayerBox from './PlayerBox';

interface PlayerListProps {
  players: (PlayerInfoDto | null)[];
}

export default function PlayerList({ players }: PlayerListProps) {
  return (
    <div className="relative md:absolute md:top-8 md:right-10 flex flex-col w-full md:w-[55%] max-w-full md:max-w-250 h-auto md:h-150 bg-[#2f5040] items-start max-h-full overflow-auto p-3 rounded-xl border-1 border-white mt-4 md:mt-0">
      {players.map((player, index) =>
        player ? (
          <PlayerBox
            key={player.id || index}
            username={player.username}
            handsPlayed={player.handsPlayed}
            handsWon={player.handsWon}
            totalEarnings={player.totalEarnings}
            balance={player.balance}
          />
        ) : (
          <div
            key={`empty-${index}`}
            className="w-full h-20 mb-3 bg-[#465a49] rounded-md border border-dashed border-white flex items-center justify-center text-white font-semibold opacity-50"
          >
            Empty slot #{index + 1}
          </div>
        )
      )}
    </div>
  );
}
