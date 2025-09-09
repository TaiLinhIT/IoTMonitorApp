import { Navigate } from "react-router-dom";
import { PATHS } from "./paths";
import Home from "../pages/home/HomePage";
import Dashboard from "../pages/dashboard/DashboardPage";
import Login from "../pages/auth/LoginPage";
import Register from "../pages/auth/RegisterPage";
import ProductList from "../pages/product/ProductListPage";
import ProductDetail from "../pages/product/ProductDetailPage";
import CartPage from "../pages/cart/CartPage";
import Checkout from "../pages/checkout/CheckoutPage";
import Forbidden from "../pages/error/ForbiddenPage";
import PageNotFound from "../pages/error/PageNotFoundPage";
import PrivateRoute from "../components/PrivateRoute";
import RoleRoute from "../components/RoleRoute";
import ErrorBoundary from "../components/ErrorBoundary";
import AdminLayout from "../layouts/AdminLayout";

// 🆕 Layout
import MainLayout from "../layouts/MainLayout";

export const routes = [
  { path: "/", element: <Navigate to={PATHS.home} replace /> },

  // Auth routes (không cần layout)
  { path: PATHS.login, element: <Login />, requiresAuth: false },
  { path: PATHS.register, element: <Register />, requiresAuth: false },

  // User routes
  {
    element: <MainLayout />,
    children: [
      {
        path: PATHS.home,
        element: (
          <ErrorBoundary>
            <Home />
          </ErrorBoundary>
        ),
        requiresAuth: false,
      },
      {
        path: PATHS.products,
        element: (
          <ErrorBoundary>
            <ProductList />
          </ErrorBoundary>
        ),
        requiresAuth: false,
      },
      {
        path: PATHS.productDetail,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <ProductDetail />
            </PrivateRoute>
          </ErrorBoundary>
        ),
        requiresAuth: true,
        // requiresCsrf: true, // 👈 nếu bạn muốn bắt buộc CSRF
      },
      {
        path: PATHS.carts,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <RoleRoute allowedRoles={["Admin", "User"]}>
                <CartPage />
              </RoleRoute>
            </PrivateRoute>
          </ErrorBoundary>
        ),
        requiresAuth: true,
        // requiresCsrf: true,
      },
      {
        path: PATHS.checkout,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <RoleRoute allowedRoles={["Admin", "User"]}>
                <Checkout />
              </RoleRoute>
            </PrivateRoute>
          </ErrorBoundary>
        ),
        requiresAuth: true,
        // requiresCsrf: true,
      },
    ],
  },

  // Dashboard (layout khác nếu muốn)
  {
    path: PATHS.dashboard,
    element: (
      <PrivateRoute>
        <RoleRoute allowedRoles={["Admin"]}>
          <AdminLayout />
        </RoleRoute>
      </PrivateRoute>
    ),
    requiresAuth: true,
    children: [
      { index: true, element: <Dashboard />, requiresAuth: true },
      { path: "users", element: <div>Quản lý user</div>, requiresAuth: true },
    ],
  },

  // Error routes
  { path: "/403", element: <Forbidden />, requiresAuth: false },
  { path: "*", element: <PageNotFound />, requiresAuth: false },
];
