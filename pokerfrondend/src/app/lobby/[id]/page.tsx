'use client';

import { useEffect, useState, useCallback } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getLobbyClient } from '../../../../lobby/services/lobby.client';
import { LobbyQueryResponse, PlayerInfoDto } from '../../../../lobby/types/lobby.types';
import PlayerBox from './components/PlayerBox';

export default function Lobby() {
  const router = useRouter();
  const { id: lobbyId } = useParams<{ id: string }>();

  const [isCreator, setIsCreator] = useState(false);
  const [lobby, setLobby] = useState<LobbyQueryResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [hasJoined, setHasJoined] = useState(false);

  const handleClickLeave = useCallback(async () => {
    const lobbyClient = await getLobbyClient();
    const result = await lobbyClient.leaveLobby(lobbyId);
    if (result.isFailure) {
      alert(result.response.message.message);
    } else {
      router.push(`/lobbies`);
    }
  }, [lobbyId, router]);

  const handleClickStartGame = useCallback(async () => {
    const lobbyClient = await getLobbyClient();
    const result = await lobbyClient.startGame(lobbyId);
    if (result.isFailure) {
      alert(result.response.message.message);
    }
  }, [lobbyId, router]);

  useEffect(() => {
    if (!lobby) return;
    const selfPlayer = lobby.players.find(p => p.isSelf);
    setIsCreator(selfPlayer?.username === lobby.creator);
  }, [lobby]);

  const attachLobbyListeners = useCallback(async () => {
    const lobbyClient = await getLobbyClient();

    lobbyClient.onPlayerJoined((player: PlayerInfoDto) => {
      setLobby(prev => {
        if (!prev || prev.players.some(p => p.id === player.id)) return prev;
        return { ...prev, players: [...prev.players, player] };
      });
    });

    lobbyClient.onPlayerLeft((playerId: string) => {
      setLobby(prev => {
        if (!prev) return prev;
        return { ...prev, players: prev.players.filter(p => p.id !== playerId) };
      });
    });

     lobbyClient.onGameStarted((gameId: string) => {
      lobbyClient.disconnect();
      router.push(`/table/${gameId}`);
    });
  }, []);

  useEffect(() => {
    if (!lobbyId || hasJoined) return;

    const joinLobby = async () => {
      setLoading(true);
      try {
        const lobbyClient = await getLobbyClient();
        const result = await lobbyClient.joinLobby(lobbyId);

        if (result.isFailure) {
          alert(result.response.message.message);
          router.push(`/lobbies`);
          return;
        }

        setLobby(result.value);
        setHasJoined(true);

        await attachLobbyListeners();
      } catch (err) {
        console.error('Failed to join lobby:', err);
      } finally {
        setLoading(false);
      }
    };

    joinLobby();
  }, [lobbyId, hasJoined, router, attachLobbyListeners]);

  if (loading) return <div>Joining lobby...</div>;
  if (!lobby) return <div>Failed to load lobby.</div>;

  const slots: (PlayerInfoDto | null)[] = [...(lobby?.players || [])];
  while (slots.length < 6) slots.push(null);

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
            <p className="text-2xl md:text-3xl font-bold">{lobby.name}</p>
            <p className="text-base md:text-l font-semibold">Creator: {lobby.creator}</p>
            <button
              className="bg-red-700 hover:bg-red-800 rounded-md h-[50px] w-[150px]"
              onClick={handleClickLeave}
            >
              Leave Game
            </button>
            {isCreator && (
              <button 
                className="bg-green-700 hover:bg-green-800 rounded-md h-[50px] w-[150px]"
                onClick={handleClickStartGame}
              >
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
                  key={player.id || index}
                  username={player.username}
                  handsPlayed={player.handsPlayed}
                  handsWon={player.handsWon}
                  totalEarnings={player.totalEarnings}
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

        </div>
      </div>
    </div>
  );
}
