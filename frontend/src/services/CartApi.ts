import axiosClient from "./axiosClient";
import type { Cart } from "../types/Cart";


const cartApi = {
  getCart: (): Promise<Cart> =>
    axiosClient.get("/Cart", { requiresAuth: true }),

  addItem: (productId: string, quantity: number): Promise<Cart> =>
    axiosClient.post(
      "/Cart/add",
      { productId, quantity },
      { requiresAuth: true }
    ),

  updateItem: (productId: string, quantity: number): Promise<Cart> =>
    axiosClient.put(
      "/Cart/update",
      { productId, quantity },
      { requiresAuth: true }
    ),

  removeItem: (productId: string): Promise<Cart> =>
    axiosClient.delete(`/Cart/remove?productId=${productId}`, {
      requiresAuth: true,
    }),

  clear: (): Promise<boolean> =>
    axiosClient.delete("/cart/clear", { requiresAuth: true }),
};


export default cartApi;
