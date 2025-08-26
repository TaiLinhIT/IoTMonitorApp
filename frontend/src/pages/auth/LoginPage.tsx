// src/Features/Auth/pages/Login.tsx
import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import authApi from "../../services/AuthApi";
import { GoogleLogin, useGoogleLogin } from "@react-oauth/google";
import axiosClient from "../../services/axiosClient";



const Login = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  // ✅ Login bằng email/password
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await authApi.login(email, password);
      const data = response.data; // đây mới là payload JSON

      localStorage.setItem("accessToken", data.accessToken);
      localStorage.setItem("csrfToken", data.csrfToken);
      localStorage.setItem("userRole", data.Role);


      navigate("/dashboard"); // chuyển hướng đến trang sản phẩm sau khi đăng nhập thành công
    } catch (err: any) {
      setError(err.response?.data?.message || "Login failed");
    }
  };

  // ✅ Login bằng Google OAuth
  const handleGoogleSuccess = async (credentialResponse: any) => {
    const idToken = credentialResponse.credential;
    if (!idToken) return;

    try {
      const res = await authApi.loginGoogle(idToken); // gọi API qua authApi
      console.log("Google login response:", res);
      localStorage.setItem("accessToken", res.token);
      localStorage.setItem("userRole",res.Role);
      

      navigate("/products");
    } catch (err) {
      console.error("Google login error:", err);
      setError("Google login failed");
    }
  };

  const handleGoogleError = () => {
    setError("Google login was cancelled or failed");
  };

  const login = useGoogleLogin({
    onSuccess: (response) => {
      localStorage.setItem("accessToken", response?.access_token || "");
      navigate(PATHS.dashboard);
    },
  });

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded shadow-md w-80"
      >
        <h2 className="text-2xl font-bold mb-4 text-center">Log In</h2>
        {error && <p className="text-red-500 mb-2">{error}</p>}

        {/* Email + Password */}
        <input
          type="email"
          placeholder="Email"
          className="w-full p-2 border mb-4 rounded"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Password"
          className="w-full p-2 border mb-4 rounded"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        <button
          type="submit"
          className="bg-blue-600 text-white w-full p-2 rounded hover:bg-blue-700 mb-3"
        >
          Log In
        </button>

        <GoogleLogin
          onSuccess={handleGoogleSuccess}
          onError={() => setError("Google login failed")}
          logo_alignment="center"
          size="large"
        />


        <p className="text-sm text-center mt-4">
          Don't have an account?{" "}
          <Link to="/register" className="text-blue-600 underline">
            Register
          </Link>
        </p>
      </form>
    </div>
  );
};

export default Login;
