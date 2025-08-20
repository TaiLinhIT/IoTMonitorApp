// src/components/PrivateRoute.tsx
import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/use-auth";

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
  // const token = localStorage.getItem("accessToken");

  const user = useAuth();

  console.log("user :>>>> ", user);

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default PrivateRoute;
