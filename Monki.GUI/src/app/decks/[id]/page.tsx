"use client";

import { useParams } from "next/navigation";
import Navbar from "@/components/Navbar";
import { useCards } from "@/hooks/useCards";
import Flashcard from "@/components/Flashcard";

export default function DeckDetailPage() {
	const params = useParams<{ id: string }>();
	const idNum = Number(params?.id);
	const { cards, isLoading, error } = useCards(Number.isFinite(idNum) ? idNum : undefined);

	return (
		<div className="min-h-screen bg-black text-white">
			<Navbar />
			<main className="mx-auto max-w-5xl px-4 py-6 grid gap-6">
				<h1 className="text-2xl font-semibold">Deck {params?.id}</h1>
				{isLoading && <p className="text-white/70">Loading cards…</p>}
				{error && <p className="text-red-400">{error}</p>}
				{!isLoading && !error && cards.length === 0 && (
					<p className="text-white/70">No cards yet.</p>
				)}
				{!isLoading && !error && cards.length > 0 && (
					<div className="grid gap-4">
						{cards.map((c) => (
							<div key={c.id}>
								<Flashcard card={c} />
							</div>
						))}
					</div>
				)}
			</main>
		</div>
	);
}


