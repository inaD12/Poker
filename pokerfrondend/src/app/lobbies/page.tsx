import lobbyService from "../../../lobby/services/lobby.services";
import { LobbyQueryResponse } from "../../../lobby/types/lobby.types";
import CreateLobbyButton from "./components/CreateLobbyButton";
import LobbiesPageWrapper from "./components/LobbiesPageWrapper";

const LobbiesPage = async () => {
  const response = await lobbyService.getAll();
  const lobbies: LobbyQueryResponse[] = response?.data.items ?? [];

  return (
  <div className="h-screen w-full flex flex-col items-center justify-start py-3">

    <h1 className="text-3xl font-bold">Available Lobbies</h1>
    
    <div className="flex items-center justify-center md:py-4 flex-grow w-full max-w-5xl">
      <div className="flex flex-col w-200 h-170 border-4 border-white bg-[#437057] items-start max-h-full overflow-auto">
        <LobbiesPageWrapper
         lobbies={lobbies}
          />
      </div>
    </div>
    <CreateLobbyButton />
  </div>
  );
};

export default LobbiesPage;