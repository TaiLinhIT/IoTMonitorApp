import "../../api/ProductApi";
import "../../assets/css/Product/productDetail.css";

const ProductDetail = () => {
    return(
        <div className="main-content">
        <div className="list-img">
            <img src="/assets/img/camera-1.jpg" alt="" className="img-item" />
            <img src="/assets/img/camera-2.jpg" alt="" className="img-item" />
            <img src="/assets/img/camera-3.jpg" alt="" className="img-item" />
            <img src="#!" alt="" className="img-item" />
            <img src="#!" alt="" className="img-item" />
            <img src="#!" alt="" className="img-item" />
            <img src="#!" alt="" className="img-item" />
            <img src="#!" alt="" className="img-item" />
            <img src="#!" alt="" className="img-item" />
        </div>
        <div className="img-main">
            <img src="/assets/img/camera-1.jpg" alt="" className="img-content" />
        </div>
        <div className="content">
            <div className="head">
                <h3 className="heading"></h3>
                <p className="desc"></p>
                <span className="price"></span>
            </div>
            <div className="label">
                <h4 className="desc"></h4>
                <a href="#!" className="linkGuid"></a>
            </div>
            <div className="option">
                <p className="desc">Đen</p>
                <p className="desc">Trắng</p>
                <p className="desc">Vàng</p>
                <p className="desc">Đỏ</p>
                <p className="desc">Cam</p>
                <p className="desc">Xanh</p>
                <p className="desc">Nâu</p>
                <p className="desc">Xám</p>
                <p className="desc">Hồng</p>
                <p className="desc">Tím</p>
                <p className="desc">Bạc</p>
            </div>
            <div className="action">
                <button className="btn btn-primary">Mua Ngay</button>
                <button className="btn btn-secondary">Thêm vào giỏ hàng</button>
            </div>
            <div className="info">
                <h4 className="heading">Thông tin sản phẩm</h4>
                <p className="desc">Camera IP Wifi ngoài trời Full HD mang đến giải pháp giám sát an ninh hiệu quả, dễ dàng lắp đặt và sử dụng ngay cả với người không am hiểu công nghệ.</p>
            </div>
            <div className="reviews">
                <h4 className="heading">Đánh giá sản phẩm</h4>
                <div className="review-item">
                    <p className="reviewer-name"></p>
                    <p className="review-text"></p>
                    <span className="review-date"></span>
                </div>
                <div className="review-item">
                    <p className="reviewer-name"></p>
                    <p className="review-text"></p>
                    <span className="review-date"></span>
                </div>
            </div>
        </div>
    </div>
    );
    
};
export default ProductDetail;