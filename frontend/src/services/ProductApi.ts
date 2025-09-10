import { publicApiWithoutCookie } from "./axiosPublic";
import privateApi from "./axiosPrivate";
import type { Product } from "../types/Product";

const productApi = {
  // Public endpoints
  getAll: async (): Promise<Product[]> => {
    const response = await publicApiWithoutCookie.get<Product[]>("/Products");
    return response.data;
  },
  getById: async (id: string): Promise<Product> => {
    const response = await publicApiWithoutCookie.get<Product>(`/Products/${id}`);
    return response.data;
  },

  // Private endpoints (dùng FormData để hỗ trợ upload ảnh)
  create: async (data: FormData): Promise<Product> => {
    const response = await privateApi.post<Product>("/Products", data, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },
  update: async (id: string, data: FormData): Promise<Product> => {
    const response = await privateApi.put<Product>(`/Products/${id}`, data, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },
  delete: async (id: string): Promise<void> => {
    await privateApi.delete(`/Products/${id}`);
  },
};

export default productApi;
