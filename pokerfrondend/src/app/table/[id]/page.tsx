'use client'

import Image from "next/image";
import { useParams } from "next/navigation";
import { useCallback, useEffect, useState } from "react";
import { getTableClient } from "../../../../table/services/table.client";
import { CardDto, GameStateDto } from "../../../../table/types/table.types";
import Card from "../../../../components/Card";

export default function Table() {
  const { id: tableId } = useParams<{ id: string }>();
  const [gameState, setGameState] = useState<GameStateDto | null>(null);
  const [cards, setCards] = useState<CardDto[] | null>(null);


const attachLobbyListeners = useCallback(async () => {
  const tableClient = await getTableClient(tableId);

  tableClient.onReceiveGameState((gameStateDto: GameStateDto) => {
    setGameState(gameStateDto);

    const player = gameStateDto.players.find(p => p.cards !== null);

    setCards(player?.cards ?? []);
  });
}, [getTableClient, tableId]);


  useEffect(() => {

    const joinTable = async () => {
      const tableClient = await getTableClient(tableId);
      
      await attachLobbyListeners();
    }

    joinTable();

  }, []);

  return (
    <div className="relative w-full h-screen flex items-center justify-center">
      <div className="relative w-[95vw] h-[95vh]">
        <Image
          src="/pokerTable.png"
          alt="Poker table"
          fill
          className="object-contain"
          priority
        />
      </div>
       {gameState && cards?.map((card, index) => (
          <Card
            key={index}
            rank={card.rank}
            suit={card.suit}
          />
        ))}
    </div>
  );
}