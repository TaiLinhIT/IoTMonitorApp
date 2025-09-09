import React, { useState, useEffect } from "react";
import axios from "axios";
import { AuthStore } from "./AuthStore";
import { setAuthUpdateHandler } from "../services/axiosPrivate";
import { AuthContext } from "./AuthContextBase";

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [isInitializing, setIsInitializing] = useState(true);

  const setAuth = ({ accessToken }: { accessToken: string }) => {
    // cập nhật AuthStore sync trước
    AuthStore.setAuth({ accessToken });
    // rồi mới update state
    setAccessToken(accessToken);
  };

  const clearAuth = () => {
    AuthStore.clearAuth();
    setAccessToken(null);
  };

  // 🔄 Khi app load lại, thử refresh
  useEffect(() => {
    const tryRefresh = async () => {
      try {
        const res = await axios.post(
          "http://localhost:5039/api/Auth/refresh",
          {},
          { withCredentials: true }
        );
        const { accessToken } = res.data;

        // 🟢 update store & state
        setAuth({ accessToken });

        // 🟢 đồng bộ ngược lại qua callback (nếu đã set)
        // trick: gọi lại chính callback mình đã set
        setAuthUpdateHandler((token: string) => {
          if (!token || typeof token !== "string") {
            clearAuth();
            return;
          }
          console.log("nó là cái này", token);
          setAuth({ accessToken: token });
        });
      } catch (err) {
        console.error("[AuthProvider] ❌ Refresh thất bại", err);
        clearAuth();
      } finally {
        setIsInitializing(false);
      }
    };
    tryRefresh();
  }, []);

  // 🟢 đăng ký callback sync từ axiosPrivate → AuthContext
  useEffect(() => {
    setAuthUpdateHandler((token: string) => {
      if (!token || typeof token !== "string") {
        clearAuth();
        return;
      }
      console.log("nó là cái này", token);
      setAuth({ accessToken: token });
    });
  }, []);

  if (isInitializing) {
    return <div>Đang khởi tạo...</div>; // block UI khi chưa refresh xong
  }

  return (
    <AuthContext.Provider
      value={{ accessToken, setAuth, clearAuth, isInitializing }}
    >
      {children}
    </AuthContext.Provider>
  );
};
