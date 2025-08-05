interface LobbyBoxProps {
  gameName: string;
  creator: string;
  createdAt: string;
  players: string;
}

const LobbyBox: React.FC<LobbyBoxProps> = ({
  gameName,
  creator,
  createdAt,
  players,
}) => {
  return (
    <div className="relative border-b-4 border-white min-h-[120px] w-full bg-[#2F5249]">
      <p className="absolute top-2 left-3 font-bold text-lg">{gameName}</p>
      <p className="absolute top-9 left-3">Creator: {creator}</p>
      <p className="absolute bottom-2 left-3">Created at: {createdAt}</p>
      <p className="absolute top-4 right-12">{players} Players</p>
      <button className="absolute bottom-4 right-3 bg-[#E3DE61] hover:bg-[#b1ad50] rounded-md h-[50px] w-[150px] font-semibold text-black">
        Join
      </button>
    </div>
  );
};

export default LobbyBox;
