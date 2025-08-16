// src/routes/index.tsx
import { Navigate } from "react-router-dom";
import { PATHS } from "./paths";
import Home from "../Features/Home/pages/Home";
import Login from "../Features/Auth/pages/Login";
import Register from "../Features/Auth/pages/Register";
import Dashboard from "../Features/Dashboard/pages/Dashboard";
import PageNotFound from "../Features/Common/pages/PageNotFound";
import Forbidden from "../Features/Common/pages/Forbidden";
import PrivateRoute from "../components/PrivateRoute";
import RoleRoute from "../components/RoleRoute";
import ErrorBoundary from "../components/ErrorBoundary";
import ProductList from "../Features/Product/ProductList";
import ProductDetail from "../Features/Product/ProductDetail";


export const routes = [
  { path: "/", element: <Navigate to="/home" replace /> },
  { path: PATHS.login, element: <Login /> },
  { path: PATHS.register, element: <Register /> },
  { path: PATHS.home, element: <Home /> },
  { path: "/403", element: <Forbidden /> },

  {
    path: PATHS.dashboard,
    element: (
      <ErrorBoundary>
        <PrivateRoute>
          <RoleRoute allowedRoles={["Admin"]}>
            <Dashboard />
          </RoleRoute>
        </PrivateRoute>
      </ErrorBoundary>
    ),
  }
  ,
  {
    path:PATHS.productDetail,
    element:(
      <ErrorBoundary>
        <PrivateRoute>
          <RoleRoute allowedRoles={["Admin", "User"]}>
            <ProductDetail />
          </RoleRoute>
        </PrivateRoute>
      </ErrorBoundary>
    )
  }
  ,
  {
    path: PATHS.products,
    element: (
      <ErrorBoundary>
        <PrivateRoute>
          <RoleRoute allowedRoles={["Admin", "User"]}>
            <ProductList />
          </RoleRoute>
        </PrivateRoute>
      </ErrorBoundary>
    ),
  },
  { path: "*", element: <PageNotFound /> },
];
