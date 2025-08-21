import axiosClient from "./axiosClient";
import type { ProductDetail } from "../types/ProductDetail";

const ProductDetailApi = {
  getById: (id: string | number): Promise<ProductDetail> => {
    return axiosClient.get(`/Products/Product/${id}`);
  },
};

export default ProductDetailApi;
