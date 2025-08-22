export type Cart = {
  Id: number;
  UserId: string;
  Items: CartItem[];
  TotalPrice: number;
};