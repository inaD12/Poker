import { signalRRoutes } from "../../api/apiEndpoints.config";
import signalRService, { ISignalRService } from "../../utilities/signalr.service";
import {HubResult, CreateLobbyResponse, LobbyQueryResponse } from "../types/lobby.types";

export default class lobbyClient{
 private signalRService: ISignalRService;

  constructor(signalRService: ISignalRService) {
    this.signalRService = signalRService;
  }

  async connect() {
    await this.signalRService.startConnection(signalRRoutes.lobby.hub);

    // this.signalRService.on("LobbiesUpdated", (lobbies: Lobby[]) => {
    //   // handle lobby update
    // });

    // // other event handlers...
  }

  async createLobby(lobbyName: string): Promise<CreateLobbyResponse> {
    const result = await this.signalRService.send("CreateLobby", lobbyName);
    return result.value.id;
  }


  async joinLobby(lobbyId: string): Promise<HubResult<LobbyQueryResponse>>{
    const result =  await this.signalRService.send("JoinLobby", lobbyId);

    return result
  }

  async leaveLobby(lobbyId: string): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("LeaveLobby", lobbyId);

    return result
  }

  disconnect() {
    this.signalRService.stop();
  }
}


let clientInstance: lobbyClient | null = null;

export async function getLobbyClient() {
  if (!clientInstance) {
    clientInstance = new lobbyClient(signalRService);
    await clientInstance.connect();
  }
  return clientInstance;
}
