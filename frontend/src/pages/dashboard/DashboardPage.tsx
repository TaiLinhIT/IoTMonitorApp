import {
  ShoppingBag,
  Users,
  DollarSign,
  BarChart3,
} from "lucide-react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

// Dữ liệu mẫu doanh thu theo tháng
const revenueData = [
  { month: "Jan", revenue: 1200 },
  { month: "Feb", revenue: 2100 },
  { month: "Mar", revenue: 800 },
  { month: "Apr", revenue: 1600 },
  { month: "May", revenue: 2500 },
  { month: "Jun", revenue: 2000 },
  { month: "Jul", revenue: 3100 },
];

const stats = [
  {
    label: "Sản phẩm",
    value: 120,
    icon: ShoppingBag,
    color: "bg-blue-100 text-blue-600",
  },
  {
    label: "Người dùng",
    value: 58,
    icon: Users,
    color: "bg-green-100 text-green-600",
  },
  {
    label: "Doanh thu",
    value: "$12,500",
    icon: DollarSign,
    color: "bg-yellow-100 text-yellow-600",
  },
  {
    label: "Đơn hàng",
    value: 230,
    icon: BarChart3,
    color: "bg-purple-100 text-purple-600",
  },
];

const DashboardPage = () => {
  return (
    <div>
      {/* Tiêu đề */}
      <h1 className="text-2xl font-bold text-gray-700 mb-6">Dashboard</h1>

      {/* Cards thống kê */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat) => {
          const Icon = stat.icon;
          return (
            <div
              key={stat.label}
              className="bg-white p-6 rounded-xl shadow hover:shadow-md transition"
            >
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-gray-500">{stat.label}</p>
                  <h2 className="text-xl font-semibold text-gray-800">
                    {stat.value}
                  </h2>
                </div>
                <div className={`p-3 rounded-full ${stat.color}`}>
                  <Icon size={20} />
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {/* Biểu đồ doanh thu */}
      <div className="mt-8 bg-white p-6 rounded-xl shadow">
        <h2 className="text-lg font-semibold text-gray-700 mb-4">
          Thống kê doanh thu
        </h2>
        <div className="h-72">
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={revenueData}>
              <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
              <XAxis dataKey="month" stroke="#6b7280" />
              <YAxis stroke="#6b7280" />
              <Tooltip />
              <Line
                type="monotone"
                dataKey="revenue"
                stroke="#3b82f6"
                strokeWidth={2}
                dot={{ r: 4 }}
                activeDot={{ r: 6 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
