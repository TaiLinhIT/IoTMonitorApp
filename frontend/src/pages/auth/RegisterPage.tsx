// src/Features/Auth/pages/Register.tsx
import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import authApi from "../../services/AuthApi";

const Register = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [fullName, setFullName] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage("");
    setIsLoading(true);

    try {
      await authApi.register({
        email,
        fullName,
        password,
      });
      alert("Đăng ký thành công!");
      navigate("/login");
    } catch (error: any) {
      const msg =
        error.response?.data?.message || "Đăng ký thất bại. Vui lòng thử lại.";
      setErrorMessage(msg);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="flex w-full max-w-5xl shadow-lg bg-white rounded-lg overflow-hidden">
        {/* Left - Form */}
        <div className="w-1/2 p-10">
          <h2 className="text-3xl font-bold">Sign up</h2>
          <p className="text-sm mt-2">
            Already have an account?{" "}
            <Link
              to="/login"
              className="text-blue-600 font-medium hover:underline"
            >
              Sign in
            </Link>
          </p>

          <form onSubmit={handleSubmit} className="mt-4">
            {errorMessage && (
              <div className="text-red-600 font-medium mb-4">
                {errorMessage}
              </div>
            )}

            <input
              type="text"
              placeholder="Full name"
              className="w-full px-4 py-2 border rounded mb-4"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              required
            />

            <input
              type="email"
              placeholder="Email"
              className="w-full px-4 py-2 border rounded mb-4"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />

            <input
              type="password"
              placeholder="Password"
              className="w-full px-4 py-2 border rounded mb-4"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            <button
              type="submit"
              className="bg-green-700 text-white w-full py-2 rounded hover:bg-green-800 transition mb-6 disabled:opacity-50"
              disabled={isLoading}
            >
              {isLoading ? "Đang đăng ký..." : "Sign up"}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Register;
