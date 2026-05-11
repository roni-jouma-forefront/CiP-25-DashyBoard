import { createContext, useContext, useState } from "react";
import type { AdminDto } from "../types/auth.types";

type AuthState = {
  token: string | null;
  admin: AdminDto | null;
};

type AuthContextType = AuthState & {
  setAuth: (token: string, admin: AdminDto) => void;
  clearAuth: () => void;
  isAuthenticated: boolean;
};

const AuthContext = createContext<AuthContextType | null>(null);

const TOKEN_KEY = "dashyboard_token";
const ADMIN_KEY = "dashyboard_admin";

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [auth, setAuthState] = useState<AuthState>(() => {
    const token = sessionStorage.getItem(TOKEN_KEY);
    const adminRaw = sessionStorage.getItem(ADMIN_KEY);
    return {
      token,
      admin: adminRaw ? (JSON.parse(adminRaw) as AdminDto) : null,
    };
  });

  const setAuth = (token: string, admin: AdminDto) => {
    sessionStorage.setItem(TOKEN_KEY, token);
    sessionStorage.setItem(ADMIN_KEY, JSON.stringify(admin));
    setAuthState({ token, admin });
  };

  const clearAuth = () => {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(ADMIN_KEY);
    setAuthState({ token: null, admin: null });
  };

  return (
    <AuthContext.Provider
      value={{
        ...auth,
        setAuth,
        clearAuth,
        isAuthenticated: !!auth.token,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
};
