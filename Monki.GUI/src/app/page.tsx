import Link from "next/link";
import Navbar from "@/components/Navbar";

export default function Home() {
	return (
		<div className="min-h-screen bg-black text-white">
			<Navbar />
			<main className="mx-auto max-w-5xl px-4 py-10 grid gap-6">
				<h1 className="text-3xl font-semibold">Monki Cards</h1>
				<p className="text-white/70">Minimal flashcards with a modern dark UI.</p>
				<div className="grid sm:grid-cols-2 gap-4">
					<Link href="/decks?tab=public" className="rounded-xl border border-white/10 bg-white/[0.04] hover:bg-white/[0.07] transition-colors p-6">
						<span className="text-lg font-medium">Browse Public Decks →</span>
						<p className="text-white/70 text-sm mt-1">Explore community shared decks.</p>
					</Link>
					<Link href="/decks" className="rounded-xl border border-white/10 bg-white/[0.04] hover:bg-white/[0.07] transition-colors p-6">
						<span className="text-lg font-medium">My Decks →</span>
						<p className="text-white/70 text-sm mt-1">View and manage your decks.</p>
					</Link>
					<Link href="/login" className="rounded-xl border border-white/10 bg-white/[0.04] hover:bg-white/[0.07] transition-colors p-6">
						<span className="text-lg font-medium">Login →</span>
						<p className="text-white/70 text-sm mt-1">Access your account.</p>
					</Link>
					<Link href="/register" className="rounded-xl border border-white/10 bg-white/[0.04] hover:bg-white/[0.07] transition-colors p-6">
						<span className="text-lg font-medium">Register →</span>
						<p className="text-white/70 text-sm mt-1">Create a new account.</p>
					</Link>
				</div>
			</main>
		</div>
	);
}
