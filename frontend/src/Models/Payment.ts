export type Payment = {
  id: number;
  orderId: number;
  PaymentMethod: string;
  Amount: number;
  PaymentDate: Date;
  TransactionId: string;
  Status: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  IsDeleted: boolean;
};
