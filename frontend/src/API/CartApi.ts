import axiosClient from "./axiosClient";
import type { Cart } from "../models/Cart";

const cartApi = {
  getCart: (): Promise<Cart> => axiosClient.get("/cart"),
  
  addItem: (productId: string, quantity: number): Promise<Cart> =>
    axiosClient.post("/cart/add", { productId, quantity }),

  updateItem: (productId: string, quantity: number): Promise<Cart> =>
    axiosClient.put("/cart/update", { productId, quantity }),

  removeItem: (productId: string): Promise<Cart> =>
    axiosClient.delete("/cart/remove", { data: { productId } }),

  clear: (): Promise<boolean> => axiosClient.delete("/cart/clear"),
};

export default cartApi;
