// src/types/Order.ts

export interface OrderItem {
  productId: number;
  productName: string;
  quantity: number;
  price: number; // giá của 1 sản phẩm
  total: number; // quantity * price
}

export interface Order {
  id: number;
  userId: number;
  status: "pending" | "confirmed" | "shipped" | "completed" | "canceled";
  totalAmount: number;
  createdAt: string; // ISO date string
  updatedAt?: string; // có thể null
  items: OrderItem[];
}
