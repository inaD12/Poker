import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

export interface ISignalRService {
  startConnection(url: string): Promise<void>;
  stop(): void;
  on(eventName: string, callback: (...args: any[]) => void): void;
   send(eventName: string, ...args: any[]): Promise<any> | undefined;
}

class SignalRService implements ISignalRService {
  private connection: HubConnection | null = null;

  async startConnection(url: string) {
      this.connection = new HubConnectionBuilder()
      .withUrl(url)
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    this.connection.onreconnecting(() => {
      console.log("Reconnecting...");
    });

    this.connection.onreconnected(() => {
      console.log("Reconnected!");
    });

    this.connection.onclose((error) => {
      console.log("Connection closed due to error:", error);
    });

    try {
      await this.connection.start();
      console.log("SignalR connected ✅");
    } catch (err) {
      console.error("Error starting SignalR connection:", err);
    }
  }

  on(eventName: string, callback: (...args: any[]) => void) {
    this.connection?.on(eventName, callback);
  }

  send(eventName: string, ...args: any[]) {
    return this.connection?.invoke(eventName, ...args).catch((err) => console.error(err));
  }

  stop() {
    this.connection?.stop();
  }
}

const signalRService = new SignalRService();
export default signalRService;