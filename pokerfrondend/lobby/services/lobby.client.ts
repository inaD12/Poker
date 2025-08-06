import { signalRRoutes } from "../../api/apiEndpoints.config";
import signalRService, { ISignalRService } from "../../utilities/signalr.service";

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

  createLobby(lobbyName: string) {
    return this.signalRService.send("CreateLobby", lobbyName);
  }

  disconnect() {
    this.signalRService.stop();
  }
}


let clientInstance: lobbyClient | null = null;

export function getLobbyClient() {
  if (!clientInstance) {
    clientInstance = new lobbyClient(signalRService);
    clientInstance.connect();
  }
  return clientInstance;
}