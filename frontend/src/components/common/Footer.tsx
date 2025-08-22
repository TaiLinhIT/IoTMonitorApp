const Footer = () => {
    return (
      <footer className="bg-gray-100 text-gray-700 mt-10 border-t">
        <div className="container mx-auto px-4 py-10 grid grid-cols-1 md:grid-cols-4 gap-8">
          {/* Thông tin công ty */}
          <div>
            <h4 className="font-bold mb-4">Về chúng tôi</h4>
            <ul className="space-y-2 text-sm">
              <li>Giới thiệu công ty</li>
              <li>Tuyển dụng</li>
              <li>Tin tức công nghệ</li>
              <li>Hệ thống cửa hàng</li>
            </ul>
          </div>
  
          {/* Chính sách */}
          <div>
            <h4 className="font-bold mb-4">Chính sách</h4>
            <ul className="space-y-2 text-sm">
              <li>Chính sách bảo hành</li>
              <li>Chính sách đổi trả</li>
              <li>Chính sách bảo mật</li>
              <li>Mua hàng trả góp</li>
            </ul>
          </div>
  
          {/* Hỗ trợ khách hàng */}
          <div>
            <h4 className="font-bold mb-4">Hỗ trợ khách hàng</h4>
            <ul className="space-y-2 text-sm">
              <li>Hướng dẫn mua hàng</li>
              <li>Hướng dẫn thanh toán</li>
              <li>Tra cứu đơn hàng</li>
              <li>Câu hỏi thường gặp</li>
            </ul>
          </div>
  
          {/* Đăng ký nhận tin */}
          <div>
            <h4 className="font-bold mb-4">Đăng ký nhận tin</h4>
            <p className="text-sm mb-4">
              Nhận thông tin khuyến mãi mới nhất từ MyShop.
            </p>
            <div className="flex">
              <input
                type="email"
                placeholder="Nhập email của bạn"
                className="flex-1 px-3 py-2 border rounded-l-lg outline-none"
              />
              <button className="bg-red-600 text-white px-4 py-2 rounded-r-lg hover:bg-red-700">
                Gửi
              </button>
            </div>
            <div className="mt-4">
              <p className="text-sm">Hotline: <strong>1800 1234</strong></p>
              <p className="text-sm">Email: <strong>support@myshop.com</strong></p>
            </div>
          </div>
        </div>
  
        {/* Copyright */}
        <div className="bg-gray-200 text-center py-4 text-sm">
          © 2025 MyShop. Tất cả các quyền được bảo lưu.
        </div>
      </footer>
    );
  };
  
  export default Footer;
  