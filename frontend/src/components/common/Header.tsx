import { useState } from "react";
import { ShoppingCart, Bell, User } from "lucide-react";
import { useCart } from "../../contexts/CartContext";
import { motion, AnimatePresence } from "framer-motion";
import MiniCart from "../cart/MiniCart"; // thêm MiniCart


const Header = () => {
  const { cartCount } = useCart();
  
  const [isCartOpen, setIsCartOpen] = useState(false);


  return (
    <>
      <header className="w-full bg-red-600 text-white">
        {/* Top header */}
        <div className="container mx-auto flex items-center justify-between px-4 py-2">
          {/* Logo */}
          <div className="flex items-center space-x-2">
            <img
              src="/logo.png"
              alt="Logo"
              className="h-10 w-auto object-contain"
            />
            <span className="text-lg font-bold">MyShop</span>
          </div>

          {/* Search */}
          <div className="flex-1 mx-6">
            <div className="flex items-center bg-white rounded-lg overflow-hidden">
              <input
                type="text"
                placeholder="Nhập tên điện thoại, máy tính, phụ kiện..."
                className="flex-1 px-4 py-2 text-gray-700 outline-none"
              />
              <button className="bg-yellow-400 px-4 py-2 text-black font-semibold hover:bg-yellow-500">
                Tìm kiếm
              </button>
            </div>
          </div>

          {/* Icons */}
          <div className="flex items-center space-x-6">
          <button type="button" className="hover:text-yellow-300">
            <Bell className="h-6 w-6" />
            <span className="sr-only">Thông báo</span>
          </button>

          <button type="button" className="hover:text-yellow-300">
            <User className="h-6 w-6" />
            <span className="sr-only">Tài khoản</span>
          </button>


            {/* Cart */}
            <div className="relative">
              <button type="button"  aria-label="Giỏ hàng"  onClick={() => setIsCartOpen(true)}>
                <ShoppingCart className="h-6 w-6 cursor-pointer hover:text-yellow-300" />
              </button>

              {/* Badge động */}
              <AnimatePresence>
                {cartCount > 0 && (
                  <motion.span
                    key={cartCount}
                    initial={{ scale: 0, y: -10, opacity: 0 }}
                    animate={{ scale: 1, y: 0, opacity: 1 }}
                    exit={{ scale: 0, opacity: 0 }}
                    transition={{ type: "spring", stiffness: 500, damping: 20 }}
                    className="absolute -top-2 -right-2 bg-yellow-400 text-black text-xs font-bold px-1.5 py-0.5 rounded-full"
                  >
                    {cartCount}
                  </motion.span>
                )}
              </AnimatePresence>
            </div>
          </div>
        </div>

        {/* Menu danh mục */}
        <nav className="bg-red-700">
          <div className="container mx-auto flex items-center space-x-6 px-4 py-2 text-sm font-medium">
            <button className="hover:text-yellow-300">Danh mục</button>
            <button className="hover:text-yellow-300">iPhone 16</button>
            <button className="hover:text-yellow-300">Laptop</button>
            <button className="hover:text-yellow-300">Apple Watch</button>
            <button className="hover:text-yellow-300">iPad</button>
            <button className="hover:text-yellow-300">Samsung</button>
            <button className="hover:text-yellow-300">Máy chiếu</button>
            <button className="hover:text-yellow-300">Tủ lạnh</button>
          </div>
        </nav>
      </header>

      {/* MiniCart Drawer */}
      
      <MiniCart isOpen={isCartOpen} onClose={() => setIsCartOpen(false)} />
    </>
  );
};

export default Header;
