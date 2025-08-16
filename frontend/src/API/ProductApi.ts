// src/api/productApi.ts
import axiosClient from "./axiosClient";
import type { Product } from "../models/Product";

const productApi = {
  getAll: (): Promise<Product[]> => {
    return axiosClient.get("/products");
  },

  getById: (id: number): Promise<Product> => {
    return axiosClient.get(`/products/${id}`);
  },

  create: (data: Product): Promise<Product> => {
    return axiosClient.post("/products", data);
  },

  update: (id: number, data: Partial<Product>): Promise<Product> => {
    return axiosClient.put(`/products/${id}`, data);
  },

  delete: (id: number): Promise<void> => {
    return axiosClient.delete(`/products/${id}`);
  },
};

export default productApi;
