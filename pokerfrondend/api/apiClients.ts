import axios from "axios";
import { apiRoutes } from "./apiEndpoints.config";

const apiClient = axios.create({
  baseURL: apiRoutes.baseUrl,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

apiClient.interceptors.request.use(async (config) => {
  if (typeof window === "undefined") {
    try {
      const { cookies } = require("next/headers");
      const cookieStore = await cookies();
      const authToken = cookieStore.get("auth_token")?.value;
      if (authToken) {
        config.headers = config.headers || {};
        config.headers["Cookie"] = `auth_token=${authToken}`;
      }
    } catch (e) {
      // TODO: Log error
    }
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      if (error.response.status === 401) {
        console.log("Unauthorized access");
        //TODO: Log out
      }
    } else {
      console.error(error.message);
    }
    return Promise.reject(error);
  }
);

export default apiClient;
