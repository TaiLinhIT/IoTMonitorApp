// src/API/axiosClient.ts
import axios from "axios";

const axiosClient = axios.create({
  baseURL: "https://localhost:7177/api",
  headers: {
    "Content-Type": "application/json",
  },
});

// 🛠 Interceptor Request: Thêm token vào header
axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("accessToken"); // dùng 1 key thống nhất
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// 🛠 Interceptor Response: Lấy data trực tiếp + xử lý lỗi
axiosClient.interceptors.response.use(
  (response) => response.data,
  (error) => {
    if (error.response && error.response.status === 401) {
      localStorage.removeItem("accessToken");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export default axiosClient; // ✅ Xuất mặc định để import dùng default
