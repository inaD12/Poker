'use client'

import Image from "next/image";
import { useParams } from "next/navigation";
import { useCallback, useEffect, useState } from "react";
import { getTableClient } from "../../../../table/services/table.client";
import { CardDto, CardRank, CardSuit, GamePhase, GameStateDto } from "../../../../table/types/table.types";
import Card from "../../../../components/Card";

export default function Table() {
  const { id: tableId } = useParams<{ id: string }>();
  const [gameState, setGameState] = useState<GameStateDto | null>(null);
  const [cards, setCards] = useState<CardDto[]>([]);
  const [publicCards, setPublicCards] = useState<CardDto[]>([]);
  const [amount, setAmount] = useState<number>(0);
  const [showBetInput, setShowBetInput] = useState<boolean>(false);

  const attachLobbyListeners = useCallback(async () => {
    const tableClient = await getTableClient(tableId);

    tableClient.onReceiveGameState((gameStateDto: GameStateDto) => {
      setGameState(gameStateDto);
      setPublicCards(gameStateDto.communityCards);

      const player = gameStateDto.players.find(p => p.cards !== null);
      setCards(player?.cards ?? []);
    });

    tableClient.onGamePhaseUpdate((gamePhase: GamePhase, cards: CardDto[]) => {
      setPublicCards((prevPublicCards) => [...prevPublicCards, ...cards]);
    });
    
  }, [tableId]);

  useEffect(() => {
    const joinTable = async () => {
      await attachLobbyListeners();
    };
    joinTable();
  }, [attachLobbyListeners]);

  const handleClickPlaceBet = useCallback(async () => {
    if (!showBetInput) {
      setShowBetInput(true);
      return;
    }

    const tableClient = await getTableClient(tableId);
    const result = await tableClient.placeBet(amount);
    if (result.isFailure) alert(result.response.message.message);

    setShowBetInput(false);
  }, [amount, tableId, showBetInput]);

  const handleClickFold = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.fold();
    if (result.isFailure) alert(result.response.message.message);
  }, [tableId]);

  const handleClickAllIn = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.allIn();
    if (result.isFailure) alert(result.response.message.message);
  }, [tableId]);

  const handleClickCheck = useCallback(async () => {
    const tableClient = await getTableClient(tableId);
    const result = await tableClient.check();
    if (result.isFailure) alert(result.response.message.message);
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
        {gameState && gameState.communityCards.length > 0 && (
          <div className="absolute top-[38.9%] left-[50%] -translate-x-1/2 flex gap-[1.9%] w-[39%]">
            {gameState.communityCards.map((card, index) => (
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
        </div>
      </div>
    </div>
  );
}