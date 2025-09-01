import { signalRRoutes } from "../../api/apiEndpoints.config";
import { HubResult } from "../../lobby/types/lobby.types";
import signalRService, { ISignalRService } from "../../utilities/signalr.service";
import { CardDto, GamePhase, GameStateDto, PlayerActionNotification, PlayerStateDto } from "../types/table.types";

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

  onPlayerAction(callback: (playerId: string, notification: PlayerActionNotification) => void) {
    this.signalRService.on("PlayerAction", (playerId: string, notification: PlayerActionNotification) => {
      callback(playerId, notification);
    });
  }

  onGamePhaseUpdate(callback: (gamePhase: GamePhase, cards:CardDto[]) => void) {
    this.signalRService.on("GamePhaseUpdate", (gamePhase: GamePhase, cards:CardDto[]) => {
      callback(gamePhase, cards);
    });
  }

  onShowdown(callback: (winnerPlayerIds: string[], winningsEach:number, playerStates:PlayerStateDto[]) => void) {
    this.signalRService.on("Showdown", (winnerPlayerIds: string[], winningsEach:number, playerStates:PlayerStateDto[]) => {
      callback(winnerPlayerIds, winningsEach, playerStates);
    });
  }

  onTurn(callback: () => void) {
    this.signalRService.on("YourTurn", () => {
      callback();
    });
  }

  onGameClose(callback: () => void) {
    this.signalRService.on("GameClose", () => {
      callback();
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

  async closeGame(): Promise<HubResult<null>>{
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
