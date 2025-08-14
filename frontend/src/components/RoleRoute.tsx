// src/components/RoleRoute.tsx
import React from "react";
import { Navigate } from "react-router-dom";

interface RoleRouteProps {
  children: JSX.Element;
  allowedRoles: string[];
}

const RoleRoute = ({ children, allowedRoles }: RoleRouteProps) => {
  const role = localStorage.getItem("userRole");

  if (!role || !allowedRoles.includes(role)) {
    return <Navigate to="/403" replace />;
  }

  return children;
};

export default RoleRoute;
