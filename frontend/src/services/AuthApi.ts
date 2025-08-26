// src/API/authApi.ts
import axiosClient from "./axiosClient";

const authApi = {
  //Login thường
  login: (email: string, password: string) =>
    axiosClient.post("/Auth/login", { email, password }),

  //Register
  register: (dto: { email: string; fullName: string; password: string }) =>
    axiosClient.post("/Auth/register", dto),

  //Login Google (SPA flow: nhận idToken từ frontend)
  loginGoogle: (idToken: string) =>
    axiosClient.post("/Auth/login-google", { idToken }),
};

export default authApi;
