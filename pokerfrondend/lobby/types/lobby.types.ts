export interface CreateLobbyResponse{
    id: string;
}

export interface PlayerInfoDto {
  id: string;
  username: string;
  gamesPlayed: number;
  gamesWon: number;
  totalEarnings: number;
  isSelf: boolean;
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

export interface HubResponse{
  message: Message;
}

export interface Message{
  message: string;
}

export interface HubResult<T> {
  isFailure: boolean;
  isSuccess: boolean;
  response: HubResponse;
  value: T;
}

export interface APIResponse<T>{
  message:string;
  data: T;
}