import apiClient from "../../api/apiClients";
import { apiRoutes } from "../../api/apiEndpoints.config";
import {APIResponse, LobbyPaginatedQueryResponse } from "../types/lobby.types";

const lobbyService = {
    getAll: async (): Promise<APIResponse<LobbyPaginatedQueryResponse>> => {
        const response = await apiClient.get<APIResponse<LobbyPaginatedQueryResponse>>(
            apiRoutes.lobby.getAll
        );
        return response.data;
    }
}

export default lobbyService;