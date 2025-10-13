"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Navbar from "@/components/Navbar";
import { apiClient } from "@/lib/api/client";
import { setToken } from "@/lib/api/auth";

interface RegisterResponse {
	token?: string;
}

export default function RegisterPage() {
	const router = useRouter();
	const [email, setEmail] = useState<string>("");
	const [userName, setUserName] = useState<string>("");
	const [password, setPassword] = useState<string>("");
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | undefined>(undefined);

	async function onSubmit(e: React.FormEvent) {
		e.preventDefault();
		setIsLoading(true);
		setError(undefined);
		try {
			const res = await apiClient.post<RegisterResponse, { email?: string; userName?: string; password?: string }>(
				"/api/Auth/register",
				{ email, userName, password }
			);
			if (res && typeof res === "object" && res.token) {
				setToken(res.token);
			}
			router.push("/decks");
		} catch (err) {
			setError(err instanceof Error ? err.message : "Register failed");
		} finally {
			setIsLoading(false);
		}
	}

	return (
		<div className="min-h-screen bg-black text-white">
			<Navbar />
			<main className="mx-auto max-w-md px-4 py-8">
				<h1 className="text-2xl font-semibold mb-6">Create account</h1>
				<form onSubmit={onSubmit} className="grid gap-4">
					<label className="grid gap-2">
						<span className="text-sm text-white/80">Email</span>
						<input
							type="email"
							value={email}
							onChange={(e) => setEmail(e.target.value)}
							className="rounded-lg border border-white/10 bg-white/5 px-3 py-2 outline-none focus:ring-2 focus:ring-white/20"
							required
						/>
					</label>
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
						{isLoading ? "Creating…" : "Create account"}
					</button>
				</form>
			</main>
		</div>
	);
}


