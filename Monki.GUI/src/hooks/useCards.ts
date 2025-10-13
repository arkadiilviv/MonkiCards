"use client";

import { useEffect, useState } from "react";
import { apiClient } from "@/lib/api/client";
import type { components } from "@/api/types";

export type Card = components["schemas"]["CardDTO"];

interface UseCardsResult {
	cards: Card[];
	isLoading: boolean;
	error?: string;
	refresh: () => void;
}

export function useCards(deckId: number | undefined): UseCardsResult {
	const [cards, setCards] = useState<Card[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | undefined>(undefined);
	const [nonce, setNonce] = useState<number>(0);

	useEffect(() => {
		if (deckId === undefined) return;
		let isActive = true;
		setIsLoading(true);
		setError(undefined);
		apiClient
			.get<Card[]>(`/api/Cards/GetByDeckId?deckId=${encodeURIComponent(deckId)}`)
			.then((data) => {
				if (!isActive) return;
				setCards(Array.isArray(data) ? data : []);
			})
			.catch((e: unknown) => {
				if (!isActive) return;
				setError(e instanceof Error ? e.message : "Failed to load cards");
				setCards([]);
			})
			.finally(() => {
				if (!isActive) return;
				setIsLoading(false);
			});
		return () => {
			isActive = false;
		};
	}, [deckId, nonce]);

	const refresh = () => setNonce((n) => n + 1);

	return { cards, isLoading, error, refresh };
}


