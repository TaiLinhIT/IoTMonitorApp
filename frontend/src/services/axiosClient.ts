// src/API/axiosClient.ts
import axios from "axios";
import { AuthStore } from "../contexts/AuthStore";
import { cloneElement } from "react";
import { col } from "framer-motion/client";

const axiosClient = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  withCredentials: true,
});

let isRefreshing = false;
let subscribers: ((token: string) => void)[] = [];

function onRefreshed(token: string) {
  subscribers.forEach((cb) => cb(token));
  subscribers = [];
}

axiosClient.interceptors.request.use((config: any) => {
  const accessToken = AuthStore.getAccessToken();
  const csrfToken = AuthStore.getCsrfToken();
  

  if (config.requiresAuth) {
    
    if (accessToken) {
      console.log("Attaching access token to request:", accessToken);
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    if (csrfToken) {
      console.log("Attaching CSRF token to request:", csrfToken);
      config.headers["X-CSRF-Token"] = csrfToken;
    }
  }

  return config;
});

axiosClient.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest: any = error.config;

    // Nếu lỗi từ chính refresh → logout luôn, tránh loop
    if (originalRequest?.url?.includes("/Auth/refresh")) {
      return Promise.reject(error);
    }

    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve) => {
          subscribers.push((token: string) => {
            originalRequest.headers.Authorization = `Bearer ${token}`;
            resolve(axiosClient(originalRequest));
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const csrfToken = sessionStorage.getItem("csrfToken");

        // gọi refresh trực tiếp bằng axios gốc
        const res = await axios.post(
          "http://localhost:5039/api/Auth/refresh",
          {},
          {
            headers: csrfToken ? { "X-CSRF-Token": csrfToken } : {},
            withCredentials: true,
          }
        );

        const newToken = res.data.accessToken;
        sessionStorage.setItem("accessToken", newToken);

        if (res.data.csrfToken) {
          sessionStorage.setItem("csrfToken", res.data.csrfToken);
        }

        onRefreshed(newToken);

        // retry lại request gốc
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return axiosClient(originalRequest);
      } catch (err) {
        sessionStorage.removeItem("accessToken");
        sessionStorage.removeItem("csrfToken");
        window.location.href = "/login";
        return Promise.reject(err);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default axiosClient;
