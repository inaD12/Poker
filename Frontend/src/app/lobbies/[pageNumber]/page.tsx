'use client';

import {useEffect, useState } from "react";
import lobbyService from "../../../../lobby/services/lobby.services";
import LobbiesPage from "../components/LobbiesPage";
import { APIResponse, LobbyPaginatedQueryResponse } from "../../../../lobby/types/lobby.types";
import React from "react";
import { useParams } from "next/navigation";


export default function LobbiesPaged() {
    const params = useParams<{ pageNumber: string }>();
    const pageNumber = Number(params.pageNumber);
    const [response, setResponse] = useState<APIResponse<LobbyPaginatedQueryResponse> | null>(null);
    const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchLobbies() {
      setLoading(true);
      const res = await lobbyService.getAll(pageNumber, 6);
      setResponse(res);
      setLoading(false);
    }
    fetchLobbies();
  }, [pageNumber]);

  if (loading) return <div>Loading...</div>;

  return <LobbiesPage page={pageNumber} response={response!} />;
}
