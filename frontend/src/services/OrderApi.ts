import privateApi from "./axiosPrivate";
import type { Order , OrderCreate } from "../types/Order";

const orderApi = {
  getAll: (): Promise<Order[]> => {
    return privateApi.get("/Order");
  },

  getById: (id: number | string): Promise<Order> => {
    return privateApi.get(`/Order/${id}`);
  },

  create: (data: OrderCreate): Promise<Order> => {
    return privateApi.post("/Order", data);
  },

  update: (id: number | string, data: Partial<Order>): Promise<Order> => {
    return privateApi.put(`/Order/${id}`, data);
  },

  delete: (id: number | string): Promise<void> => {
    return privateApi.delete(`/Order/${id}`);
  },
};

export default orderApi;
