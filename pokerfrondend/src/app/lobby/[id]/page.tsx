'use client';

import { useEffect, useState, useCallback } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getLobbyClient } from '../../../../lobby/services/lobby.client';
import { LobbyQueryResponse, PlayerInfoDto } from '../../../../lobby/types/lobby.types';
import LobbyControls from './components/LobbyControls';
import PlayerList from './components/PlayerList';

export default function Lobby() {
  const router = useRouter();
  const { id: lobbyId } = useParams<{ id: string }>();

  const [lobby, setLobby] = useState<LobbyQueryResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [hasJoined, setHasJoined] = useState(false);
  const [isCreator, setIsCreator] = useState(false);
  const [funds, setFunds] = useState<number>(0);
  const [isAddingFunds, setIsAddingFunds] = useState(false);

  const handleClickLeave = useCallback(async () => {
    const lobbyClient = await getLobbyClient();
    const result = await lobbyClient.leaveLobby(lobbyId);
    if (result.isFailure) return alert(result.response.message.message);
    router.push(`/lobbies`);
  }, [lobbyId, router]);

  const handleClickStartGame = useCallback(async () => {
    const lobbyClient = await getLobbyClient();
    const result = await lobbyClient.startGame(lobbyId);
    if (result.isFailure) alert(result.response.message.message);
  }, [lobbyId]);

  const handleClickAddFunds = useCallback(async () => {
    if (!funds || funds <= 0) return alert('Please enter a valid amount.');
    setIsAddingFunds(true);

    try {
      const lobbyClient = await getLobbyClient();
      const result = await lobbyClient.addFunds(lobbyId, funds);
      if (result.isFailure) return alert(result.response.message.message);

      setLobby(prev => {
        if (!prev) return prev;
        const updatedPlayers = prev.players.map(p =>
          p.isSelf ? { ...p, balance: p.balance + funds } : p
        );
        return { ...prev, players: updatedPlayers };
      });
      setFunds(0);
    } finally {
      setIsAddingFunds(false);
    }
  }, [funds, lobbyId]);

  useEffect(() => {
    if (!lobby) return;
    const selfPlayer = lobby.players.find(p => p.isSelf);
    setIsCreator(selfPlayer?.username === lobby.creator);
  }, [lobby]);

  const attachLobbyListeners = useCallback(async () => {
    const lobbyClient = await getLobbyClient();

    lobbyClient.onPlayerJoined(player => {
      setLobby(prev => {
        if (!prev || prev.players.some(p => p.id === player.id)) return prev;
        return { ...prev, players: [...prev.players, player] };
      });
    });

    lobbyClient.onPlayerLeft(playerId => {
      setLobby(prev => {
        if (!prev) return prev;
        return { ...prev, players: prev.players.filter(p => p.id !== playerId) };
      });
    });

    lobbyClient.onGameStarted(gameId => {
      lobbyClient.disconnect();
      router.push(`/table/${gameId}`);
    });
  }, [router]);

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

  const slots: (PlayerInfoDto | null)[] = [...lobby.players];
  while (slots.length < 6) slots.push(null);

  return (
    <div className="h-screen w-full flex flex-col items-center justify-start py-3">
      <div className="flex items-center justify-center md:py-4 flex-grow w-full max-w-5xl">
        <div className="relative w-full md:max-w-350 h-auto md:h-182 border-4 border-white bg-[#437057]">

          <LobbyControls
            lobby={lobby}
            isCreator={isCreator}
            funds={funds}
            setFunds={setFunds}
            isAddingFunds={isAddingFunds}
            onLeave={handleClickLeave}
            onStartGame={handleClickStartGame}
            onAddFunds={handleClickAddFunds}
          />

          <PlayerList players={slots} />

        </div>
      </div>
    </div>
  );
}
