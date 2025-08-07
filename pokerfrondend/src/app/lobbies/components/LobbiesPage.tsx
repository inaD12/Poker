import { APIResponse, LobbyPaginatedQueryResponse, LobbyQueryResponse } from "../../../../lobby/types/lobby.types";
import CreateLobbyButton from "./CreateLobbyButton";
import Pagination from "../../../../components/Pagination";
import LobbyBox from "./LobbyBox";

const LobbiesPage = ({ 
    page, 
    response
 }: { 
    page: number, 
    response: APIResponse<LobbyPaginatedQueryResponse>
 }) => {

const lobbies: LobbyQueryResponse[] = response?.data.items ?? [];

  return (
  <div className="h-screen w-full flex flex-col items-center justify-start py-3">

    <h1 className="text-3xl font-bold">Available Lobbies</h1>
    
    <div className="flex items-center justify-center md:py-4 flex-grow w-full max-w-5xl">
      <div className="flex flex-col w-200 h-182 border-4 border-white bg-[#437057] items-start max-h-full overflow-auto">
        {lobbies.length === 0 ? (
          <div className="flex flex-grow items-center justify-center w-full p-4">
            <p className="text-white italic text-center">No lobbies available.</p>
          </div>
        ) : (
          lobbies.map((lobby) => (
            <LobbyBox
              key={lobby.id}
              gameName={lobby.name}
              creator={lobby.creator}
              createdAt={lobby.createdAt}
              players={`${lobby.players.length}/6`}
            />
          ))
        )}
      </div>
    </div>

    <CreateLobbyButton />

    <Pagination 
    totalCount={response.data.totalCount} 
    currentPage={page} 
    route={"lobbies"} 
    pageSize={6} />

  </div>
  );
};

export default LobbiesPage;