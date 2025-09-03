import { useEffect, useState, useMemo, useCallback, memo } from "react";
import cartApi from "../../services/CartApi";
import orderApi from "../../services/OrderApi";
import type { Cart } from "../../types/Cart";
import type { CartItem } from "../../types/CartItem";
import TrashIcon from "/assets/icons/trash.svg";
import { motion, AnimatePresence } from "framer-motion";
import { useNavigate } from "react-router-dom";
import checkoutApi from "../../services/CheckoutApi";

/* -------------------- CheckoutModal -------------------- */
interface CheckoutModalProps {
  isOpen: boolean;
  onClose: () => void;
  onConfirm: () => void;
  totalPrice: number;
}

const CheckoutModal = ({
  isOpen,
  onClose,
  onConfirm,
  totalPrice,
}: CheckoutModalProps) => {
  if (!isOpen) return null;

  return (
    <motion.div
      className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
    >
      <motion.div
        className="bg-white rounded-2xl p-6 w-full max-w-lg shadow-lg"
        initial={{ y: 50, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        transition={{ duration: 0.3 }}
      >
        <h2 className="text-xl font-bold mb-4">Xác nhận đơn hàng</h2>

        {/* Địa chỉ nhận hàng */}
        <div className="mb-4">
          <label className="block font-medium">Địa chỉ nhận hàng</label>
          <input
            type="text"
            placeholder="Nhập địa chỉ"
            className="w-full border rounded-lg p-2 mt-1"
          />
        </div>

        {/* Phương thức thanh toán */}
        <div className="mb-4">
          <label className="block font-medium">Phương thức thanh toán</label>
          <select className="w-full border rounded-lg p-2 mt-1">
            <option>Thanh toán khi nhận hàng</option>
            <option>Chuyển khoản ngân hàng</option>
            <option>Ví MoMo</option>
          </select>
        </div>

        {/* Tổng tiền */}
        <div className="flex justify-between font-semibold text-lg border-t pt-2 mb-4">
          <span>Cần thanh toán:</span>
          <span>{totalPrice.toLocaleString("vi-VN")}₫</span>
        </div>

        {/* Action buttons */}
        <div className="flex justify-end gap-3">
          <button
            onClick={onClose}
            className="px-4 py-2 rounded-lg border border-gray-300 hover:bg-gray-100"
          >
            Hủy
          </button>
          <button
            onClick={onConfirm}
            className="px-4 py-2 rounded-lg bg-black text-white hover:bg-gray-900"
          >
            Xác nhận đặt hàng
          </button>
        </div>
      </motion.div>
    </motion.div>
  );
};

/* -------------------- CartItemRow -------------------- */
interface CartItemRowProps {
  item: CartItem;
  selected: boolean;
  onSelect: (productId: string) => void;
  onIncrease: (item: CartItem) => void;
  onDecrease: (item: CartItem) => void;
  onRemove: (productId: string) => void;
}

const CartItemRow = memo(
  ({
    item,
    selected,
    onSelect,
    onIncrease,
    onDecrease,
    onRemove,
  }: CartItemRowProps) => {
    return (
      <motion.div
        layout
        onClick={() => onSelect(item.ProductId)}
        className={`flex items-center bg-white shadow-md rounded-xl p-4 transition-colors cursor-pointer ${
          selected ? "bg-gray-100" : "hover:bg-gray-50"
        }`}
        initial={{ opacity: 0, x: 50 }}
        animate={{ opacity: 1, x: 0 }}
        exit={{ opacity: 0, x: -100 }}
        transition={{ duration: 0.3 }}
      >
        <input
          type="checkbox"
          className="w-5 h-5 mr-4 pointer-events-none"
          checked={selected}
          readOnly
        />
        <img
          src={item.ImageUrl}
          alt={item.ProductName}
          className="w-24 h-24 object-cover rounded-lg mr-4"
          loading="lazy"
        />
        <div className="flex-1">
          <p className="font-semibold text-lg">{item.ProductName}</p>
          <p className="text-red-600 font-bold mt-1">
            {item.Price.toLocaleString("vi-VN")}₫
          </p>
          <div className="flex items-center mt-2 space-x-2">
            <button
              onClick={(e) => {
                e.stopPropagation();
                onDecrease(item);
              }}
              className="w-8 h-8 bg-gray-200 rounded hover:bg-gray-300 transition-colors"
            >
              -
            </button>

            <motion.span
              key={item.Quantity}
              initial={{ scale: 0.7, opacity: 0.6 }}
              animate={{ scale: 1, opacity: 1 }}
              transition={{ duration: 0.2 }}
              className="px-3 font-semibold"
            >
              {item.Quantity}
            </motion.span>

            <button
              onClick={(e) => {
                e.stopPropagation();
                onIncrease(item);
              }}
              className="w-8 h-8 bg-gray-200 rounded hover:bg-gray-300 transition-colors"
            >
              +
            </button>
          </div>
        </div>
        <button
          onClick={(e) => {
            e.stopPropagation();
            onRemove(item.ProductId);
          }}
          className="ml-4 text-gray-400 hover:text-red-600 transition-colors"
        >
          <img src={TrashIcon} alt="Xóa" className="w-6 h-6" />
        </button>
      </motion.div>
    );
  }
);

/* -------------------- Cart -------------------- */
const Cart = () => {
  const navigate = useNavigate();
  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedItems, setSelectedItems] = useState<string[]>([]);
  const [selectAll, setSelectAll] = useState(false);
  const [isCheckingOut, setIsCheckingOut] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  // Lấy giỏ hàng
  useEffect(() => {
    const fetchCart = async () => {
      setLoading(true);
      try {
        const response: Cart = await cartApi.getCart();
        setCartItems(response.data.Items || []);
      } catch (error) {
        console.error("Lỗi lấy giỏ hàng:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchCart();
  }, []);

  const handleSelectItem = useCallback((productId: string) => {
    setSelectedItems((prev) =>
      prev.includes(productId)
        ? prev.filter((id) => id !== productId)
        : [...prev, productId]
    );
  }, []);

  const handleSelectAll = useCallback(() => {
    if (selectAll) {
      setSelectedItems([]);
    } else {
      setSelectedItems(cartItems.map((item) => item.ProductId));
    }
    setSelectAll(!selectAll);
  }, [selectAll, cartItems]);

  const handleIncrease = useCallback(async (item: CartItem) => {
    const newQuantity = item.Quantity + 1;
    try {
      await cartApi.updateItem(item.ProductId, newQuantity);
      setCartItems((prev) =>
        prev.map((p) =>
          p.ProductId === item.ProductId ? { ...p, Quantity: newQuantity } : p
        )
      );
    } catch (error) {
      console.error("Lỗi update số lượng:", error);
    }
  }, []);

  const handleDecrease = useCallback(async (item: CartItem) => {
    const newQuantity = Math.max(1, item.Quantity - 1);
    try {
      await cartApi.updateItem(item.ProductId, newQuantity);
      setCartItems((prev) =>
        prev.map((p) =>
          p.ProductId === item.ProductId ? { ...p, Quantity: newQuantity } : p
        )
      );
    } catch (error) {
      console.error("Lỗi update số lượng:", error);
    }
  }, []);

  const handleRemove = useCallback(async (productId: string) => {
    try {
      await cartApi.removeItem(productId);
      setCartItems((prev) => prev.filter((p) => p.ProductId !== productId));
      setSelectedItems((prev) => prev.filter((id) => id !== productId));
    } catch (error) {
      console.error("Lỗi xóa sản phẩm:", error);
    }
  }, []);

  const handleCheckout = async () => {
    if (selectedItems.length === 0) return;
  
    const itemsToOrder = cartItems.filter((item) =>
      selectedItems.includes(item.ProductId)
    );
  
    // Payload KHÔNG có userId
    const payload = {
      totalPrice: totalPrice,
      items: itemsToOrder.map((item) => ({
        productId: item.ProductId,
        quantity: item.Quantity,
        price: item.Price,
      })),
    };
  
    try {
      const response = await checkoutApi.createDraft(payload);
  
      navigate(`/checkout`);
    } catch (error) {
      console.error("Tạo CheckoutDraft thất bại:", error);
      alert("Không thể tiến hành thanh toán. Vui lòng thử lại.");
    }
  };
  
  // Người dùng xác nhận trong modal => gọi API
  const handleConfirmOrder = async () => {
    try {
      setIsCheckingOut(true);

      const itemsToOrder = cartItems.filter((item) =>
        selectedItems.includes(item.ProductId)
      );

      const payload = {
        userId: 1,
        items: itemsToOrder.map((item) => ({
          productId: item.ProductId,
          quantity: item.Quantity,
        })),
      };

      const order = await orderApi.create(payload);

      alert("Thanh toán thành công! Mã đơn #" + order.id);

      setCartItems((prev) =>
        prev.filter((item) => !selectedItems.includes(item.ProductId))
      );
      setSelectedItems([]);
      setSelectAll(false);
    } catch (error) {
      console.error("Lỗi khi thanh toán:", error);
      alert("Thanh toán thất bại. Vui lòng thử lại.");
    } finally {
      setIsCheckingOut(false);
      setIsModalOpen(false);
    }
  };

  const totalPrice = useMemo(() => {
    return cartItems
      .filter((item) => selectedItems.includes(item.ProductId))
      .reduce((sum, item) => sum + item.Price * item.Quantity, 0);
  }, [selectedItems, cartItems]);

  if (loading) {
    return (
      <div className="space-y-4 p-4 lg:p-10">
        {[...Array(3)].map((_, idx) => (
          <div
            key={idx}
            className="h-28 bg-gray-200 rounded-xl animate-pulse"
          ></div>
        ))}
      </div>
    );
  }

  if (!cartItems.length)
    return <p className="text-center mt-10 text-gray-500">Giỏ hàng trống</p>;

  return (
    <div className="flex flex-col lg:flex-row lg:space-x-6 p-4 lg:p-10 bg-gray-50 min-h-screen">
      {/* Danh sách sản phẩm */}
      <div className="flex-1 space-y-4">
        <div className="flex items-center mb-4">
          <input
            type="checkbox"
            className="w-5 h-5 mr-2"
            checked={selectAll}
            onChange={handleSelectAll}
          />
          <span className="font-semibold">Chọn tất cả</span>
        </div>

        <AnimatePresence mode="popLayout">
          {cartItems.map((item) => (
            <CartItemRow
              key={item.ProductId}
              item={item}
              selected={selectedItems.includes(item.ProductId)}
              onSelect={handleSelectItem}
              onIncrease={handleIncrease}
              onDecrease={handleDecrease}
              onRemove={handleRemove}
            />
          ))}
        </AnimatePresence>
      </div>

      {/* Thanh toán */}
      <motion.div
        layout
        className="lg:w-96 mt-6 lg:mt-0 bg-gradient-to-b from-white to-gray-50 shadow-2xl rounded-3xl p-8 flex flex-col space-y-6 sticky top-6"
      >
        <h3 className="text-3xl font-extrabold tracking-tight text-gray-900 flex items-center gap-2">
          🛍️ Thanh toán
        </h3>

        <div className="space-y-3 text-gray-700">
          <div className="flex justify-between">
            <span className="text-sm">
              Tổng tiền ({selectedItems.length} sản phẩm)
            </span>
            <motion.span
              key={totalPrice}
              initial={{ opacity: 0, y: 5 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.25 }}
              className="font-medium"
            >
              {totalPrice.toLocaleString("vi-VN")}₫
            </motion.span>
          </div>

          <div className="flex justify-between text-sm text-gray-500">
            <span>Khuyến mãi</span>
            <span>0₫</span>
          </div>
        </div>

        <div className="flex justify-between items-center border-t border-gray-200 pt-4">
          <span className="text-lg font-semibold">Cần thanh toán</span>
          <motion.span
            key={totalPrice + "-final"}
            initial={{ opacity: 0, y: 5 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            className="text-2xl font-bold bg-gradient-to-r from-black to-gray-800 bg-clip-text text-transparent"
          >
            {totalPrice.toLocaleString("vi-VN")}₫
          </motion.span>
        </div>

        <button
          disabled={selectedItems.length === 0 || isCheckingOut}
          onClick={handleCheckout}
          className={`w-full py-4 rounded-2xl font-bold text-lg tracking-wide transition-all duration-300 ${
            selectedItems.length > 0 && !isCheckingOut
              ? "bg-black text-white hover:scale-[1.02] hover:shadow-lg active:scale-95"
              : "bg-gray-200 text-gray-500 cursor-not-allowed"
          }`}
        >
          {isCheckingOut ? "Đang xử lý..." : "Thanh toán ngay"}
        </button>
      </motion.div>
    </div>
  );
};

export default Cart;
