// src/API/authApi.ts
import publicApi from "./axiosPublic";
import privateApi from "./axiosPrivate";

const authApi = {
  // 🔹 Public endpoints (chưa login)
  login: (email: string, password: string) =>
    publicApi.post("/Auth/login", { email, password }),

  register: (dto: { email: string; fullName: string; password: string }) =>
    publicApi.post("/Auth/register", dto),

  loginGoogle: (idToken: string) =>
    publicApi.post("/Auth/login-google", { idToken }),

  // 🔹 Private endpoints (đã login)
  refresh: () =>
    privateApi.post("/Auth/refresh", {}),

  logout: () =>
    privateApi.post("/Auth/logout", {}),
};

export default authApi;
