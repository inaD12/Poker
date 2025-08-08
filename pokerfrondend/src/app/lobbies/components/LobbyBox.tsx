import JoinButton from "./JoinButton";

interface LobbyBoxProps {
  lobbyId: string
  lobbyName: string;
  creator: string;
  createdAt: string;
  players: string;
}

const LobbyBox: React.FC<LobbyBoxProps> = ({
  lobbyId,
  lobbyName,
  creator,
  createdAt,
  players,
}) => {
  return (
    <div className="relative border-b-4 border-white min-h-[120px] w-full bg-[#2F5249]">
      <p className="absolute top-2 left-3 font-bold text-lg">{lobbyName}</p>
      <p className="absolute top-9 left-3">Creator: {creator}</p>
      <p className="absolute bottom-2 left-3">Created at: {createdAt}</p>
      <p className="absolute top-4 right-12">{players} Players</p>
      <JoinButton lobbyId={lobbyId}/>
    </div>
  );
};

export default LobbyBox;
