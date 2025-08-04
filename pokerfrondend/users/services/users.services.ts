import apiClient from "../../api/apiClients";
import apiRoutes from "../../api/apiEndpoints.config";
import type { LoginUserRequest,RegisterUserRequest, RegisterUserResponse } from "../types/users.types";

const userService = {
    login: async (data: LoginUserRequest) => {
        return await apiClient.post(apiRoutes.users.login, data);
    },

    logout: async () => {
        return await apiClient.post(apiRoutes.users.logout);
    },

    checkAuth: async () => {
        return await apiClient.get(apiRoutes.users.checkAuth);
    },

    register: async (data: RegisterUserRequest): Promise<RegisterUserResponse> => {
        const response = await apiClient.post<RegisterUserResponse>(
            apiRoutes.users.register,
            data
        );
        return response.data;
    }
}

export default userService;