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

export interface LobbyQueryResponse {
  id: string;
  name:string,
  creator:string,
  createdAt: string;
  players: PlayerInfoDto[];
  isFull: boolean;
  isReadyToStart: boolean;
}

export interface LobbyPaginatedQueryResponse {
  items: LobbyQueryResponse[];
  page: number;
  pageSize: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface APIResponse<T> {
  data: T;
  message: string;
}
