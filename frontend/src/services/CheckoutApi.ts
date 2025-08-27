import axiosClient from "./axiosClient";
import type { CreateCheckoutDraftPayload } from "../types/Checkout";

const checkoutApi = {
  createDraft: (payload: CreateCheckoutDraftPayload) =>
    axiosClient.post("/CheckoutDraft/create", payload),
};
export default checkoutApi;
