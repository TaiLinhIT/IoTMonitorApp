import axios from "axios";
import { AuthStore } from "../contexts/AuthStore";

let isRefreshing = false;
let subscribers: ((token: string) => void)[] = [];

// Callback sync ngược về AuthContext
let authUpdateHandler: ((token: string) => void) | null = null;
export function setAuthUpdateHandler(handler: (token: string) => void) {
  console.log("[axiosPrivate] 🔗 setAuthUpdateHandler registered");
  authUpdateHandler = handler;
}

function onRefreshed(token: string) {
  console.log("[axiosPrivate] 🔔 onRefreshed, retry queued requests with token:", token);
  subscribers.forEach((cb) => cb(token));
  subscribers = [];
}

const privateApi = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" }
  // withCredentials: true,
});

// 🟢 Request interceptor
privateApi.interceptors.request.use((config) => {
  const token = AuthStore.getAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
    console.log("[axiosPrivate] ➡️ Request with token:", config.url);
  } else {
    console.log("[axiosPrivate] ➡️ Request without token (may be queued):", config.url);
  }
  return config;
});

// 🔴 Response interceptor
privateApi.interceptors.response.use(
  (res) => {
    // console.log("[axiosPrivate] ✅ Response:", res.config.url, "Status:", res.status);
    return res;
  },
  async (error) => {
    console.log("1");
    const originalRequest =   error.config;
    console.warn("[axiosPrivate] ⚠️ Response error:", originalRequest?.url, "Status:", error.response?.status);

    if (originalRequest?.url?.includes("/Auth/refresh")) {
      console.log("[axiosPrivate] ❌ Refresh request failed");
      return Promise.reject(error);
    }

    if (error.response?.status === 401 && !originalRequest._retry) {
      console.log("[axiosPrivate] 🔄 401 detected, start token refresh");

      if (isRefreshing) {
        console.log("[axiosPrivate] ⏳ Already refreshing, queue this request");
        return new Promise((resolve) => {
          subscribers.push((token: string) => {
            console.log("[axiosPrivate] 🔔 Retry queued request with new token");
            originalRequest.headers.Authorization = `Bearer ${token}`;
            resolve(privateApi(originalRequest));
          });
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        console.log("[axiosPrivate] 🔄 Calling /Auth/refresh");
        const res = await axios.post(
          "http://localhost:5039/api/Auth/refresh",
          {},
          { withCredentials: true }
        );
        const { accessToken } = res.data;

        console.log("[axiosPrivate] ✅ Refresh success, new accessToken:", accessToken);

        AuthStore.setAuth({ accessToken });
        if (authUpdateHandler) {
          console.log("[axiosPrivate] 🔗 Updating AuthContext with new token");
          authUpdateHandler(accessToken);
        }

        onRefreshed(accessToken);

        // Retry original request
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return privateApi(originalRequest);
      } catch (err) {
        console.error("[axiosPrivate] ❌ Refresh failed, redirecting to /login", err);
        AuthStore.clearAuth();
        window.location.href = "/login";
        return Promise.reject(err);
      } finally {
        isRefreshing = false;
        console.log("[axiosPrivate] 🔚 Refresh process ended");
      }
    }

    if (error.response?.status === 403) {
      console.warn("[axiosPrivate] 🚫 403 Forbidden, redirecting to /forbidden");
      window.location.href = "/forbidden";
    }

    return Promise.reject(error);
  }
);

export default privateApi;
