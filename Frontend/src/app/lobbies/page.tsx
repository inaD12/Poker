export const dynamic = "force-dynamic";

import lobbyService from "../../../lobby/services/lobby.services";
import LobbiesPage from "./components/LobbiesPage";

const LobbiesDefaultPage = async () => {
  try {
    const response = await lobbyService.getAll(1,6);
    return <LobbiesPage page={1} response={response} />;
  } catch (err) {
    console.error("SSR error in /lobbies:", err);
    return <div>Failed to load lobbies.</div>;
  }
};

export default LobbiesDefaultPage;


//This is here so the page works with Azure Static Web Apps
// 'use client';

// import {useEffect, useState } from "react";
// import React from "react";
// import { useParams } from "next/navigation";
// import lobbyService from "../../../lobby/services/lobby.services";
// import { APIResponse, LobbyPaginatedQueryResponse } from "../../../lobby/types/lobby.types";
// import LobbiesPage from "./components/LobbiesPage";


// export default function LobbiesPaged() {
//     const params = useParams<{ pageNumber: string }>();
//     const pageNumber = Number(params.pageNumber);
//     const [response, setResponse] = useState<APIResponse<LobbyPaginatedQueryResponse> | null>(null);
//     const [loading, setLoading] = useState(true);

//   useEffect(() => {
//     async function fetchLobbies() {
//       setLoading(true);
//       const res = await lobbyService.getAll(pageNumber, 6);
//       setResponse(res);
//       setLoading(false);
//     }
//     fetchLobbies();
//   }, [pageNumber]);

//   if (loading) return <div>Loading...</div>;

//   return <LobbiesPage page={pageNumber} response={response!} />;
// }
