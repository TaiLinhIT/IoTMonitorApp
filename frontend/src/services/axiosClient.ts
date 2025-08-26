// api/axiosPrivate.ts
import axios from "axios";

const privateApi = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  withCredentials: true, // gửi kèm cookie (nếu có)
});

// instance riêng cho refresh
const axiosBase = axios.create({
  baseURL: "http://localhost:5039/api",
  withCredentials: true,
});

let isRefreshing = false;
let subscribers: ((token: string) => void)[] = [];

function onRefreshed(token: string) {
  subscribers.forEach((cb) => cb(token));
  subscribers = [];
}

privateApi.interceptors.request.use((config) => {
  const token = sessionStorage.getItem("accessToken");
  const csrfToken = sessionStorage.getItem("csrfToken");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  if (csrfToken) {
    config.headers["X-CSRF-Token"] = csrfToken;
  }

  return config;
});

privateApi.interceptors.response.use(
  (res) => res, // giữ nguyên full response
  async (error) => {
    const originalRequest: any = error.config;

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
        const csrfToken = sessionStorage.getItem("csrfToken");
        const res = await axiosBase.post(
          "/auth/refresh",
          {},
          { headers: csrfToken ? { "X-CSRF-Token": csrfToken } : {} }
        );

        const newToken = res.data.accessToken;
        sessionStorage.setItem("accessToken", newToken);

        if (res.data.csrfToken) {
          sessionStorage.setItem("csrfToken", res.data.csrfToken);
        }

        onRefreshed(newToken);
        return privateApi(originalRequest);
      } catch (err) {
        // có thể gọi navigate("/login") nếu dùng react-router
        window.location.href = "/login";
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default privateApi;
