// src/routes/paths.ts
export const PATHS = {
  home: "/home",

  // Auth
  login: "/login",
  register: "/register",

  // User
  products: "/products",
  productDetail: "/product/:slug/:id",
  carts: "/carts",
  checkout: "/checkout",

  // Dashboard cho từng role
  admin: {
    dashboard: "/admin/dashboard",
    users: "/admin/users",
    reports: "/admin/reports",
    products: "/admin/products",
  },
  staff: {
    dashboard: "/staff/dashboard",
    orders: "/staff/orders",
    products: "/staff/products",
  },
  user: {
    dashboard: "/user/dashboard", // nếu cần
    profile: "/user/profile",
    orders: "/user/orders",
  },

  // Error
  forbidden: "/403",
  notFound: "*",
};
