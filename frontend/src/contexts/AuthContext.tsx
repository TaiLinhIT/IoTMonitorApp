// src/context/AuthContext.tsx
import React, { createContext, useState, useContext, useEffect } from "react";
import axios from "axios";
import { AuthStore } from "./AuthStore";
import { setAuthUpdateHandler } from "../services/axiosPrivate";

type AuthContextType = {
  accessToken: string | null;
  setAuth: (data: { accessToken: string}) => void;
  clearAuth: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [accessToken, setAccessToken] = useState<string | null>(null);

  const setAuth = ({ accessToken }: { accessToken: string}) => {
    setAccessToken(accessToken);
    AuthStore.setAuth({ accessToken});
  };

  const clearAuth = () => {
    setAccessToken(null);
    AuthStore.clearAuth();
  };

  // ✅ Refresh 1 lần khi app load
  useEffect(() => {
    const tryRefresh = async () => {
      try {
        const res = await axios.post(
          "http://localhost:5039/api/Auth/refresh",
          {},
          { withCredentials: true }
        );

        const { accessToken} = res.data;
        setAuth({ accessToken});
      } catch (err) {
        clearAuth();
      }
    };

    tryRefresh();
  }, []);

  // ✅ Cho interceptor biết cách cập nhật context
  useEffect(() => {
    setAuthUpdateHandler((data) => {
      if (data.accessToken) {
        setAuth(data);
      } else {
        clearAuth();
      }
    });
  }, []);

  return (
    <AuthContext.Provider value={{ accessToken,  setAuth, clearAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
};
