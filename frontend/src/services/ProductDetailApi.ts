import publicApi from "./axiosPublic";
import type { ProductDetail } from "../types/ProductDetail";

const ProductDetailApi = {
  getById: (id: string | number): Promise<ProductDetail> => {
    return publicApi.get(`/Products/Product/${id}`);
  },
};

export default ProductDetailApi;
