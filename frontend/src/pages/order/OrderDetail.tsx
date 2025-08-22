import OrderItemList from "../components/OrderItemList";

const OrderDetail = () => {
  const orderId = 101; // ví dụ fix cứng, thực tế có thể lấy từ useParams() khi dùng React Router

  return (
    <div className="max-w-3xl mx-auto mt-6">
      <h1 className="text-2xl font-bold mb-4">Chi tiết đơn hàng</h1>
      <OrderItemList orderId={orderId} />
    </div>
  );
};

export default OrderDetail;
