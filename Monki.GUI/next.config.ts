import type { NextConfig } from "next";

const nextConfig: NextConfig = {
	rewrites: async () => {
		return [
			{
				source: "/api/:path*",
				destination: "http://localhost:5125/api/:path*",
			},
		];
	},
};

export default nextConfig;
