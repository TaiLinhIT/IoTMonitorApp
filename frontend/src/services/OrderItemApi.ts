// src/API/OrderItemApi.ts
import axiosClient from "./axiosClient";
import type { OrderItem } from "../types/Order";

const orderItemApi = {
  // Lấy toàn bộ sản phẩm trong một đơn hàng
  getAll: (orderId: number | string): Promise<OrderItem[]> => {
    return axiosClient.get(`/orders/${orderId}/items`);
  },

  // Lấy chi tiết 1 sản phẩm trong đơn hàng
  getById: (orderId: number | string, itemId: number | string): Promise<OrderItem> => {
    return axiosClient.get(`/orders/${orderId}/items/${itemId}`);
  },

  // Thêm sản phẩm vào đơn hàng
  create: (orderId: number | string, data: Omit<OrderItem, "total">): Promise<OrderItem> => {
    return axiosClient.post(`/orders/${orderId}/items`, data);
  },

  // Cập nhật sản phẩm trong đơn hàng
  update: (
    orderId: number | string,
    itemId: number | string,
    data: Partial<OrderItem>
  ): Promise<OrderItem> => {
    return axiosClient.put(`/orders/${orderId}/items/${itemId}`, data);
  },

  // Xóa sản phẩm trong đơn hàng
  delete: (orderId: number | string, itemId: number | string): Promise<void> => {
    return axiosClient.delete(`/orders/${orderId}/items/${itemId}`);
  },
};

export default orderItemApi;
