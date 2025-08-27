// api/axiosPrivate.ts
import axios from "axios";

// Instance chính cho request bình thường
const privateApi = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  // Không cần gửi cookie cho request bình thường
  withCredentials: false,
});

// Instance riêng cho refresh token (cần cookie)
const axiosBase = axios.create({
  baseURL: "http://localhost:5039/api",
  headers: { "Content-Type": "application/json" },
  withCredentials: true, // gửi kèm cookie chứa refresh token
});

let isRefreshing = false;//đánh dấu đang thực hiện refresh token hay chưa
let subscribers: ((token: string) => void)[] = []; // mảng chứa các request chờ token mới. Khi token mới về sẽ retry lại

// Khi token mới về, retry tất cả request đang chờ
// Đây là hàm gọi và thực thi các request đang được chờ sẽ thực hiện sau khi có access token mới
function onRefreshed(token: string) {
  subscribers.forEach((cb) => cb(token));
  subscribers = [];
}

// Thêm token và CSRF vào request
// Interceptor này chạy trước khi gửi request đến server
privateApi.interceptors.request.use((config) => {
  // Lấy accessToken và csrfToken từ sessionStorage
  const token = sessionStorage.getItem("accessToken");
  const csrfToken = sessionStorage.getItem("csrfToken");
  // Nếu có token thì thêm vào header Authorization
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  // Nếu có csrfToken thì thêm vào header X-CSRF-Token
  if (csrfToken) {
    config.headers["X-CSRF-Token"] = csrfToken;
  }

  // Trả về config để Axios gửi request
  return config;
});

// Xử lý response, tự động refresh token khi 401
privateApi.interceptors.response.use(
  (res) => res,// Nếu thành công thì trả về luôn
  // Nếu thấy 401 (Unauthorized) thì thực hiện refresh token
  async (error) => {
    // Lấy request gốc đã gửi
    const originalRequest: any = error.config;
    // Nếu chưa thực hiện refresh và response trả về 401

    if (error.response?.status === 401 && !originalRequest._retry) {
      // Nếu đang trong quá trình refresh token thì không lại thực hiện refresh nữa
      if (isRefreshing) {

        return new Promise((resolve) => {
          subscribers.push((token: string) => {
            // Thêm token mới vào header Authorization
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
        window.location.href = "/login";
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default privateApi;
