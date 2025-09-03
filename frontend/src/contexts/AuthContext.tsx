// src/context/AuthContext.tsx
import React, { createContext, useState, useContext, useEffect } from "react";
import axios from "axios";
import { AuthStore } from "./AuthStore";

type AuthContextType = {
  accessToken: string | null;
  csrfToken: string | null;
  role: string | null;
  setAuth: (data: { accessToken: string; csrfToken: string; role: string }) => void;
  clearAuth: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [csrfToken, setCsrfToken] = useState<string | null>(null);
  const [role, setRole] = useState<string | null>(null);

  const setAuth = ({ accessToken, csrfToken, role }: { accessToken: string; csrfToken: string; role: string }) => {
    setAccessToken(accessToken);
    setCsrfToken(csrfToken);
    setRole(role);

    // đồng bộ ra ngoài (axiosClient hoặc store khác)
    AuthStore.setAuth({ accessToken, csrfToken, role });
  };

  const clearAuth = () => {
    setAccessToken(null);
    setCsrfToken(null);
    setRole(null);

    AuthStore.clearAuth();
  };

  // 🔑 Khi reload -> gọi /Auth/refresh để lấy token mới
  useEffect(() => {
    const tryRefresh = async () => {
      try {
        console.log("🔄 Đang thử refresh token...");

        const res = await axios.post(
          "http://localhost:5039/api/Auth/refresh",
          {},
          {
            
            withCredentials: true, // để gửi cookie refresh token
          }
        );

        const { accessToken } = res.data;

        setAuth({
          accessToken,
        });

        

        console.log("✅ Refresh thành công, accessToken mới:", accessToken);
      } catch (err) {
        console.warn("❌ Refresh thất bại, buộc logout:", err);
        clearAuth();
      }
    };

    tryRefresh();
  }, []); // chỉ chạy khi app load lần đầu

  return (
    <AuthContext.Provider value={{ accessToken, csrfToken, role, setAuth, clearAuth }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
};
