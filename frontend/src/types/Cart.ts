import type { CartItem } from "./CartItem";

export type Cart = {
  Id: number;
  UserId: string;
  Items: CartItem[];
  TotalPrice: number;
};