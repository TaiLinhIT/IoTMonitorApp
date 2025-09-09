import { createContext } from "react";

export type AuthContextType = {
  accessToken: string | null;
  setAuth: (data: { accessToken: string }) => void;
  clearAuth: () => void;
  isInitializing: boolean;
};

// ✅ file này chỉ export context, không có component
export const AuthContext = createContext<AuthContextType | undefined>(undefined);
