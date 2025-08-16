import React, { useEffect, useState } from "react";
import type { Product } from "../../models/Product";
import productApi from "../../api/ProductApi";
import "../../assets/css/Product/products.css";

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
    <div className="product-list">
      {products.map((product) => (
        <div className="product-item" key={product.Id}>
          <a href="#!">
            <img src={product.ImageUrl} alt={product.Name} className="thumb" />
          </a>
          <div className="info">
            <div className="head">
              <h3 className="title">
                <a href="#!">{product.Name}</a>
              </h3>
              <div className="rating">
                <img src="" alt="star" className="star" />
                <span className="value">{product.CategoryName}</span>
              </div>
            </div>
            <p className="desc">{product.BrandName}</p>
            <div className="foot">
              <span className="price">
                {product.Price.toLocaleString("vi-VN")}₫
              </span>
              <button className="btn book-btn">Mua Ngay</button>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default ProductList;
