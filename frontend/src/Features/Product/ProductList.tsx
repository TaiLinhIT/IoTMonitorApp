import React, { useEffect, useState } from "react";
import type { Product } from "../../Models/Product";
import productApi from "../../API/ProductApi";

const ProductList: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    productApi
      .getAll()
      .then((data) => {
        console.log("Sản phẩm:", data);
        setProducts(data);
      })
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Đang tải sản phẩm...</p>;

  return (
    <div className="px-8 py-6">
      <h2 className="text-2xl font-bold mb-6">Danh sách sản phẩm</h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {products.map((product) => (
          <div
            key={product.id}
            className="border rounded-lg overflow-hidden shadow-sm hover:shadow-lg transition-shadow"
          >
            <div className="aspect-[4/3] bg-gray-100">
              <img
                src={product.ImageUrl}
                alt={product.Name}
                className="w-full h-full object-cover"
              />
            </div>
            <div className="p-4">
              <p className="text-red-500 text-sm font-semibold mb-1">Just In</p>
              <h3 className="font-bold text-lg">{product.Name}</h3>
              <p className="text-gray-600 text-sm">
                {product.CategoryName} - {product.BrandName}
              </p>
              <p className="mt-2 font-semibold">
                {product.Price.toLocaleString("vi-VN")}₫
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProductList;
