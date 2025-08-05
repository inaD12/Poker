import apiClient from "../../api/apiClients";
import apiRoutes from "../../api/apiEndpoints.config";
import signalRService from "../../utilities/signalr.service";
import { CreateLobbyResponse, LobbyPaginatedQueryViewModel } from "../types/lobby.types";

const lobbyService = {
    getAll: async (): Promise<LobbyPaginatedQueryViewModel> => {
        const response = await apiClient.get<LobbyPaginatedQueryViewModel>(
            apiRoutes.lobby.getAll
        );
        return response.data;
    }, 
    
    createLobby: async (): Promise<CreateLobbyResponse> => {
    try {
      const result = await signalRService.send("CreateLobby");
      return result;
    } catch (err) {
      console.error("Failed to invoke CreateLobby:", err);
      throw err;
    }
  },
}

export default lobbyService;