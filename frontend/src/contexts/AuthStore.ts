// src/context/authStore.ts
let accessToken: string | null = null;
let csrfToken: string | null = null;
let role: string | null = null;

export const AuthStore = {
  getAccessToken: () => accessToken,
  getCsrfToken: () => csrfToken,
  getRole: () => role,
  setAuth: (data: { accessToken: string; csrfToken: string; role: string }) => {
    accessToken = data.accessToken;
    csrfToken = data.csrfToken;
    role = data.role;
    // log ra khi setAuth được gọi
    // console.log("AuthStore updated:", {
    //   data
    // });
  },
  clearAuth: () => {
    accessToken = null;
    csrfToken = null;
    role = null;
  },
};
