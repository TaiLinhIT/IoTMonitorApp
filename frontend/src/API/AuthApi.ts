// src/API/authApi.ts
import axiosClient from "./axiosClient";
//import type { User } from "../Models/User";

const authApi = {
  login: (email: string, password: string) =>
    axiosClient.post("/Auth/login", { email, password }),

  register: (email: string, fullName: string, password: string) =>
    axiosClient.post("/Auth/register", { email, fullName, password }),

  loginGoogle: () => {
    window.location.href = "https://localhost:7177/api/auth/login-google";
  },
};

export default authApi;
