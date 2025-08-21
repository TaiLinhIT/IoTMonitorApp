import { useState } from "react";
import "../../assets/css/Cart/Cart.css";




const Cart = () => {
  const [quantity, setQuantity] = useState(1);

  const handleIncrease = () => setQuantity((q) => q + 1);
  const handleDecrease = () =>
    setQuantity((q) => (q > 1 ? q - 1 : 1));

  return (
    <div className="main-content">
      {/* Danh sách sản phẩm */}
      <div className="item-product">
        <div className="selected">
          <input type="checkbox" className="select-all" />
          <p className="label">Chọn tất cả</p>
          <img src="/assets/icons/trash.svg" alt="Xóa tất cả" className="delete-all" />
        </div>

        <div className="product-row">
          <input type="checkbox" className="selected" />
          <img
            src="/assets/img/camera-1.jpg"
            alt="product"
            className="product"
          />
          <div className="product-info">
            <p className="product-name">Camera Insta360 Ace Pro</p>
            <p className="desc">Màu: Đen</p>
            <p className="price">11.190.000₫</p>
          </div>

          {/* Nút tăng giảm số lượng */}
          <div className="quantity">
            <button onClick={handleDecrease}>-</button>
            <span>{quantity}</span>
            <button onClick={handleIncrease}>+</button>
          </div>

          <img src="/assets/icons/trash.svg" alt="Xóa" className="delete" />
        </div>
      </div>

      {/* Thanh toán */}
      <div className="Cart-summary">
        <h3>Thông tin đơn hàng</h3>
        <div className="summary-row">
          <span>Tổng tiền</span>
          <span>11.190.000₫</span>
        </div>
        <div className="summary-row">
          <span>Tổng khuyến mãi</span>
          <span>0₫</span>
        </div>
        <div className="summary-row total">
          <span>Cần thanh toán</span>
          <span>11.190.000₫</span>
        </div>
        <button className="btn-confirm">Xác nhận đơn</button>
      </div>
    </div>
  );
};

export default Cart;
