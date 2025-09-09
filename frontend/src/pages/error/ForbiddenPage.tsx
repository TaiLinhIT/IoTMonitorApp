// src/Features/Common/pages/Forbidden.tsx
import { Link } from "react-router-dom";

const Forbidden = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <h1 className="text-4xl font-bold text-red-600">403 - Forbidden</h1>
      <p className="mt-4">Bạn không có quyền truy cập trang này.</p>
      <Link
        to="/home"
        className="mt-6 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
      >
        Về trang chủ
      </Link>
    </div>
  );
};

export default Forbidden;
