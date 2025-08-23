import { CheckouDraftItem } from './CheckoutDraftItem';
interface CreateCheckoutDraftPayload {
    items: CheckouDraftItem[];
    totalPrice: number;
}