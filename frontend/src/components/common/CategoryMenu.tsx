import { ChevronDown } from "lucide-react";

const categories = [
  { name: "Điện thoại", href: "/category/dien-thoai" },
  { name: "Laptop", href: "/category/laptop" },
  { name: "Máy tính bảng", href: "/category/may-tinh-bang" },
  { name: "PC - Máy tính để bàn", href: "/category/pc" },
  { name: "Màn hình", href: "/category/man-hinh" },
  { name: "Phụ kiện", href: "/category/phu-kien" },
  { name: "Đồng hồ thông minh", href: "/category/dong-ho" },
  { name: "Sim - Thẻ", href: "/category/sim" },
  { name: "Gia dụng", href: "/category/gia-dung" },
];

const CategoryMenu = () => {
  return (
    <div className="bg-white border-b shadow-sm">
      <div className="container mx-auto flex items-center space-x-6 px-4 py-3 overflow-x-auto">
        {/* Nút danh mục chính */}
        <button className="flex items-center space-x-1 font-semibold text-red-600 hover:text-red-700 whitespace-nowrap">
          <span>Danh mục</span>
          <ChevronDown className="h-4 w-4" />
        </button>

        {/* Danh mục nổi bật */}
        {categories.map((cat) => (
          <a
            key={cat.name}
            href={cat.href}
            className="text-sm text-gray-700 hover:text-red-600 whitespace-nowrap"
          >
            {cat.name}
          </a>
        ))}
      </div>
    </div>
  );
};

export default CategoryMenu;
