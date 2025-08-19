import React, { useEffect, useState } from "react";
import type { Product } from "../../models/Product";
import productApi from "../../api/ProductApi";
import "../../assets/css/Product/products.css";
import { Link } from "react-router-dom";
import { PATHS } from "../../routes/paths";
import { generatePath } from "react-router-dom";

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
        <Link to={generatePath(PATHS.productDetail, { slug:product.Slug, id: product.Id })} className="product-item">
          <img src={product.UrlProduct} alt={product.Name} className="thumb" />
          <div className="info">
            <div className="head">
              <h3 className="title">{product.Name}</h3>
              <div className="rating">
                <span className="value">{product.CategoryName}</span>
              </div>
            </div>
            <p className="desc">{product.BrandName}</p>
            <div className="foot">
              <span className="price">
                {product.Price.toLocaleString("vi-VN")}₫
              </span>
            </div>
          </div>
        </Link>
      ))}
    </div>

  );
};

export default ProductList;
