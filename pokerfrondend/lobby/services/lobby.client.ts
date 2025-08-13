import { signalRRoutes } from "../../api/apiEndpoints.config";
import signalRService, { ISignalRService } from "../../utilities/signalr.service";
import {HubResult, CreateLobbyResponse, LobbyQueryResponse, PlayerInfoDto } from "../types/lobby.types";

export default class lobbyClient{
 private signalRService: ISignalRService;

  constructor(signalRService: ISignalRService) {
    this.signalRService = signalRService;
  }

  async connect() {
    await this.signalRService.startConnection(signalRRoutes.lobby.hub);

  }

  onPlayerJoined(callback: (player: PlayerInfoDto) => void) {
    this.signalRService.on("PlayerJoined", (player: PlayerInfoDto) => {
      callback(player);
    });
  }

  onPlayerLeft(callback: (playerId: string) => void) {
    this.signalRService.on("PlayerLeft", (playerId: string) => {
      callback(playerId);
    });
  }

  onGameStarted(callback: (gameId: string) => void) {
    this.signalRService.on("GameStarted", (gameId: string) => {
      callback(gameId);
    });
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

  async startGame(lobbyId: string): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("StartGame", lobbyId);

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
