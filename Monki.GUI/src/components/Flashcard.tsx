"use client";

import { useState } from "react";
import { motion } from "framer-motion";
import type { Card } from "@/hooks/useCards";

interface Props {
	card: Card;
}

export default function Flashcard({ card }: Props) {
	const [flipped, setFlipped] = useState<boolean>(false);
	return (
		<div className="perspective-1000 cursor-pointer select-none" onClick={() => setFlipped((f) => !f)}>
			<motion.div
				className="relative w-full h-64 sm:h-72 md:h-80"
				style={{ transformStyle: "preserve-3d" }}
				animate={{ rotateY: flipped ? 180 : 0 }}
				transition={{ duration: 0.6, ease: "easeInOut" }}
			>
				<div className="absolute inset-0 rounded-2xl bg-white/[0.04] border border-white/10 p-6 text-white backface-hidden flex items-center justify-center text-center text-xl">
					{card.sideA ?? ""}
				</div>
				<div className="absolute inset-0 rounded-2xl bg-white/[0.04] border border-white/10 p-6 text-white backface-hidden rotate-y-180 flex items-center justify-center text-center text-xl">
					{card.sideB ?? ""}
				</div>
			</motion.div>
		</div>
	);
}


