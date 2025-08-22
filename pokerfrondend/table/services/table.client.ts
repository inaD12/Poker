import { signalRRoutes } from "../../api/apiEndpoints.config";
import { HubResult } from "../../lobby/types/lobby.types";
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

  async placeBet(amount: number): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("PlaceBet", amount);

    return result
  }

  async fold(): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("Fold");

    return result
  }

  async allIn(): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("AllIn");

    return result
  }

  async check(): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("Check");

    return result
  }

  async startNextHand(): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("StartNextHand");

    return result
  }

  async closeGame(amount: number): Promise<HubResult<null>>{
    const result =  await this.signalRService.send("CloseGame");

    return result
  }

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
