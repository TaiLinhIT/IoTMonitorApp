import { Outlet, Link } from "react-router-dom";
import { LayoutDashboard, ShoppingBag, Users, LogOut } from "lucide-react";

const AdminLayout = () => {
  return (
    <div className="flex min-h-screen bg-gray-100">
      {/* Sidebar */}
      <aside className="w-64 bg-white shadow-md flex flex-col">
        <div className="px-6 py-4 border-b">
          <h1 className="text-xl font-bold text-blue-600">Admin Panel</h1>
        </div>
        <nav className="flex-1 px-4 py-6">
          <ul className="space-y-3">
            <li>
              <Link
                to="/dashboard"
                className="flex items-center gap-2 p-2 rounded hover:bg-blue-50 text-gray-700"
              >
                <LayoutDashboard size={18} />
                Dashboard
              </Link>
            </li>
            <li>
              <Link
                to="/products"
                className="flex items-center gap-2 p-2 rounded hover:bg-blue-50 text-gray-700"
              >
                <ShoppingBag size={18} />
                Quản lý sản phẩm
              </Link>
            </li>
            <li>
              <Link
                to="/users"
                className="flex items-center gap-2 p-2 rounded hover:bg-blue-50 text-gray-700"
              >
                <Users size={18} />
                Quản lý người dùng
              </Link>
            </li>
          </ul>
        </nav>
        <div className="p-4 border-t">
          <button className="flex items-center gap-2 text-red-600 hover:text-red-800">
            <LogOut size={18} />
            Đăng xuất
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {/* Header */}
        <header className="bg-white shadow px-6 py-3 flex justify-between items-center">
          <h2 className="text-lg font-semibold text-gray-700">
            Trang quản trị
          </h2>
          <div className="flex items-center gap-4">
            <span className="text-gray-600">Xin chào, Admin</span>
            <img
              src="https://i.pravatar.cc/40"
              alt="avatar"
              className="w-9 h-9 rounded-full border"
            />
          </div>
        </header>

        {/* Nội dung route con */}
        <main className="flex-1 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default AdminLayout;
