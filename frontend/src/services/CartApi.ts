// import publicApi from "./axiosPublic";
import privateApi from "./axiosPrivate";
import type { Cart } from "../types/Cart";


const cartApi = {
  getCart: (): Promise<Cart> =>
    privateApi.get("/Cart"),

  addItem: (productId: string, quantity: number): Promise<Cart> =>
    privateApi.post(
      "/Cart/add",
      { productId, quantity }
    ),

  updateItem: (productId: string, quantity: number): Promise<Cart> =>
    privateApi.put(
      "/Cart/update",
      { productId, quantity }
    ),

  removeItem: (productId: string): Promise<Cart> =>
    privateApi.delete(`/Cart/remove?productId=${productId}`),

  clear: (): Promise<boolean> =>
    privateApi.delete("/cart/clear"),
};


export default cartApi;
