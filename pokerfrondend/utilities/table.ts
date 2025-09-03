import { PlayerActionNotification } from "../table/types/table.types";

export const getSeatPositions = (numPlayers: number) => {
  switch (numPlayers) {
    case 1: return ["absolute top-[5%] left-1/2"];
    case 2: return ["absolute top-[5%] left-[25%]", "absolute top-[5%] left-[75%]"];
    case 3: return ["absolute top-[5%] left-1/2", "absolute top-[20%] left-[85%]", "absolute top-[20%] left-[15%]"];
    case 4: return ["absolute top-[5%] left-[75%]", "absolute top-[70%] left-[85%]", "absolute top-[70%] left-[15%]", "absolute top-[5%] left-[25%]"];
    case 5: return ["absolute top-[5%] left-1/2", "absolute top-[20%] left-[85%]", "absolute top-[70%] left-[85%]", "absolute top-[70%] left-[15%]", "absolute top-[20%] left-[15%]"];
    default: return [];
  }
};

export function formatAction(action: PlayerActionNotification): string {
  switch (action.type) {
    case "Bet": return `Bet ${action.amount}`;
    case "Check": return "Checked";
    default: return "";
  }
}
