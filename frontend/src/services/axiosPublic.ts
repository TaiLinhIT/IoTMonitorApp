import axios from "axios";

// 🔓 Public API (không gửi cookie)
export const publicApiWithoutCookie = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  withCredentials: false, // 🚫 không gửi cookie
});

// 🔒 Private API (có gửi cookie)
export const publicApiWithCookie = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  withCredentials: true, // ✅ gửi cookie (httponly, refresh token,...)
});
