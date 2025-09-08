// src/API/axiosPrivate.ts
import axios from "axios";
import { AuthStore } from "../contexts/AuthStore";

let isRefreshing = false;
let subscribers: ((token: string) => void)[] = [];

// Callback sync ngược về AuthContext
let authUpdateHandler: ((accessToken: string) => void) | null = null;
export function setAuthUpdateHandler(handler: (accessToken: string) => void) {
  authUpdateHandler = handler;
}

function onRefreshed(token: string) {
  subscribers.forEach((cb) => cb(token));
  subscribers = [];
}

const privateApi = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  withCredentials: true,
});

// 🟢 Request interceptor
privateApi.interceptors.request.use((config: any) => {
  const token = AuthStore.getAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// 🔴 Response interceptor
privateApi.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest: any = error.config;

    if (originalRequest?.url?.includes("/Auth/refresh")) {
      return Promise.reject(error);
    }

    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve) => {
          subscribers.push((token: string) => {
            originalRequest.headers.Authorization = `Bearer ${token}`;
            resolve(privateApi(originalRequest));
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const res = await axios.post(
          "http://localhost:5039/api/Auth/refresh",
          {},
          { withCredentials: true }
        );

        const { accessToken } = res.data;

        // Lưu vào AuthStore (in-memory)
        AuthStore.setAuth({ accessToken });

        // Sync về React state nếu có handler
        if (authUpdateHandler) {
          authUpdateHandler(accessToken);
        }

        onRefreshed(accessToken);

        // Retry request gốc
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return privateApi(originalRequest);
      } catch (err) {
        AuthStore.clearAuth();
        window.location.href = "/login";
        return Promise.reject(err);
      } finally {
        isRefreshing = false;
      }
    }

    if (error.response?.status === 403) {
      window.location.href = "/forbidden";
    }

    return Promise.reject(error);
  }
);

export default privateApi;
