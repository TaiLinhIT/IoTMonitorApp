import axios from "axios";
const axiosClient = axios.create({
    baseURL: "https://localhost:5000/api",
    headers: {
        "Content-Type":"application/json",
    },
});


//Thêm interceptors nếu cần
axiosClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);


axiosClient.interceptors.response.use(
    (config) => {
        const token = localStorage.getItem("access_token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);
axiosClient.interceptors.response.use(
    (response) => {
        return response.data;
    },
    (error) => {
        if (error.response && error.response.status === 401) {
            localStorage.removeItem("accessToken");
            window.location.href = "/login";
        }
        return Promise.reject(error);
    }
);