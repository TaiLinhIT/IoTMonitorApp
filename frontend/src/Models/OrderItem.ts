export type OrderItem = {
    Id: number;
    OrderId: number;
    ProductId: number;
    ProductName: string;
    Quantity: number;
    UnitPrice: number;
    Discount: number;
    TotalPrice: number;
    slug: string;
    CreatedDate: Date;
    UpdatedDate: Date;
    IsDeleted: boolean;
};