export enum GamePhase {
  PreFlop = "PreFlop",
  Flop = "Flop",
  Turn = "Turn",
  River = "River",
  Showdown = "Showdown",
}

export enum CardSuit {
  Hearts = "Hearts",
  Diamonds = "Diamonds",
  Clubs = "Clubs",
  Spades = "Spades",
}

export enum CardRank {
  Two = "Two",
  Three = "Three",
  Four = "Four",
  Five = "Five",
  Six = "Six",
  Seven = "Seven",
  Eight = "Eight",
  Nine = "Nine",
  Ten = "Ten",
  Jack = "Jack",
  Queen = "Queen",
  King = "King",
  Ace = "Ace",
}

export interface CardDto {
  suit: CardSuit;
  rank: CardRank;
}

export interface PlayerStateDto {
  id: string;
  balance: number;
  isFolded: boolean;
  isAllIn: boolean;
  currentBet: number;
  isCurrentTurn: boolean;
  isDisconnected: boolean;
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
