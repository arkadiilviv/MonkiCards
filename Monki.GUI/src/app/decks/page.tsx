"use client";

import Navbar from "@/components/Navbar";
import DeckList from "@/components/DeckList";
import { useDecks } from "@/hooks/useDecks";
import { usePublicDecks } from "@/hooks/usePublicDecks";
import { useSearchParams, useRouter } from "next/navigation";

export default function DecksPage() {
	const router = useRouter();
	const search = useSearchParams();
	const tab = search.get("tab") === "public" ? "public" : "mine";
	const { decks: myDecks, isLoading: mineLoading, error: mineError } = useDecks();
	const { decks: publicDecks, isLoading: publicLoading, error: publicError } = usePublicDecks();

	const isPublic = tab === "public";

	function setTab(next: "mine" | "public") {
		const qs = new URLSearchParams(search.toString());
		qs.set("tab", next);
		router.push(`/decks?${qs.toString()}`);
	}

	return (
		<div className="min-h-screen bg-black text-white">
			<Navbar />
			<main className="mx-auto max-w-5xl px-4 py-6">
				<div className="flex items-center justify-between mb-4">
					<h1 className="text-2xl font-semibold">Decks</h1>
					<div className="inline-flex rounded-lg border border-white/10 bg-white/5 p-1">
						<button
							className={`px-3 py-1.5 rounded-md text-sm ${!isPublic ? "bg-white/15" : "text-white/70"}`}
							onClick={() => setTab("mine")}
						>
							My Decks
						</button>
						<button
							className={`px-3 py-1.5 rounded-md text-sm ${isPublic ? "bg-white/15" : "text-white/70"}`}
							onClick={() => setTab("public")}
						>
							Public decks
						</button>
					</div>
				</div>

				{!isPublic ? (
					<DeckList decks={myDecks} isLoading={mineLoading} error={mineError} />
				) : (
					<DeckList decks={publicDecks} isLoading={publicLoading} error={publicError} />
				)}
			</main>
		</div>
	);
}


