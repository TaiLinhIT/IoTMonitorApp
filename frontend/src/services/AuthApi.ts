// src/API/authApi.ts
import axiosClient from "./axiosClient";

const authApi = {
  login: (email: string, password: string) =>
    axiosClient.post("/Auth/login", { email, password }),

  register: (dto: { email: string; fullName: string; password: string }) =>
    axiosClient.post("/Auth/register", dto),

  loginGoogle: (idToken: string) =>
    axiosClient.post("/Auth/login-google", { idToken }),

  refresh: () =>
    axiosClient.post("/Auth/refresh", {}, { withCredentials: true }),

  logout: () =>
    axiosClient.post("/Auth/logout", {}, { withCredentials: true }),
};

export default authApi;
