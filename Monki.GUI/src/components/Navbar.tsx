"use client";

import Link from "next/link";

export default function Navbar() {
	return (
		<header className="w-full sticky top-0 z-50 border-b border-white/10 bg-black/40 backdrop-blur-md">
			<div className="mx-auto max-w-5xl px-4 h-14 flex items-center justify-between">
				<Link href="/" className="text-lg font-semibold text-white">
					Monki
				</Link>
				<nav className="flex items-center gap-4 text-sm text-white/80">
					<Link href="/decks" className="hover:text-white transition-colors">
						Decks
					</Link>
					<Link href="/review" className="hover:text-white transition-colors">
						Review
					</Link>
				</nav>
			</div>
		</header>
	);
}


