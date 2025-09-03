import Link from "next/link";

export default function HomePage() {
  return (
    <div className="h-screen w-full flex items-center justify-center px-6">
      <div className="flex flex-col items-center gap-8">

        {/* Title */}
        <h1 className="text-5xl font-bold text-center">Welcome to Poker</h1>

        {/* Subtitle */}
        <p className="text-lg text-center max-w-xl">
          Play poker online with friends or join public lobbies. Fast, fair, and fun!
        </p>

        {/* Navigation Buttons */}
        <div className="flex flex-col md:flex-row gap-6 mt-4">
          <Link
            href="/login"
            className="bg-[#E3DE61] hover:bg-[#b1ad50] text-black font-semibold py-3 px-6 rounded-md text-center w-48"
          >
            Login
          </Link>
          <Link
            href="/register"
            className="bg-[#E3DE61] hover:bg-[#b1ad50] text-black font-semibold py-3 px-6 rounded-md text-center w-48"
          >
            Register
          </Link>
          <Link
            href="/lobbies"
            className="bg-[#437057] hover:bg-[#385844] text-white font-semibold py-3 px-6 rounded-md text-center w-48 border-2 border-white"
          >
            Lobbies
          </Link>
        </div>
      </div>
    </div>
  );
}
