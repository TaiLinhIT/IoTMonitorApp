import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import axios from 'axios';

const Register = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [fullName, setFullName] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage('');
    setIsLoading(true);

    try {
      const response = await axios.post('https://localhost:7177/api/auth/register', {
        email,
        fullName,
        password,
      });
      console.log(response.data);
      alert('Đăng ký thành công!');
      navigate('/login');
    } catch (error: any) {
      console.error(error);
      const msg = error.response?.data?.message || 'Đăng ký thất bại. Vui lòng thử lại.';
      setErrorMessage(msg);
    } finally {
      setIsLoading(false);
    }
  };

  const handleGoogleLogin = () => {
    window.location.href = 'https://localhost:7177/api/auth/login-google';
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="flex w-full max-w-5xl shadow-lg bg-white rounded-lg overflow-hidden">
        {/* Left - Form */}
        <div className="w-1/2 p-10">
          <div className="mb-6">
            <img src="/logo.svg" alt="Logo" className="h-8 mb-4" />
            <h2 className="text-3xl font-bold">Sign up</h2>
            <p className="text-sm mt-2">
              Already have an account?{' '}
              <Link to="/login" className="text-blue-600 font-medium hover:underline">
                Sign in
              </Link>
            </p>
          </div>

          <form onSubmit={handleSubmit}>
            {errorMessage && (
              <div className="text-red-600 font-medium mb-4">{errorMessage}</div>
            )}

            <input
              type="text"
              placeholder="Full name"
              className="w-full px-4 py-2 border rounded mb-4"
              value={fullName}
              onChange={e => setFullName(e.target.value)}
              required
            />

            <input
              type="email"
              placeholder="Email"
              className="w-full px-4 py-2 border rounded mb-4"
              value={email}
              onChange={e => setEmail(e.target.value)}
              required
            />

            <input
              type="password"
              placeholder="Password"
              className="w-full px-4 py-2 border rounded mb-4"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
            />

            <label className="flex items-center mb-4">
              <input type="checkbox" className="mr-2" /> Remember me
            </label>

            <button
              type="submit"
              className="bg-green-700 text-white w-full py-2 rounded hover:bg-green-800 transition mb-6 disabled:opacity-50"
              disabled={isLoading}
            >
              {isLoading ? 'Đang đăng ký...' : 'Sign up'}
            </button>

            <div className="text-center text-gray-400 mb-4">────────  or  ────────</div>

            <button
              type="button"
              onClick={handleGoogleLogin}
              className="flex items-center justify-center w-full border rounded py-2 mb-2 hover:bg-gray-100"
            >
              <img src="/google.svg" alt="Google" className="h-5 w-5 mr-2" />
              Continue with Google
            </button>

            <button
              type="button"
              className="flex items-center justify-center w-full border rounded py-2 hover:bg-gray-100"
            >
              <img src="/facebook.svg" alt="Facebook" className="h-5 w-5 mr-2" />
              Continue with Facebook
            </button>
          </form>
        </div>

        {/* Right - Info Panel */}
        <div className="w-1/2 bg-green-900 text-white p-10 flex flex-col justify-between">
          <div className="flex justify-end mb-8">
            <button className="text-sm underline">Support</button>
          </div>

          <div className="space-y-6">
            <div className="bg-white rounded p-4 text-black">
              <h3 className="font-bold mb-1">Reach financial goals faster</h3>
              <p className="text-sm">Use your Venus card with no hidden fees. Hold, transfer and spend.</p>
              <button className="bg-green-700 text-white px-4 py-1 mt-3 rounded text-sm">Learn more</button>
              <div className="mt-4 text-right font-semibold text-green-800">$350.40</div>
            </div>

            <div>
              <h2 className="text-xl font-bold">Introducing new features</h2>
              <p className="text-sm opacity-80 mt-1">
                Analyzing previous trends ensures businesses always make the right decision...
              </p>
            </div>
          </div>

          <div className="flex justify-center mt-10 space-x-2 text-lg">
            <span className="opacity-70">•</span>
            <span className="opacity-70">☾</span>
            <span className="opacity-70">›</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register;
