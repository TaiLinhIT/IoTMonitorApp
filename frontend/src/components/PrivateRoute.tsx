// src/components/PrivateRoute.tsx
import React from "react";
import { Navigate } from "react-router-dom";
import { AuthStore } from "../contexts/AuthStore";

const PrivateRoute = ({ children }: { children: React.ReactNode }) => {
  const isAuthenticated = AuthStore.getAccessToken() !== null;

  if (!isAuthenticated) {
    // Nếu chưa đăng nhập → về trang login
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>
};

export default PrivateRoute;
