// src/routes/index.tsx
import { Navigate } from "react-router-dom";
import { PATHS } from "./paths";
import Home from "../pages/home/HomePage";
import Dashboard from "../pages/dashboard/DashboardPage";
import Login from "../pages/auth/LoginPage";
import Register from "../pages/auth/RegisterPage";
import ProductList from "../pages/product/ProductListPage";
import ProductDetail from "../pages/product/ProductDetailPage";
import Cart from "../pages/cart/CartPage";
// import {PageNotFound} from "../pages/error/PageNotFound";
import Forbidden from "../pages/error/ForbiddenPage";
import PrivateRoute from "../components/PrivateRoute";
import RoleRoute from "../components/RoleRoute";
import ErrorBoundary from "../components/ErrorBoundary";
// src/routes/index.tsx


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
  {
    path:PATHS.carts,
    element:(
      <ErrorBoundary>
        <PrivateRoute>
          <RoleRoute allowedRoles={["Admin", "User"]}>
            <Cart />
          </RoleRoute>
        </PrivateRoute>
      </ErrorBoundary>
    )
  }
  ,
  { path: "*", element: <PageNotFound /> },
];
