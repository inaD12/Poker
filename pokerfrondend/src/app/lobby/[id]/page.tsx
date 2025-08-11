'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getLobbyClient } from '../../../../lobby/services/lobby.client';
import { LobbyQueryResponse, PlayerInfoDto } from '../../../../lobby/types/lobby.types';
import PlayerBox from './components/PlayerBox';

export default function Lobby() {
  const router = useRouter();
  const params = useParams<{ id: string }>();
  const lobbyId = params.id;

  const [isCreator, setIsCreator] = useState<boolean>(false);
  const [lobby, setLobby] = useState<LobbyQueryResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [hasJoined, setHasJoined] = useState<boolean>(false);

  useEffect(() => {
    if(!lobby) return;
    const selfPlayer = lobby.players.find(p => p.isSelf === true);
    setIsCreator(selfPlayer?.username === lobby.creator);
  }, [lobby]);


  useEffect(() => {
    if (!lobbyId || hasJoined) return;

    async function joinLobby() {
      setLoading(true);
      try {
        const lobbyClient = await getLobbyClient();
        const result = await lobbyClient.joinLobby(lobbyId);
        if(result.isFailure){
            alert(result.response.message.message)
            router.push(`/lobbies`);
        }
        setLobby(result.value);
        setHasJoined(true);

      } catch (err) {
        console.error('Failed to join lobby:', err);
      } finally {
        setLoading(false);
      }
    }

    joinLobby();
  }, [lobbyId, hasJoined]);

   const players = lobby?.players || [];

    const slots: (PlayerInfoDto | null)[] = [...players];
    while (slots.length < 6) {
      slots.push(null);
    }


  const handleClickLeave= async () => {
      const lobbyClient = await getLobbyClient();
      const result = await lobbyClient.leaveLobby(lobbyId);
      if(result.isFailure){
            alert(result.response.message.message)
      }
      else{
        router.push(`/lobbies`);
      }
  };

  if (loading) return <div>Joining lobby...</div>;
  if (!lobby) return <div>Failed to load lobby.</div>;

return (
  <div className="h-screen w-full flex flex-col items-center justify-start py-3">
    <div className="flex items-center justify-center md:py-4 flex-grow w-full max-w-5xl">
      <div className="relative w-full md:max-w-350 h-auto md:h-182 border-4 border-white bg-[#437057]">
        
        <div className="
          relative md:absolute md:top-10 md:left-30
          flex flex-col items-center gap-3 
          w-full md:w-[45%] max-w-full md:max-w-50
          h-auto md:h-150
          p-3 rounded bg-transparent"
          >
          <p className="text-2xl md:text-3xl font-bold">Game name</p>
          <p className="text-base md:text-l font-semibold">Creator: inaD</p>
          <button
            className="bg-red-700 hover:bg-red-800 rounded-md h-[50px] w-[150px]"
            onClick={handleClickLeave}
          >
            Leave Game
          </button>
          {isCreator && (
          <button className="bg-green-700 hover:bg-green-800 rounded-md h-[50px] w-[150px]">
            Start Game
          </button>    
          )}
        </div>

        <div className="
          relative md:absolute md:top-8 md:right-10
          flex flex-col 
          w-full md:w-[55%] max-w-full md:max-w-250
          h-auto md:h-150 
          bg-[#2f5040] items-start max-h-full overflow-auto p-3 rounded-xl border-1 border-white mt-4 md:mt-0"
          >
          {slots.map((player, index) =>
            player ? (
              <PlayerBox
                key={index}
                username={player.username}
                gamesPlayed={player.gamesPlayed}
                gamesWon={player.gamesWon}
                totalEarnings={player.totalEarnings}
              />
            ) : (
              <div
                key={index}
                className="w-full h-20 mb-3 bg-[#465a49] rounded-md border border-dashed border-white flex items-center justify-center text-white font-semibold opacity-50"
              >
                Empty slot #{index + 1}
              </div>
            )
          )}
        </div>

      </div>
    </div>
  </div>
);


}
