// src/api/productApi.ts
import axiosClient from "./axiosClient";
import type { Product } from "../types/Product";

const productApi = {
  getAll: (): Promise<Product[]> => {
    return axiosClient.get("/Products");
  },

  getById: (id: number): Promise<Product> => {
    return axiosClient.get(`/Products/${id}`);
  },

  create: (data: Product): Promise<Product> => {
    return axiosClient.post("/Products", data);
  },

  update: (id: number, data: Partial<Product>): Promise<Product> => {
    return axiosClient.put(`/Products/${id}`, data);
  },

  delete: (id: number): Promise<void> => {
    return axiosClient.delete(`/Products/${id}`);
  },
};

export default productApi;
