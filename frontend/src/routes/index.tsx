import { Navigate } from "react-router-dom";
import { PATHS } from "./paths";
import Home from "../pages/home/HomePage";
import Dashboard from "../pages/dashboard/DashboardPage";
import Login from "../pages/auth/LoginPage";
import Register from "../pages/auth/RegisterPage";
import ProductList from "../pages/product/ProductListPage";
import ProductDetail from "../pages/product/ProductDetailPage";
import Cart from "../pages/cart/CartPage";
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
  { path: PATHS.login, element: <Login /> },
  { path: PATHS.register, element: <Register /> },

  // User routes (dùng MainLayout)
  {
    element: <MainLayout />,   // 👈 tất cả children sẽ nằm trong layout
    children: [
      { path: PATHS.home, element: <Home /> },
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
        path: PATHS.productDetail,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <RoleRoute allowedRoles={["Admin", "User"]}>
                <ProductDetail />
              </RoleRoute>
            </PrivateRoute>
          </ErrorBoundary>
        ),
      },
      {
        path: PATHS.carts,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <RoleRoute allowedRoles={["Admin", "User"]}>
                <Cart />
              </RoleRoute>
            </PrivateRoute>
          </ErrorBoundary>
        ),
      },
    ],
  },

  // Dashboard (layout khác nếu muốn)
  {
    path: PATHS.dashboard,
    element: (
      <PrivateRoute>
        <RoleRoute allowedRoles={["Admin"]}>
          <AdminLayout />  {/* Layout riêng cho admin */}
        </RoleRoute>
      </PrivateRoute>
    ),
    children: [
      { index: true, element: <Dashboard /> },
      { path: "users", element: <div>Quản lý user</div> },
      // thêm các route quản trị khác ở đây
    ],
  },

  // Error routes
  { path: "/403", element: <Forbidden /> },
  { path: "*", element: <PageNotFound /> },
];
