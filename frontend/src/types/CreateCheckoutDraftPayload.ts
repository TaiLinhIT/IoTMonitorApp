import type { CheckoutDraftItem } from './CheckoutDraftItem';
export interface CreateCheckoutDraftPayload {
    items: CheckoutDraftItem[];
    totalPrice: number;
}