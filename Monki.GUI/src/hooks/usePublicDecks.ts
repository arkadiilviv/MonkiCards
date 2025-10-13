"use client";

import { useEffect, useState } from "react";
import { apiClient } from "@/lib/api/client";
import type { components } from "@/api/types";

export type PublicDeck = components["schemas"]["DeckDTO"];

interface UsePublicDecksResult {
	decks: PublicDeck[];
	isLoading: boolean;
	error?: string;
	refresh: () => void;
}

export function usePublicDecks(): UsePublicDecksResult {
	const [decks, setDecks] = useState<PublicDeck[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | undefined>(undefined);
	const [nonce, setNonce] = useState<number>(0);

	useEffect(() => {
		let isActive = true;
		setIsLoading(true);
		setError(undefined);
		apiClient
			.get<PublicDeck[]>("/api/Decks/GetDeckPublic")
			.then((data) => {
				if (!isActive) return;
				setDecks(Array.isArray(data) ? data : []);
			})
			.catch((e: unknown) => {
				if (!isActive) return;
				setError(e instanceof Error ? e.message : "Failed to load public decks");
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


