import lobbyService from "../../../lobby/services/lobby.services";
import { LobbyQueryViewModel } from "../../../lobby/types/lobby.types";
import LobbyBox from "./components/LobbyBox";

const LobbiesPage = async () => {
  const response = await lobbyService.getAll();
  const lobbies: LobbyQueryViewModel[] = response?.items ?? [];


  return (
  <div className="h-screen w-full flex flex-col items-center justify-start py-3">

    <h1 className="text-3xl font-bold">Available Lobbies</h1>
    
    <div className="flex items-center justify-center md:py-4 flex-grow w-full max-w-5xl">
      <div className="flex flex-col w-200 h-170 border-4 border-white bg-[#437057] items-start max-h-full overflow-auto">
        <LobbyBox
          gameName="inaD's Game"
          creator="inaD"
          createdAt="2025-08-05"
          players="3/6"
        />
      </div>
    </div>
    
    <button className="bg-[#E3DE61] hover:bg-[#b1ad50] text-black font-bold px-6 py-3 rounded-md shadow-md h-fit">
      Create New
    </button>
  </div>
);

  // return (
  //   <div className="flex flex-col items-center justify-center min-h-screen">
  //     <h1 className="text-2xl font-bold mb-4">Available Lobbies</h1>

  //     {lobbies.length === 0 ? (
  //       <p className="text-gray-500 text-lg">No lobbies available at the moment.</p>
  //     ) : (
  //       <ul className="space-y-4">
  //         {lobbies.map((lobby) => (
  //           <li key={lobby.id} className="border p-4 rounded">
  //             <p><strong>ID:</strong> {lobby.id}</p>
  //             <p><strong>Players:</strong> {lobby.players.length}</p>
  //             <p><strong>Full:</strong> {lobby.isFull ? "Yes" : "No"}</p>
  //             <p><strong>Ready:</strong> {lobby.isReadyToStart ? "Yes" : "No"}</p>
  //           </li>
  //         ))}
  //       </ul>
  //     )}
  //   </div>
  // );
};

export default LobbiesPage;