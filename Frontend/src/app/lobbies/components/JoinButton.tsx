'use client'

import { useRouter } from "next/navigation";


const JoinButton = ({lobbyId}: {lobbyId: string}) => {
    const router = useRouter();

    const handleClick= async () => {
        router.push(`/lobby/${lobbyId}`);
    };

    return(
        <button
            className="absolute bottom-4 right-3 bg-[#E3DE61] hover:bg-[#b1ad50] rounded-md h-[50px] w-[150px] font-semibold text-black"
            onClick={handleClick}
        >
            Join
        </button>
    );
}

export default JoinButton;