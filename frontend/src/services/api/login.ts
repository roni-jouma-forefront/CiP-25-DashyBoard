import type { LoginRequest, LoginResponse } from "../../types/auth.types";

const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

export async function login(data: LoginRequest): Promise<LoginResponse> {
  const res = await fetch(`${apiUrl}/api/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });

  if (res.status === 401) {
    throw new Error("Ogiltiga inloggningsuppgifter");
  }

  if (!res.ok) {
    throw new Error("Något gick fel, försök igen");
  }

  return res.json();
}
