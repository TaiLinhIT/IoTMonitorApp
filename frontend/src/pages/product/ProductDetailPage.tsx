import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import ProductDetailApi from "../../services/ProductDetailApi";
import type { ProductDetail as ProductDetailModel } from "../../types/ProductDetail";
import "../../assets/css/Product/productDetail.css";
import cartApi from "../../services/CartApi";
import { useCart } from "../../contexts/CartContext"; // 🔹 lấy context

const ProductDetail = () => {
  const { id } = useParams();
  const [product, setProduct] = useState<ProductDetailModel | null>(null);
  const [loading, setLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);
  const navigate = useNavigate();

  const { addToCartAndOpen } = useCart(); // 🔹 từ context

  useEffect(() => {
    if (!id) return;
    ProductDetailApi.getById(id)
      .then((response) => {

        setProduct(response.data);
        if (data?.ProductUrl?.length > 0) {
          setSelectedImage(data.ProductUrl[0]);
        }
      })
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  }, [id]);

  const handleAddToCart = async () => {
    
  
    if (!product) return;
  
    try {
      await cartApi.addItem(product.Id, 1);
  
      // 🔹 Vừa thêm giỏ hàng, vừa mở MiniCart
      addToCartAndOpen({
        id: product.Id,
        name: product.Name,
        price: product.Price,
        image: selectedImage || product.ProductUrl[0],
        quantity: 1,
      });
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
            {product?.Price?.toLocaleString("vi-VN") ?? 'Chưa có giá'}₫
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
