"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Navbar from "@/components/Navbar";
import { apiClient } from "@/lib/api/client";
import { setToken } from "@/lib/api/auth";

interface LoginResponse {
	// Backend returns 200 OK without schema; assume token as string for now
	token?: string;
}

export default function LoginPage() {
	const router = useRouter();
	const [userName, setUserName] = useState<string>("");
	const [password, setPassword] = useState<string>("");
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | undefined>(undefined);

	async function onSubmit(e: React.FormEvent) {
		e.preventDefault();
		setIsLoading(true);
		setError(undefined);
		try {
			const res = await apiClient.post<LoginResponse, { userName?: string; password?: string }>(
				"/api/Auth/login",
				{ userName, password }
			);
			if (res && typeof res === "object" && res.token) {
				setToken(res.token);
			}
			// If no token payload, assume cookie-based or header-based session
			router.push("/decks");
		} catch (err) {
			setError(err instanceof Error ? err.message : "Login failed");
		} finally {
			setIsLoading(false);
		}
	}

	return (
		<div className="min-h-screen bg-black text-white">
			<Navbar />
			<main className="mx-auto max-w-md px-4 py-8">
				<h1 className="text-2xl font-semibold mb-6">Login</h1>
				<form onSubmit={onSubmit} className="grid gap-4">
					<label className="grid gap-2">
						<span className="text-sm text-white/80">Username</span>
						<input
							type="text"
							value={userName}
							onChange={(e) => setUserName(e.target.value)}
							className="rounded-lg border border-white/10 bg-white/5 px-3 py-2 outline-none focus:ring-2 focus:ring-white/20"
							required
						/>
					</label>
					<label className="grid gap-2">
						<span className="text-sm text-white/80">Password</span>
						<input
							type="password"
							value={password}
							onChange={(e) => setPassword(e.target.value)}
							className="rounded-lg border border-white/10 bg-white/5 px-3 py-2 outline-none focus:ring-2 focus:ring-white/20"
							required
						/>
					</label>
					{error && <p className="text-red-400 text-sm">{error}</p>}
					<button
						type="submit"
						disabled={isLoading}
						className="h-10 rounded-lg bg-white/90 text-black font-medium hover:bg-white transition-colors disabled:opacity-60"
					>
						{isLoading ? "Signing in…" : "Sign in"}
					</button>
				</form>
			</main>
		</div>
	);
}


