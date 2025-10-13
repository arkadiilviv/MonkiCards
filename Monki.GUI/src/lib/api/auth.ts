"use client";

const TOKEN_KEY = "monki_token";

export function setToken(token: string | undefined) {
	if (typeof window === "undefined") return;
	if (!token) {
		localStorage.removeItem(TOKEN_KEY);
		return;
	}
	localStorage.setItem(TOKEN_KEY, token);
}

export function getToken(): string | undefined {
	if (typeof window === "undefined") return undefined;
	const t = localStorage.getItem(TOKEN_KEY);
	return t ?? undefined;
}

export function clearToken() {
	if (typeof window === "undefined") return;
	localStorage.removeItem(TOKEN_KEY);
}


