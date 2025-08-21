import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import ProductDetailApi from "../../services/ProductDetailApi";
import type { ProductDetail as ProductDetailModel } from "../../types/ProductDetail";
import "../../assets/css/Product/productDetail.css";
import { PATHS } from "../../routes/paths";
import cartApi from "../../services/CartApi"; // 🔹 import cartApi để gọi giỏ hàng
import { useNavigate } from "react-router-dom";

const ProductDetail = () => {
  const { id } = useParams();
  const [product, setProduct] = useState<ProductDetailModel | null>(null);
  const [loading, setLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (id) {
      ProductDetailApi.getById(id)
        .then((data) => {
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

  const handleAddToCart = async () => {
    const token = localStorage.getItem("accessToken");
    if (!token) {
      alert("Bạn cần đăng nhập trước khi thêm giỏ hàng");
      navigate("/login");
      return;
    }

    try {
      await cartApi.addItem(product.Id, 1);
      alert("Đã thêm vào giỏ hàng");
    } catch (error) {
      console.error("Lỗi thêm giỏ hàng:", error);
      alert("Không thể thêm sản phẩm vào giỏ hàng");
    }
  };

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
            onClick={() => setSelectedImage(url)}
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
          <button onClick={handleAddToCart} className="btn book-btn">
            Thêm Giỏ Hàng
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductDetail;
