export type Order = {
  Id: number;
  UserId: number;
  OrderCode: number;
  OrderDate: Date;
  Status: string;
  PaymentMethod: string;
  PaymentStatus: string;
  TotalAmount: number;
  ShippingAddress: string;
  ShippingFee: number;
  ShippingMethod: string;
  Note: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  IsDeleted: boolean;
};
