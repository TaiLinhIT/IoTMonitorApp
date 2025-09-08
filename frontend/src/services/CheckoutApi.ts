import privateApi from "./axiosPrivate";
import type { CreateCheckoutDraftPayload } from "../types/CreateCheckoutDraftPayload";

const checkoutApi = {
  createDraft: (payload: CreateCheckoutDraftPayload) =>
    privateApi.post("/CheckoutDraft/create", payload),
};
export default checkoutApi;
