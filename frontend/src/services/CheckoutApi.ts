import privateApi from "./axiosPrivate";
import type { CreateCheckoutDraftPayload } from "../types/CreateCheckoutDraftPayload";


const checkoutApi = {
  createDraft: (payload: CreateCheckoutDraftPayload) =>
    privateApi.post("/CheckoutDraft/create", payload),
  getByUserId: async (id: string ): Promise<ProductDetail> => {
    const response = await privateApi.get(`/Products/Product/${id}`);
    return response.data;
  },
};
export default checkoutApi;
