import { signalRRoutes } from "../../api/apiEndpoints.config";
import signalRService, { ISignalRService } from "../../utilities/signalr.service";
import { GameStateDto } from "../types/table.types";

export default class tableClient{
 private signalRService: ISignalRService;

  constructor(signalRService: ISignalRService) {
    this.signalRService = signalRService;
  }

  async connect(tableId: string) {
  await this.signalRService.startConnection(
    `${signalRRoutes.game.hub}?tableId=${tableId}`
  );
}


  onReceiveGameState(callback: (gameStateDto: GameStateDto) => void) {
    this.signalRService.on("ReceiveGameState", (gameStateDto: GameStateDto) => {
      callback(gameStateDto);
    });
  }

  // async joinLobby(lobbyId: string): Promise<HubResult<LobbyQueryResponse>>{
  //   const result =  await this.signalRService.send("JoinLobby", lobbyId);

  //   return result
  // }

  disconnect() {
    this.signalRService.stop();
  }
}


let clientInstance: tableClient | null = null;

export async function getTableClient(tableId: string) {
  if (!clientInstance) {
    clientInstance = new tableClient(signalRService);
    await clientInstance.connect(tableId);
  }
  return clientInstance;
}
