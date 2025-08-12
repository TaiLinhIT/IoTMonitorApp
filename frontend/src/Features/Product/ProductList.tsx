// src/features/product/ProductList.tsx
import React, { useEffect, useState } from "react";
import { Product } from "../../models/Product";
import productApi from "../../API/ProductApi";

const ProductList: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    productApi
      .getAll()
      .then((data) => setProducts(data))
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Đang tải sản phẩm...</p>;

  return (
    <div>
      <h2>Danh sách sản phẩm</h2>
      <ul>
        {products.map((p) => (
          <li key={p.Id}>
            {p.Name} - {p.Price}₫
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ProductList;
