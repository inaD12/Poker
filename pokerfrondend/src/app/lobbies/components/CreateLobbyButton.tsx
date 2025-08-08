"use client";

import { useState } from "react";
import { getLobbyClient } from "../../../../lobby/services/lobby.client";
import { useRouter } from "next/navigation";

export default function CreateLobbyButton() {
  const [name, setName] = useState("");
  const lobbyClient = getLobbyClient();
  const router = useRouter();

  const handleCreateLobby = async () => {
    if (!name.trim()) return alert("Please enter a lobby name.");
    const lobbyClient = await getLobbyClient();
    var lobbyId = await lobbyClient.createLobby(name);
    router.push(`/lobby/${lobbyId}`);
  };

  return (
    <div className="flex flex-col gap-2 w-full max-w-sm">
      <input
        type="text"
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Enter lobby name"
        className="px-4 py-2 border rounded-md shadow-sm"
      />
      <button
        className="bg-[#E3DE61] hover:bg-[#b1ad50] text-black font-bold px-6 py-3 rounded-md shadow-md"
        onClick={handleCreateLobby}
      >
        Create New
      </button>
    </div>
  );
}
