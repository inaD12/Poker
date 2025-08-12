
interface PlayerBoxProps {
  username: string
  gamesPlayed: number;
  gamesWon: number;
  totalEarnings: number;
}

const PlayerBox: React.FC<PlayerBoxProps> = ({
  username,
  gamesPlayed,
  gamesWon,
  totalEarnings,
}) => {
  return (
  <div className="relative w-full h-20 bg-[#2F5249] p-2 mb-3 border-1 border-dotted border-white rounded-md text-white">
    <p className="font-bold text-lg">{username}</p>
    <div className="flex justify-between text-md mt-3">
      <p>Games played: {gamesPlayed}</p>
      <p>Games won: {gamesWon}</p>
      <p>Total earnings: {totalEarnings}</p>
    </div>
  </div>
);


};

export default PlayerBox;
