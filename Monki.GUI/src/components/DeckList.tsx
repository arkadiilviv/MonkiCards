"use client";

import Link from "next/link";
import type { Deck } from "@/hooks/useDecks";

interface Props {
	decks: Deck[];
	isLoading?: boolean;
	error?: string;
}

export default function DeckList({ decks, isLoading, error }: Props) {
	if (isLoading) {
		return (
			<div className="grid gap-3">
				{Array.from({ length: 4 }).map((_, i) => (
					<div key={i} className="h-20 rounded-xl bg-white/5 animate-pulse" />
				))}
			</div>
		);
	}

	if (error) {
		return <p className="text-red-400">{error}</p>;
	}

	if (!decks?.length) {
		return <p className="text-white/70">No decks yet. Create your first one!</p>;
	}

	return (
		<ul className="grid gap-3">
			{decks.map((d) => (
				<li key={d.id} className="rounded-xl border border-white/10 bg-white/[0.03] hover:bg-white/[0.06] transition-colors">
					<Link href={`/decks/${d.id ?? ""}`} className="block p-4">
						<p className="text-white font-medium">{d.name ?? "Untitled"}</p>
						<p className="text-white/60 text-sm">{d.description ?? ""}</p>
					</Link>
				</li>
			))}
		</ul>
	);
}


