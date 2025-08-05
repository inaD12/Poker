export interface CreateLobbyResponse{
    id: string;
}

export interface PlayerInfoDto {
  id: string;
  username: string;
  gamesPlayed: number;
  gamesWon: number;
  totalEarnings: number;
}

export interface LobbyQueryViewModel {
  id: string;
  createdAt: string;
  players: PlayerInfoDto[];
  isFull: boolean;
  isReadyToStart: boolean;
}

export interface LobbyPaginatedQueryViewModel {
  items: LobbyQueryViewModel[];
  page: number;
  pageSize: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
