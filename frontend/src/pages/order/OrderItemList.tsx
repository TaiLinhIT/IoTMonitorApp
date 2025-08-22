import { useEffect, useState } from "react";
import orderItemApi from "../../services/OrderItemApi";
import type { OrderItem } from "../../types/Order";

type Props = {
  orderId: number; // Truyền orderId từ cha vào
};

const OrderItemList = ({ orderId }: Props) => {
  const [items, setItems] = useState<OrderItem[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchItems = async () => {
      try {
        setLoading(true);
        const res = await orderItemApi.getAll(orderId);
        setItems(res);
      } catch (err) {
        console.error("Lỗi khi load OrderItem:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchItems();
  }, [orderId]);

  if (loading) return <p>Đang tải dữ liệu...</p>;
  if (items.length === 0) return <p>Chưa có sản phẩm nào trong đơn hàng.</p>;

  return (
    <div className="p-4 bg-white rounded shadow-md">
      <h2 className="text-lg font-semibold mb-3">Chi tiết đơn hàng #{orderId}</h2>
      <table className="w-full border-collapse">
        <thead>
          <tr className="bg-gray-200">
            <th className="p-2 border">Tên sản phẩm</th>
            <th className="p-2 border">Số lượng</th>
            <th className="p-2 border">Giá</th>
            <th className="p-2 border">Thành tiền</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <tr key={item.productId}>
              <td className="p-2 border">{item.productName}</td>
              <td className="p-2 border text-center">{item.quantity}</td>
              <td className="p-2 border text-right">{item.price.toLocaleString()} đ</td>
              <td className="p-2 border text-right">{item.total.toLocaleString()} đ</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default OrderItemList;
