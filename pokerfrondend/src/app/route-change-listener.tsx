"use client";

import { usePathname } from "next/navigation";
import { useEffect, useRef } from "react";
import { getLobbyClient } from "../../lobby/services/lobby.client";
import { getTableClient } from "../../table/services/table.client";

export function RouteChangeListener() {
  const pathname = usePathname();
  const lastLobbyId = useRef<string | null>(null);
  const lastGameId = useRef<string | null>(null);

  const leaveLobby = async (lobbyId: string) => {
    try {
      const lobbyClient = await getLobbyClient();
      await lobbyClient.leaveLobby(lobbyId);
    } catch (err) {
      console.error("Failed to leave lobby:", err);
    }
  };

  const leaveGame = async (gameId: string) => {
    try {
      const tableClient = await getTableClient(gameId);
      await tableClient.disconnectGame(gameId);
    } catch (err) {
      console.error("Failed to leave game:", err);
    }
  };

  useEffect(() => {
    const lobbyMatch = pathname.match(/^\/lobby\/([^/]+)$/);
    const gameMatch = pathname.match(/^\/table\/([^/]+)$/);

    const currentLobbyId = lobbyMatch ? lobbyMatch[1] : null;
    const currentGameId = gameMatch ? gameMatch[1] : null;

    if (lastLobbyId.current && !currentLobbyId) {
      leaveLobby(lastLobbyId.current);
    }

    if (lastGameId.current && !currentGameId) {
      leaveGame(lastGameId.current);
    }

    lastLobbyId.current = currentLobbyId;
    lastGameId.current = currentGameId;
  }, [pathname]);

  return null;
}
