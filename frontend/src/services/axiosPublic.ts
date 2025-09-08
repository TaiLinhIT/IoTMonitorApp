import axios from "axios";
const publicApi = axios.create({
    baseURL: "http://localhost:5039/api",
    headers: { "Content-Type": "application/json" },
    withCredentials:true,
});

export default publicApi;