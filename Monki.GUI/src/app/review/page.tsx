"use client";

import Navbar from "@/components/Navbar";
import Flashcard from "@/components/Flashcard";
import { useCards } from "@/hooks/useCards";
import { useState } from "react";

export default function ReviewPage() {
	const [deckId, setDeckId] = useState<number | undefined>(undefined);
	const { cards, isLoading, error } = useCards(deckId);
	const current = cards[0];

	return (
		<div className="min-h-screen bg-black text-white">
			<Navbar />
			<main className="mx-auto max-w-5xl px-4 py-6 grid gap-6">
				<h1 className="text-2xl font-semibold">Review</h1>
				<div className="flex items-center gap-3">
					<input
						type="number"
						placeholder="Enter deck id"
						className="w-40 rounded-lg border border-white/10 bg-white/5 px-3 py-2 outline-none focus:ring-2 focus:ring-white/20"
						value={deckId ?? ""}
						onChange={(e) => setDeckId(e.target.value ? Number(e.target.value) : undefined)}
					/>
				</div>

				{isLoading && <p className="text-white/70">Loading cards…</p>}
				{error && <p className="text-red-400">{error}</p>}
				{!isLoading && !error && current && (
					<div className="mt-2">
						<Flashcard card={current} />
						<p className="mt-3 text-sm text-white/60">Tap the card to flip</p>
					</div>
				)}
				{!isLoading && !error && !current && <p className="text-white/70">No cards to review.</p>}
			</main>
		</div>
	);
}


