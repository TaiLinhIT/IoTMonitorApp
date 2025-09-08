import publicApi from "./axiosPublic";
import privateApi from "./axiosPrivate";
import type { Product } from "../types/Product";

const productApi = {
  //public endpoints
  getAll: (): Promise<Product[]> =>{
    return publicApi.get("/Products");
  },
  getById: (id: number): Promise<Product> => {
    return publicApi.get(`/Products/${id}`);
  },

  //Private endpoints
  create:(data: Product):Promise<Product> =>{
    return privateApi.post("/Products",data);
  },
  update:(id:string, data:Partial<Product>):Promise<Product> =>{
    return privateApi.put(`/Products/${id}`, data);
  },
  delete:(id:number):Promise<void> =>{
    return privateApi.delete(`/Products/${id}`);
  },

};

export default productApi;

