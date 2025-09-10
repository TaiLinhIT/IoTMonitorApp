import { Navigate } from "react-router-dom";
import { PATHS } from "./paths";
import { ROLES } from "../constants/roles";

import Home from "../pages/home/HomePage";
import Dashboard from "../pages/dashboard/DashboardPage";
import ProductManager from "../pages/admin/product/ProductManager";
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

import MainLayout from "../layouts/MainLayout";
import AdminLayout from "../layouts/AdminLayout";

export const routes = [
  { path: "/", element: <Navigate to={PATHS.home} replace /> },

  // Auth
  { path: PATHS.login, element: <Login /> },
  { path: PATHS.register, element: <Register /> },

  // User
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
      },
      {
        path: PATHS.products,
        element: (
          <ErrorBoundary>
            <ProductList />
          </ErrorBoundary>
        ),
      },
      {
        path: PATHS.productDetail,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <RoleRoute allowedRoles={[ROLES.USER, ROLES.ADMIN, ROLES.STAFF]}>
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
              <RoleRoute allowedRoles={[ROLES.USER]}>
                <CartPage />
              </RoleRoute>
            </PrivateRoute>
          </ErrorBoundary>
        ),
      },
      {
        path: PATHS.checkout,
        element: (
          <ErrorBoundary>
            <PrivateRoute>
              <RoleRoute allowedRoles={[ROLES.USER]}>
                <Checkout />
              </RoleRoute>
            </PrivateRoute>
          </ErrorBoundary>
        ),
      },
    ],
  },

  // Admin & Staff dashboard
  {
    path: "/admin",
    element: (
      <PrivateRoute>
        {/* <RoleRoute allowedRoles={[ROLES.ADMIN, ROLES.STAFF]}> */}
          <AdminLayout />
        {/* </RoleRoute> */}
      </PrivateRoute>
    ),
    children: [
      { path: "dashboard", element: <Dashboard /> },
      { path: "products", element: <ProductManager /> },
      { path: "users", element: <div>Quản lý User</div> },
      { path: "reports", element: <div>Báo cáo</div> },
    ],
  }
  ,
  {
    path: PATHS.admin.users,
    element: (
      <PrivateRoute>
        <RoleRoute allowedRoles={[ROLES.ADMIN]}>
          <AdminLayout>
            <div>Quản lý User</div>
          </AdminLayout>
        </RoleRoute>
      </PrivateRoute>
    ),
  },
  {
    path: PATHS.admin.products,
    element: (
      <PrivateRoute>
        {/* <RoleRoute allowedRoles={[ROLES.ADMIN, ROLES.STAFF]}> */}
          <AdminLayout>
            <ProductManager />
          </AdminLayout>
        {/* </RoleRoute> */}
      </PrivateRoute>
    ),
  },
  {
    path: PATHS.admin.reports,
    element: (
      <PrivateRoute>
        {/* <RoleRoute allowedRoles={[ROLES.ADMIN]}> */}
          <AdminLayout>
            <div>Báo cáo</div>
          </AdminLayout>
        {/* </RoleRoute> */}
      </PrivateRoute>
    ),
  },
  

  // Error routes
  { path: "/403", element: <Forbidden /> },
  { path: "*", element: <PageNotFound /> },
];
