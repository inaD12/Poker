'use client';

import { usePathname } from 'next/navigation';
import { useEffect, useRef } from 'react';
import { getLobbyClient } from '../../lobby/services/lobby.client';

export function RouteChangeListener() {
  const pathname = usePathname();
  const lastLobbyId = useRef<string | null>(null);

  const leaveLobby = async (lobbyId: string) => {
      const lobbyClient = await getLobbyClient();
      await lobbyClient.leaveLobby(lobbyId);
  };

  useEffect(() => {
    const match = pathname.match(/^\/lobby\/([^/]+)$/);
    const currentLobbyId = match ? match[1] : null;

    if (lastLobbyId.current && !currentLobbyId) {
      leaveLobby(lastLobbyId.current);
    }

    lastLobbyId.current = currentLobbyId;
  }, [pathname]);

  return null;
}
