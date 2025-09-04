'use client';

interface LobbyControlsProps {
  lobby: any;
  isCreator: boolean;
  funds: number;
  setFunds: (val: number) => void;
  isAddingFunds: boolean;
  onLeave: () => void;
  onStartGame: () => void;
  onAddFunds: () => void;
}

export default function LobbyControls({
  lobby, isCreator, funds, setFunds, isAddingFunds,
  onLeave, onStartGame, onAddFunds
}: LobbyControlsProps) {
  return (
    <div className="relative md:absolute md:top-10 md:left-30 flex flex-col justify-between w-full md:w-[45%] max-w-full md:max-w-50 h-auto md:h-150 p-3 rounded bg-transparent">
      <div className="flex flex-col items-center gap-3">
        <p className="text-2xl md:text-3xl font-bold">{lobby.name}</p>
        <p className="text-base md:text-l font-semibold">Creator: {lobby.creator}</p>

        <button className="bg-red-700 hover:bg-red-800 rounded-md h-[50px] w-[150px]" onClick={onLeave}>
          Leave Game
        </button>

        {isCreator && (
          <button className="bg-green-700 hover:bg-green-800 rounded-md h-[50px] w-[150px]" onClick={onStartGame}>
            Start Game
          </button>
        )}
      </div>

      <div className="flex flex-col items-center gap-2 mt-4">
        <button
          className="bg-blue-700 hover:bg-blue-800 rounded-md h-[40px] w-[100px] text-white disabled:opacity-50"
          onClick={onAddFunds}
          disabled={isAddingFunds}
        >
          {isAddingFunds ? 'Adding...' : 'Add Funds'}
        </button>
        <input
          type="number"
          value={funds}
          onChange={e => setFunds(Number(e.target.value))}
          className="rounded-md px-2 py-1 w-32 text-black"
          placeholder="Funds"
          min={1}
        />
      </div>
    </div>
  );
}
