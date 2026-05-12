export interface LoginRequest {
  hotelId: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  admin: {
    id: string;
    firstName: string | null;
    lastName: string | null;
    role: string | null;
    hotelId: string;
  };
}

export async function login(data: LoginRequest): Promise<LoginResponse> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });

  if (!res.ok) {
    throw new Error("Felaktigt Hotel ID eller lösenord");
  }

  return res.json();
}
