"use client";

import { useEffect, useState } from "react";
import { apiClient } from "@/lib/api/client";
import type { components } from "@/api/types";

export type Deck = components["schemas"]["DeckDTO"];

interface UseDecksResult {
	decks: Deck[];
	isLoading: boolean;
	error?: string;
	refresh: () => void;
}

export function useDecks(): UseDecksResult {
	const [decks, setDecks] = useState<Deck[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | undefined>(undefined);
	const [nonce, setNonce] = useState<number>(0);

	useEffect(() => {
		let isActive = true;
		setIsLoading(true);
		setError(undefined);
		apiClient
			.get<Deck[]>("/api/Decks/GetDecks")
			.then((data) => {
				if (!isActive) return;
				setDecks(Array.isArray(data) ? data : []);
			})
			.catch((e: unknown) => {
				if (!isActive) return;
				setError(e instanceof Error ? e.message : "Failed to load decks");
				setDecks([]);
			})
			.finally(() => {
				if (!isActive) return;
				setIsLoading(false);
			});
		return () => {
			isActive = false;
		};
	}, [nonce]);

	const refresh = () => setNonce((n) => n + 1);

	return { decks, isLoading, error, refresh };
}


