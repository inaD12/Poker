import apiClient from "../../api/apiClients";
import { apiRoutes } from "../../api/apiEndpoints.config";
import { APIResponse, LobbyPaginatedQueryResponse } from "../types/lobby.types";

const lobbyService = {
    getAll: async (pageNumber = 1, pageSize = 10): Promise<APIResponse<LobbyPaginatedQueryResponse>> => {
    const response = await apiClient.get<APIResponse<LobbyPaginatedQueryResponse>>(
        apiRoutes.lobby.getAll,
        {
            params: {
                pageNumber,
                pageSize
            }
        }
    );
    return response.data;
    }
}

export default lobbyService;