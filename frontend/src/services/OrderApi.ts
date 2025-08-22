import axiosClient from "./axiosClient";
import type { Order } from "../types/Order";

const orderApi = {
  getAll: (): Promise<Order[]> => {
    return axiosClient.get("/Order");
  },

  getById: (id: number | string): Promise<Order> => {
    return axiosClient.get(`/Order/${id}`);
  },

  create: (data: Omit<Order, "id" | "createdAt" | "updatedAt">): Promise<Order> => {
    return axiosClient.post("/Order", data);
  },

  update: (id: number | string, data: Partial<Order>): Promise<Order> => {
    return axiosClient.put(`/Order/${id}`, data);
  },

  delete: (id: number | string): Promise<void> => {
    return axiosClient.delete(`/Order/${id}`);
  },
};

export default orderApi;
