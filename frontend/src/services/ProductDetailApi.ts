import {publicApiWithoutCookie} from "./axiosPublic";
import type { ProductDetail } from "../types/ProductDetail";

const ProductDetailApi = {
  getById: async (id: string | number): Promise<ProductDetail> => {
    const response = await publicApiWithoutCookie.get(`/Products/Product/${id}`);
    return response.data;
  },
};

export default ProductDetailApi;
