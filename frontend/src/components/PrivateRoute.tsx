// src/components/PrivateRoute.tsx
import React from "react";
import { Navigate } from "react-router-dom";

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
   const token = localStorage.getItem("accessToken");

  // Kiểm tra xem người dùng đã đăng nhập hay chưa
  if (!token) {
    // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default PrivateRoute;
