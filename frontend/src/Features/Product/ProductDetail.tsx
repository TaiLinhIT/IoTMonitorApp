import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import ProductDetailApi from "../../api/ProductDetailApi";
import type { ProductDetail as ProductDetailModel } from "../../models/ProductDetail";
import "../../assets/css/Product/productDetail.css";
import { Link, generatePath } from "react-router-dom";
import { PATHS } from "../../routes/paths";

const ProductDetail = () => {
  const { id } = useParams();
  const [product, setProduct] = useState<ProductDetailModel | null>(null);
  const [loading, setLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      ProductDetailApi.getById(id)
        .then((data) => {
          console.log("Chi tiết sản phẩm:", data);
          setProduct(data);
          // mặc định ảnh đầu tiên làm ảnh chính
          if (data?.ProductUrl?.length > 0) {
            setSelectedImage(data.ProductUrl[0]);
          }
        })
        .catch((err) => console.error(err))
        .finally(() => setLoading(false));
    }
  }, [id]);

  if (loading) return <p>Đang tải sản phẩm...</p>;
  if (!product) return <p>Không tìm thấy sản phẩm</p>;

  return (
    <div className="main-content">
      {/* Danh sách ảnh nhỏ */}
      <div className="list-img">
        {product.ProductUrl?.map((url, idx) => (
          <img
            key={idx}
            src={url}
            alt={product.Name}
            className={`img-item ${selectedImage === url ? "active" : ""}`}
            onClick={() => setSelectedImage(url)} // khi nhấn thì đổi ảnh chính
            style={{ cursor: "pointer" }}
          />
        ))}
      </div>

      {/* Ảnh chính */}
      <div className="img-main">
        {selectedImage && (
          <img src={selectedImage} alt={product.Name} className="img-content" />
        )}
      </div>

      <div className="content">
        <div className="head">
          <h3 className="heading">{product.Name}</h3>
          <p className="desc">{product.BrandName}</p>
          <span className="price">
            {product.Price.toLocaleString("vi-VN")}₫
          </span>
        </div>

        <div className="info">
          <h4 className="heading">Thông tin sản phẩm</h4>
          <p className="desc">{product.SpecificationsName}</p>
        </div>

        <div>
          <Link to={generatePath(PATHS.checkOut, { slug:product.Slug, id: product.Id })}>
            <button className="btn book-btn">Đặt Hàng</button>
          </Link>
          <button className="btn book-btn">Thêm Giỏ Hàng</button>

        </div>
      </div>
    </div>
  );
};

export default ProductDetail;
