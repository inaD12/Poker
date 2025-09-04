'use client'

import Image from "next/image";
import { useParams, useRouter } from "next/navigation";
import { useCallback, useEffect, useRef, useState } from "react";
import tableClient, { getTableClient } from "../../../../table/services/table.client";
import { CardDto, GamePhase, GameStateDto, PlayerActionNotification, PlayerStateDto } from "../../../../table/types/table.types";
import { getSeatPositions } from "../../../../utilities/table";
import CommunityCards from "./components/CommunityCards";
import Player from "./components/Player";
import Controls from "./components/Controls";
import Card from "../../../../components/Card";


export default function Table() {
  const { id: tableId } = useParams<{ id: string }>();
  const router = useRouter();

  const [cards, setCards] = useState<CardDto[]>([]);
  const [publicCards, setPublicCards] = useState<CardDto[]>([]);
  const [amount, setAmount] = useState<number>(0);
  const [showBetInput, setShowBetInput] = useState<boolean>(false);
  const [playerTurn, setPlayerTurn] = useState<boolean>(false);
  const [winnerNames, setWinnerNames] = useState<string[] | null>(null);
  const [playerActions, setPlayerActions] = useState<Record<string, PlayerActionNotification | null>>({});
  const [currentTurn, setCurrentTurn] = useState<string | null>(null);
  const [isHost, setIsHost] = useState<boolean>(false);
  const [players, setPlayers] = useState<PlayerStateDto[]>([]);

  const playerIdRef = useRef<string | null>(null);
  const tableClientRef = useRef<tableClient | null>(null);
  const listenersAttached = useRef(false);

  const otherPlayers = players.filter(p => p.id !== playerIdRef.current);
  const seatPositions = getSeatPositions(otherPlayers.length);

  const attachLobbyListeners = useCallback(async () => {
    if (!tableClientRef.current) tableClientRef.current = await getTableClient(tableId);
    const client = tableClientRef.current;

    client.onReceiveGameState((gameState: GameStateDto) => {
      setWinnerNames(null);
      setPlayerActions({});
      setPlayers(gameState.players);
      setPublicCards(gameState.communityCards);
      setCurrentTurn(gameState.currentTurnPlayerId ?? null);

      const self = gameState.players.find(p => p.isSelf);
      if (self) {
        playerIdRef.current = self.id;
        setCards(self.cards ?? []);
        setPlayerTurn(self.isCurrentTurn ?? false);
      }

      if (gameState.hostingPlayerId === playerIdRef.current) setIsHost(true);
    });

    client.onGamePhaseUpdate((_: GamePhase, cards: CardDto[]) => {
      setPublicCards(prev => [...prev, ...cards]);
      setPlayerActions({});
    });

    client.onTurn(() => setPlayerTurn(true));

    client.onHostChange(() => setIsHost(true));

    client.onGameClose(() => { alert("Game closed"); router.push(`/`); });

    client.onPlayerAction((playerId, notification) => {
      if (notification.type === "Turn") {
        setCurrentTurn(playerId);
        setPlayerActions(prev => { const { [playerId]: _, ...rest } = prev; return rest; });
      } else if (notification.type === "Disconnect") {
        setPlayers(prev => prev.map(p => p.id === playerId ? { ...p, isDisconnected: true } : p));
      } else if (notification.type === "Reconnect") {
        setPlayers(prev => prev.map(p => p.id === playerId ? { ...p, isDisconnected: false } : p));
      } else if (notification.type === "Fold" || notification.type === "AllIn") {
        setPlayers(prev => prev.map(p => p.id === playerId
          ? { ...p, isFolded: notification.type === "Fold" ? true : p.isFolded, isAllIn: notification.type === "AllIn" ? true : p.isAllIn }
          : p
        ));
      } else {
        setPlayerActions(prev => ({ ...prev, [playerId]: notification }));
        setCurrentTurn(prev => (prev === playerId ? null : prev));
      }
    });

    client.onShowdown((winnerIds, _, playerStates) => {
      setPlayers(prev => {
        const winningNames = prev.filter(p => winnerIds.includes(p.id)).map(p => p.username);
        if (winningNames.length > 0) setWinnerNames(winningNames);
        return playerStates;
      });
    });
  }, []);

  useEffect(() => {
    if (!listenersAttached.current) {
      attachLobbyListeners();
      listenersAttached.current = true;
    }
  }, []);

  const handleAction = useCallback(async (action: "placeBet" | "fold" | "allIn" | "check" | "startNextHand" | "closeGame") => {
    const client = await getTableClient(tableId);
    let result;
    switch(action){
      case "placeBet": result = await client.placeBet(amount); break;
      case "fold": result = await client.fold(); break;
      case "allIn": result = await client.allIn(); break;
      case "check": result = await client.check(); break;
      case "startNextHand": result = await client.startNextHand(); break;
      case "closeGame": result = await client.closeGame(); break;
    }
    if(result?.isFailure) alert(result.response.message.message);
    else if(["placeBet","fold","allIn","check"].includes(action)) setPlayerTurn(false);
  }, [amount, tableId]);

  return (
    <div className="relative w-full h-screen flex items-center justify-center">

      {/* Winner announcement */}
      {winnerNames && (
        <div className="absolute top-6 left-1/2 -translate-x-1/2 z-20 bg-black/70 text-white px-6 py-3 rounded-xl shadow-lg">
          <h1 className="text-2xl font-bold">Winner(s): {winnerNames.join(", ")}</h1>
        </div>
      )}

      {/* Table container */}
      <div className="relative w-full max-w-screen-xl min-w-[200px] aspect-[16/9]">
        <Image src="/pokerTable.png" alt="Poker table" fill className="object-cover z-0" priority />

        {/* Community Cards */}
        {publicCards.length > 0 && <CommunityCards cards={publicCards} />}

        {/* Other Players */}
        {otherPlayers.map((p, i) => (
          <Player
            key={p.id}
            player={p}
            index={i}
            seatPosition={seatPositions[i]}
            playerActions={playerActions}
            currentTurn={currentTurn}
            winnerNames={winnerNames}
          />
        ))}

        {/* Self Player Cards */}
        {cards.length === 2 && (
          <div className="absolute bottom-[1%] left-[50%] -translate-x-1/2 flex gap-[4%] w-[25%] justify-center">
            {cards.map((card, i) => <div key={i} className="w-[45%]"><Card rank={card.rank} suit={card.suit} /></div>)}
          </div>
        )}
      </div>

      {/* Bottom Controls */}
      <Controls
        amount={amount} setAmount={setAmount} showBetInput={showBetInput} playerTurn={playerTurn}
        isHost={isHost} winnerNames={winnerNames}
        onPlaceBet={() => handleAction("placeBet")}
        onFold={() => handleAction("fold")}
        onAllIn={() => handleAction("allIn")}
        onCheck={() => handleAction("check")}
        onStartNextHand={() => handleAction("startNextHand")}
        onCloseGame={() => handleAction("closeGame")}
      />
    </div>
  );
}