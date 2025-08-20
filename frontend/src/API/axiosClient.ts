// src/API/axiosClient.ts
import axios from "axios";

const axiosClient = axios.create({
  baseURL: "https://localhost:7177/api", // backend API
  headers: {
    "Content-Type": "application/json",
  },
  // ❌ bỏ withCredentials nếu bạn không dùng cookie (chỉ xài Bearer token)
  // ✅ giữ lại nếu backend của bạn xài cookie session song song
  // withCredentials: true, 
});

// 🛠 Interceptor Request: Thêm token vào header
axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("accessToken");
    if (token) {
      config.headers = config.headers ?? {};
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
    if (!error.response) {
      console.error("🌐 Network or CORS error:", error);
      alert("Không thể kết nối tới server. Kiểm tra backend hoặc CORS.");
    } else if (error.response.status === 401) {
      console.warn("🚫 Token hết hạn hoặc chưa đăng nhập");
      localStorage.removeItem("accessToken");
      // 👉 chỉ redirect nếu bạn chắc chắn đang ở flow login
      window.location.href = "/login"; 
    } else if (error.response.status === 403) {
      alert("Bạn không có quyền truy cập.");
    } else {
      console.error("❌ API Error:", error.response);
    }
    return Promise.reject(error);
  }
);

export default axiosClient;
