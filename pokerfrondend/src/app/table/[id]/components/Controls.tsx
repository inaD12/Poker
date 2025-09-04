interface ControlsProps {
  amount: number;
  setAmount: (n: number) => void;
  showBetInput: boolean;
  playerTurn: boolean;
  isHost: boolean;
  winnerNames: string[] | null;
  onPlaceBet: () => void;
  onFold: () => void;
  onAllIn: () => void;
  onCheck: () => void;
  onStartNextHand: () => void;
  onCloseGame: () => void;
}

export default function Controls({
  amount, setAmount, showBetInput, playerTurn, isHost, winnerNames,
  onPlaceBet, onFold, onAllIn, onCheck, onStartNextHand, onCloseGame
}: ControlsProps) {
  return (
    <div className="absolute bottom-4 w-full flex flex-col items-center gap-3 z-10">
      {showBetInput && <input type="number" value={amount} onChange={(e) => setAmount(Number(e.target.value))} className="border p-2 w-40 text-center" placeholder="Bet amount" />}
      {playerTurn && <div className="bg-yellow-600 text-white px-3 py-1 rounded-full text-sm shadow-md animate-pulse">Your turn</div>}
      <div className="flex gap-2">
        <button className="bg-amber-600 p-2" onClick={onPlaceBet}>{showBetInput ? "Confirm Bet" : "Place Bet"}</button>
        <button className="bg-amber-600 p-2" onClick={onFold}>Fold</button>
        <button className="bg-amber-600 p-2" onClick={onAllIn}>All In</button>
        <button className="bg-amber-600 p-2" onClick={onCheck}>Check</button>
        {isHost && winnerNames && <>
          <button className="bg-amber-600 p-2" onClick={onStartNextHand}>Start Next Hand</button>
          <button className="bg-amber-600 p-2" onClick={onCloseGame}>Close Game</button>
        </>}
      </div>
    </div>
  );
}
