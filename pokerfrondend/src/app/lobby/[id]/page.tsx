'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getLobbyClient } from '../../../../lobby/services/lobby.client';
import { LobbyQueryResponse } from '../../../../lobby/types/lobby.types';

export default function Lobby() {
  const router = useRouter();
  const params = useParams<{ id: string }>();
  const lobbyId = params.id;

  const [lobby, setLobby] = useState<LobbyQueryResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [hasJoined, setHasJoined] = useState(false);

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

  const handleClick= async () => {
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
    <div>
      <h1>{lobby.name}</h1>
      <p>Creator: {lobby.creator}</p>
      <p>{lobby.players.length} players</p>
      <button className='bg-amber-500' onClick={handleClick}>Leave</button>
    </div>
  );
}
