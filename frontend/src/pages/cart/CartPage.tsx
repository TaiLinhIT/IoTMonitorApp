import { useEffect, useState, useMemo, useCallback, memo } from "react";
import cartApi from "../../services/CartApi";
import orderApi from "../../services/OrderApi";
import type { Cart } from "../../types/Cart";
import type { CartItem } from "../../types/CartItem";
import TrashIcon from "/assets/icons/trash.svg";
import { motion, AnimatePresence } from "framer-motion";

// Row sản phẩm, memo để giảm re-render
interface CartItemRowProps {
  item: CartItem;
  selected: boolean;
  onSelect: (productId: string) => void;
  onIncrease: (item: CartItem) => void;
  onDecrease: (item: CartItem) => void;
  onRemove: (productId: string) => void;
}

const CartItemRow = memo(
  ({ item, selected, onSelect, onIncrease, onDecrease, onRemove }: CartItemRowProps) => {
    return (
      <motion.div
        layout
        onClick={() => onSelect(item.ProductId)} // click vào row => toggle select
        className={`flex items-center bg-white shadow-md rounded-xl p-4 transition-colors cursor-pointer ${
          selected ? "bg-gray-100" : "hover:bg-gray-50"
        }`}
        initial={{ opacity: 0, x: 50 }}         // hiệu ứng khi mount
        animate={{ opacity: 1, x: 0 }}          // hiệu ứng khi hiện
        exit={{ opacity: 0, x: -100 }}          // hiệu ứng khi xóa
        transition={{ duration: 0.3 }}          // tốc độ
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


const Cart = () => {
  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedItems, setSelectedItems] = useState<string[]>([]);
  const [selectAll, setSelectAll] = useState(false);
  const [isCheckingOut, setIsCheckingOut] = useState(false);
  // Lấy giỏ hàng
  useEffect(() => {
    const fetchCart = async () => {
      setLoading(true);
      try {
        const data: Cart = await cartApi.getCart();
        setCartItems(data.Items || []);
      } catch (error) {
        console.error("Lỗi lấy giỏ hàng:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchCart();
  }, []);

  // Chọn/bỏ chọn từng item
  const handleSelectItem = useCallback((productId: string) => {
    setSelectedItems((prev) =>
      prev.includes(productId) ? prev.filter((id) => id !== productId) : [...prev, productId]
    );
  }, []);

  // Chọn tất cả
  const handleSelectAll = useCallback(() => {
    if (selectAll) {
      setSelectedItems([]);
    } else {
      setSelectedItems(cartItems.map((item) => item.ProductId));
    }
    setSelectAll(!selectAll);
  }, [selectAll, cartItems]);

  // Tăng giảm số lượng
  const handleIncrease = useCallback(async (item: CartItem) => {
    const newQuantity = item.Quantity + 1;
    try {
      await cartApi.updateItem(item.ProductId, newQuantity);
      setCartItems((prev) =>
        prev.map((p) => (p.ProductId === item.ProductId ? { ...p, Quantity: newQuantity } : p))
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
        prev.map((p) => (p.ProductId === item.ProductId ? { ...p, Quantity: newQuantity } : p))
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

    try {
      setIsCheckingOut(true);

      // Lấy danh sách item được chọn
      const itemsToOrder = cartItems.filter((item) =>
        selectedItems.includes(item.ProductId)
      );

      // Chuẩn bị payload gửi API tạo Order
      const payload = {
        userId: 1, // hoặc lấy từ localStorage / context
        status: "pending",
        totalAmount: itemsToOrder.reduce(
          (sum, item) => sum + item.Price * item.Quantity,
          0
        ),
        items: itemsToOrder.map((item) => ({
          productId: Number(item.ProductId),
          productName: item.ProductName,
          quantity: item.Quantity,
          price: item.Price,
          total: item.Price * item.Quantity,
        })),
      };

      const order = await orderApi.create(payload); // gọi API tạo đơn hàng
      console.log("Đơn hàng đã tạo:", order);

      alert("Thanh toán thành công! Mã đơn #" + order.id);

      // ✅ Xóa các item đã chọn khỏi giỏ hàng
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
    }
  };

  // Tổng tiền tính nhanh với useMemo
  const totalPrice = useMemo(() => {
    return cartItems
      .filter((item) => selectedItems.includes(item.ProductId))
      .reduce((sum, item) => sum + item.Price * item.Quantity, 0);
  }, [selectedItems, cartItems]);

  if (loading) {
    return (
      <div className="space-y-4 p-4 lg:p-10">
        {[...Array(3)].map((_, idx) => (
          <div key={idx} className="h-28 bg-gray-200 rounded-xl animate-pulse"></div>
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
        className="lg:w-96 mt-6 lg:mt-0 bg-white shadow-xl rounded-2xl p-6 flex flex-col space-y-4 sticky top-4"
      >
        <h3 className="text-2xl font-bold border-b pb-2">Thông tin đơn hàng</h3>
        <div className="flex justify-between text-gray-600">
          <span>Tổng tiền ({selectedItems.length} sản phẩm)</span>
          <motion.span
            key={totalPrice}
            initial={{ opacity: 0.5, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 0.25 }}
          >
            {totalPrice.toLocaleString("vi-VN")}₫
          </motion.span>
        </div>
        <div className="flex justify-between text-gray-600">
          <span>Khuyến mãi</span>
          <span>0₫</span>
        </div>
        <div className="flex justify-between font-bold text-xl border-t pt-2">
          <span>Cần thanh toán</span>
          <motion.span
            key={totalPrice + "-final"}
            initial={{ opacity: 0.5, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 0.25 }}
          >
            {totalPrice.toLocaleString("vi-VN")}₫
          </motion.span>
        </div>
        {/* ... */}
        <button
          disabled={selectedItems.length === 0 || isCheckingOut}
          onClick={handleCheckout}
          className={`mt-4 w-full py-3 rounded-xl font-semibold text-lg transition ${
            selectedItems.length > 0 && !isCheckingOut
              ? "bg-black text-white hover:bg-gray-900"
              : "bg-gray-300 text-gray-500 cursor-not-allowed"
          }`}
        >
          {isCheckingOut ? "Đang xử lý..." : "Thanh toán"}
        </button>
      </motion.div>
    </div>
  );
};

export default Cart;
