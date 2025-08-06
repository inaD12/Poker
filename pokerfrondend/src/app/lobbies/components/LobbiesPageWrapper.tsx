"use client";

import { useEffect } from "react";
import { LobbyQueryResponse } from "../../../../lobby/types/lobby.types";
import LobbyBox from "./LobbyBox";
import { getLobbyClient } from "../../../../lobby/services/lobby.client";


const LobbiesPageWrapper = ({ lobbies }: { lobbies: LobbyQueryResponse[]}) => {
  useEffect(() => {
    const lobbyClient = getLobbyClient();
  }, []);
    
  return (
    <>
    {lobbies.length === 0 ? (
          <div className="flex flex-grow items-center justify-center w-full p-4">
            <p className="text-white italic text-center">No lobbies available.</p>
          </div>
        ) : (
          lobbies.map((lobby) => (
            <LobbyBox
              key={lobby.id}
              gameName={lobby.name}
              creator={lobby.creator}
              createdAt={lobby.createdAt}
              players={`${lobby.players.length}/6`}
            />
          ))
        )}
    </>
  );
};

export default LobbiesPageWrapper;