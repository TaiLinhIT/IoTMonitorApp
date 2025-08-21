export type Shipment = {
  Id: number;
  OrderId: number;
  ShipmentMethod: string;
  Carrier: string;
  TrackingNumber: string;
  ShipmentDate: Date;
  EstimatedDeliveryDate: Date;
  DeliveryDate: Date;
  Status: string;
  ShippingFee: number;
  CreatedDate: Date;
  UpdatedDate: Date;
  IsDeleted: boolean;
};
