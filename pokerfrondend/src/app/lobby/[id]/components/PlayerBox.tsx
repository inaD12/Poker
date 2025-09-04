
interface PlayerBoxProps {
  username: string
  handsPlayed: number;
  handsWon: number;
  totalEarnings: number;
  balance: number
}

const PlayerBox: React.FC<PlayerBoxProps> = ({
  username,
  handsPlayed,
  handsWon,
  totalEarnings,
  balance
}) => {
  return (
  <div className="relative w-full h-20 bg-[#2F5249] p-2 mb-3 border-1 border-dotted border-white rounded-md text-white">
    <p className="font-bold text-lg">{username}</p>
    <div className="flex justify-between text-md mt-3">
      <p>Balance: {balance}</p>
      <p>Hands played: {handsPlayed}</p>
      <p>Hands won: {handsWon}</p>
      <p>Total earnings: {totalEarnings}</p>
    </div>
  </div>
);


};

export default PlayerBox;
