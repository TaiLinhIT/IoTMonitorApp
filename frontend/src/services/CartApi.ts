import axiosClient from "./axiosClient";
import type { Cart } from "../types/Cart";

const cartApi = {
  getCart: (): Promise<Cart> => axiosClient.get("/Cart"),

  addItem: (productId: string, quantity: number): Promise<Cart> =>
    axiosClient.post("/Cart/add", { productId, quantity }),

  updateItem: (productId: string, quantity: number): Promise<Cart> =>
    axiosClient.put("/Cart/update", { productId, quantity }),

  removeItem: (productId: string): Promise<Cart> =>
    axiosClient.delete(`/Cart/remove?productId=${productId}`),
  

  clear: (): Promise<boolean> => axiosClient.delete("/cart/clear"),
};

export default cartApi;
