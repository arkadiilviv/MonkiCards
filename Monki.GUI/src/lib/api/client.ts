import type { paths } from "@/api/types";
import { getToken } from "./auth";

type HttpMethod = "get" | "post" | "put" | "delete";

export interface ApiClientOptions {
	baseUrl?: string;
	getToken?: () => string | undefined;
}

const defaultBaseUrl = process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5125";

export class ApiClient {
	private readonly baseUrl: string;
	private readonly getToken?: () => string | undefined;

	constructor(options?: ApiClientOptions) {
		this.baseUrl = options?.baseUrl ?? defaultBaseUrl;
		this.getToken = options?.getToken;
	}

	private buildHeaders(init?: HeadersInit): HeadersInit {
		const headers = new Headers(init);
		if (!headers.has("Content-Type")) headers.set("Content-Type", "application/json");
		const token = this.getToken?.();
		if (token) headers.set("Authorization", `Bearer ${token}`);
		return headers;
	}

	private async request<T>(path: string, method: HttpMethod, body?: unknown, init?: RequestInit): Promise<T> {
		const res = await fetch(`${this.baseUrl}${path}`, {
			method: method.toUpperCase(),
			headers: this.buildHeaders(init?.headers),
			body: body !== undefined ? JSON.stringify(body) : undefined,
			...init,
		});
		if (!res.ok) {
			const text = await res.text().catch(() => "");
			throw new Error(`API ${method.toUpperCase()} ${path} failed: ${res.status} ${text}`);
		}
		// attempt json, fall back to void
		const contentType = res.headers.get("content-type") ?? "";
		if (contentType.includes("application/json")) return (await res.json()) as T;
		return undefined as unknown as T;
	}

	get<T>(path: string, init?: RequestInit) {
		return this.request<T>(path, "get", undefined, init);
	}

	post<T, B = unknown>(path: string, body: B, init?: RequestInit) {
		return this.request<T>(path, "post", body, init);
	}

	put<T, B = unknown>(path: string, body: B, init?: RequestInit) {
		return this.request<T>(path, "put", body, init);
	}

	delete<T>(path: string, init?: RequestInit) {
		return this.request<T>(path, "delete", undefined, init);
	}
}

export const apiClient = new ApiClient({ getToken });

// Re-export important OpenAPI types to use across the app
export type Paths = paths;

