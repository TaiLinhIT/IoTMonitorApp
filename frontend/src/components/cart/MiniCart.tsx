import { motion, AnimatePresence } from "framer-motion";
import { X } from "lucide-react";
import { useCart } from "../../contexts/CartContext";
import { useNavigate } from "react-router-dom";

type MiniCartProps = {
  isOpen: boolean;
  onClose: () => void;
};

const MiniCart = ({ isOpen, onClose }: MiniCartProps) => {
  const { cartItems } = useCart();
  const navigate = useNavigate();

  const handleCheckout = () => {
    onClose();
    navigate("/checkout");
  };

  const handleViewCart = () => {
    onClose();
    navigate("/carts");
  };

  return (
    <AnimatePresence>
      {isOpen && (
        <motion.div
          initial={{ opacity: 0, y: -10 }}
          animate={{ opacity: 1, y: 0 }}
          exit={{ opacity: 0, y: -10 }}
          transition={{ duration: 0.2 }}
          className="absolute right-4 top-14 w-80 bg-white shadow-lg rounded-lg z-50"
        >
          {/* Header */}
          <div className="flex justify-between items-center p-3 border-b">
            <h2 className="text-base font-semibold">Giỏ hàng</h2>
            <button onClick={onClose}>
              <X className="h-5 w-5 text-gray-600" />
            </button>
          </div>

          {/* Danh sách sản phẩm */}
          <div className="max-h-64 overflow-y-auto p-3">
            {cartItems.length === 0 ? (
              <p className="text-gray-500 text-sm">Giỏ hàng trống</p>
            ) : (
              cartItems.map((item, idx) => (
                <div
                  key={idx}
                  className="flex items-center space-x-3 mb-3 border-b pb-2"
                >
                  <img
                    src={item.image}
                    alt={item.name}
                    className="w-12 h-12 object-cover rounded"
                  />
                  <div className="flex-1">
                    <p className="font-medium text-sm">{item.name}</p>
                    <p className="text-xs text-gray-500">SL: {item.quantity}</p>
                    <p className="text-red-600 font-semibold text-sm">
                      {item.price.toLocaleString("vi-VN")}₫
                    </p>
                  </div>
                </div>
              ))
            )}
          </div>

          {/* Footer */}
          {cartItems.length > 0 && (
            <div className="p-3 border-t space-y-2">
              <button
                onClick={handleViewCart}
                className="w-full bg-gray-100 py-2 rounded text-sm hover:bg-gray-200"
              >
                Xem giỏ hàng
              </button>
              <button
                onClick={handleCheckout}
                className="w-full bg-red-600 text-white py-2 rounded text-sm hover:bg-red-700"
              >
                Thanh toán
              </button>
            </div>
          )}
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default MiniCart;
