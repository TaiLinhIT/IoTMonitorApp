import React, { createContext, useContext, useState } from "react";

type CartItem = {
  id: string;
  name: string;
  price: number;
  quantity: number;
  image: string;
};

type CartContextType = {
  cartItems: CartItem[];
  cartCount: number;
  addToCart: (item: CartItem) => void;
  addToCartAndOpen: (item: CartItem) => void; // 🔹 thêm
  isMiniCartOpen: boolean;
  openMiniCart: () => void;
  closeMiniCart: () => void;
};

const CartContext = createContext<CartContextType | undefined>(undefined);

export const CartProvider = ({ children }: { children: React.ReactNode }) => {
  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [isMiniCartOpen, setIsMiniCartOpen] = useState(false);

  const cartCount = cartItems.reduce((sum, item) => sum + item.quantity, 0);

  const addToCart = (item: CartItem) => {
    setCartItems((prev) => {
      const existing = prev.find((p) => p.id === item.id);
      if (existing) {
        return prev.map((p) =>
          p.id === item.id ? { ...p, quantity: p.quantity + item.quantity } : p
        );
      }
      return [...prev, item];
    });
  };

  // 🔹 vừa thêm vào giỏ, vừa mở MiniCart
  const addToCartAndOpen = (item: CartItem) => {
    addToCart(item);
    setIsMiniCartOpen(true);
  };

  const openMiniCart = () => setIsMiniCartOpen(true);
  const closeMiniCart = () => setIsMiniCartOpen(false);

  return (
    <CartContext.Provider
      value={{
        cartItems,
        cartCount,
        addToCart,
        addToCartAndOpen, // 🔹 expose
        isMiniCartOpen,
        openMiniCart,
        closeMiniCart,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart must be used within CartProvider");
  }
  return context;
};
