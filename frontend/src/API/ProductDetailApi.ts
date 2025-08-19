import axiosClient from "./axiosClient";
import type { ProductDetail } from "../models/ProductDetail";

const ProductDetailApi = {
  getById: (id: string | number): Promise<ProductDetail> => {
    return axiosClient.get(`/Products/Product/${id}`);
  },
};

export default ProductDetailApi;
