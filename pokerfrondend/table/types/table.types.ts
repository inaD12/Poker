export enum GamePhase {
  PreFlop = "PreFlop",
  Flop = "Flop",
  Turn = "Turn",
  River = "River",
  Showdown = "Showdown",
}

export enum CardSuit {
  Hearts = 0,
  Diamonds = 1,
  Clubs = 2,
  Spades = 3,
}

export enum CardRank {
  Two = 0,
  Three = 1,
  Four = 2,
  Five = 3,
  Six = 4,
  Seven = 5,
  Eight = 6,
  Nine = 7,
  Ten = 8,
  Jack = 9,
  Queen = 10,
  King = 11,
  Ace = 12,
}

export interface CardDto {
  suit: CardSuit;
  rank: CardRank;
}


export interface PlayerStateDto {
  id: string;
  username: string;
  balance: number;
  isFolded: boolean;
  isAllIn: boolean;
  currentBet: number;
  isCurrentTurn: boolean;
  isDisconnected: boolean;
  isSelf: boolean;
  cards?: CardDto[];
}

export interface GameStateDto {
  phase: GamePhase;
  communityCards: CardDto[];
  currentPot: number;
  currentBet: number;
  minimumRaise: number;
  currentTurnPlayerId?: string;
  dealerPlayerId: string;
  players: PlayerStateDto[];
}

export type PlayerActionNotification =
  | PlayerBetNotification
  | PlayerFoldNotification
  | PlayerAllInNotification
  | PlayerCheckNotification
  | PlayerTurnNotification;

export interface PlayerBetNotification {
  type: "Bet";
  amount: number;
}

export interface PlayerFoldNotification {
  type: "Fold";
}

export interface PlayerAllInNotification {
  type: "AllIn";
}

export interface PlayerCheckNotification {
  type: "Check";
}

export interface PlayerTurnNotification {
  type: "Turn";
}

