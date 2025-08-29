'use client'

import Image from "next/image";
import { useParams } from "next/navigation";
import { useCallback, useEffect, useRef, useState } from "react";
import tableClient, { getTableClient } from "../../../../table/services/table.client";
import { CardDto, GamePhase, GameStateDto } from "../../../../table/types/table.types";
import Card from "../../../../components/Card";

export default function Table() {
  const { id: tableId } = useParams<{ id: string }>();

  const [cards, setCards] = useState<CardDto[]>([]);
  const [publicCards, setPublicCards] = useState<CardDto[]>([]);
  const [amount, setAmount] = useState<number>(0);
  const [showBetInput, setShowBetInput] = useState<boolean>(false);
  const [playerTurn, setPlayerTurn] = useState<boolean>(false);
  const [winnerNames, setWinnerNames] = useState<string[] | null>(null);

  const playerIdRef = useRef<string | null>(null);
  const tableClientRef = useRef<tableClient | null>(null);
  const playersRef = useRef<GameStateDto["players"]>([]);

  const attachLobbyListeners = useCallback(async () => {
    if (!tableClientRef.current) {
      tableClientRef.current = await getTableClient(tableId);
    }
    const tableClient = tableClientRef.current;

    tableClient.onReceiveGameState((gameStateDto: GameStateDto) => {
      setWinnerNames(null);
      playersRef.current = gameStateDto.players;
      setPublicCards(gameStateDto.communityCards);

      const player = gameStateDto.players.find(p => p.cards !== null);
      if (player) {
        playerIdRef.current = player.id;
        setCards(player.cards ?? []);
        setPlayerTurn(player.isCurrentTurn ?? false);
      }
    });

    tableClient.onGamePhaseUpdate((gamePhase: GamePhase, cards: CardDto[]) => {
      setPublicCards(prev => [...prev, ...cards]);
    });

    tableClient.onTurn(() => {
      setPlayerTurn(true);
    });

    tableClient.onShowdown((winnerPlayerIds: string[], winningsEach:number) => {
      const winningPlayerNames = playersRef.current
        .filter(p => winnerPlayerIds.includes(p.id))
        .map(p => p.username);

      if (winningPlayerNames.length > 0) {
        setWinnerNames(winningPlayerNames);
      }
    });
  }, [tableId]);

  useEffect(() => {
    attachLobbyListeners();
  }, [attachLobbyListeners]);

  const handleClickPlaceBet = useCallback(async () => {
    if (!showBetInput) {
      setShowBetInput(true);
      return;
    }

    const tableClient = await getTableClient(tableId);
    const result = await tableClient.placeBet(amount);
    if (result.isFailure) alert(result.response.message.message);
    else setPlayerTurn(false);

    setShowBetInput(false);
  }, [amount, tableId, showBetInput]);

  const handleClickFold = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.fold();
    if (result.isFailure) alert(result.response.message.message);
    else setPlayerTurn(false);
  }, [tableId]);

  const handleClickAllIn = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.allIn();
    if (result.isFailure) alert(result.response.message.message);
    else setPlayerTurn(false);
  }, [tableId]);

  const handleClickCheck = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.check();
    if (result.isFailure) alert(result.response.message.message);
    else setPlayerTurn(false);
  }, [tableId]);

  const handleClickStartNextHand = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.startNextHand();
    if (result.isFailure) {
      alert(result.response.message.message);
    }
  }, [tableId]);

  const handleClickCloseGame = useCallback(async () => {
    const tableClient = await getTableClient(tableId);  
    const result = await tableClient.closeGame();
    if (result.isFailure) {
      alert(result.response.message.message);
    }
  }, [tableId]);
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
      <Image
        src="/pokerTable.png"
        alt="Poker table"
        fill
        className="object-cover z-0"
        priority
      />

      {/* Community Cards */}
      {publicCards.length > 0 && (
        <div className="absolute top-[38.9%] left-[50%] -translate-x-1/2 flex gap-[1.9%] w-[39%]">
          {publicCards.map((card, index) => (
            <div key={index} className="w-[18%]">
              <Card rank={card.rank} suit={card.suit} />
            </div>
          ))}
        </div>
      )}

      {/* Player Cards */}
      {cards.length === 2 && (
        <div className="absolute bottom-[1%] left-[50%] -translate-x-1/2 flex gap-[4%] w-[25%] justify-center">
          {cards.map((card, index) => (
            <div key={index} className="w-[45%]">
              <Card rank={card.rank} suit={card.suit} />
            </div>
          ))}
        </div>
      )}
    </div>

    {/* Bottom controls */}
    <div className="absolute bottom-4 w-full flex flex-col items-center gap-3 z-10">
      {showBetInput && (
        <input
          type="number"
          value={amount}
          onChange={(e) => setAmount(Number(e.target.value))}
          className="border p-2 w-40 text-center"
          placeholder="Bet amount"
        />
      )}

      <div className="flex gap-2">
        <button className="bg-amber-600 p-2" onClick={handleClickPlaceBet}>
          {showBetInput ? "Confirm Bet" : "Place Bet"}
        </button>
        <button className="bg-amber-600 p-2" onClick={handleClickFold}>Fold</button>
        <button className="bg-amber-600 p-2" onClick={handleClickAllIn}>All In</button>
        <button className="bg-amber-600 p-2" onClick={handleClickCheck}>Check</button>
        <button className="bg-amber-600 p-2" onClick={handleClickStartNextHand}>Start Next Hand</button>
        <button className="bg-amber-600 p-2" onClick={handleClickCloseGame}>Close Game</button>
        {playerTurn && (
          <h1>Your turn</h1>
        )}
      </div>
    </div>
  </div>
);
}