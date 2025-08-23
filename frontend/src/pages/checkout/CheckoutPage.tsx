import { useState } from "react";

const CheckoutPage = () => {
  const [payment, setPayment] = useState("cod");
  const [shipping, setShipping] = useState("standard");

  return (
    <div className="max-w-5xl mx-auto p-6 grid grid-cols-1 lg:grid-cols-3 gap-8">
      {/* LEFT SIDE: CUSTOMER INFO */}
      <div className="lg:col-span-2 space-y-6">
        <h1 className="text-2xl font-bold mb-4">Checkout</h1>

        {/* Customer Info */}
        <div className="bg-white p-5 shadow rounded-2xl space-y-4">
          <h2 className="font-semibold text-lg">Thông tin khách hàng</h2>
          <input type="text" placeholder="Họ và tên" className="w-full border rounded p-2" />
          <input type="text" placeholder="Số điện thoại" className="w-full border rounded p-2" />
          <input type="email" placeholder="Email" className="w-full border rounded p-2" />
          <input type="text" placeholder="Địa chỉ nhận hàng" className="w-full border rounded p-2" />
          <textarea placeholder="Ghi chú đơn hàng" className="w-full border rounded p-2"></textarea>
        </div>

        {/* Payment */}
        <div className="bg-white p-5 shadow rounded-2xl space-y-3">
          <h2 className="font-semibold text-lg">Phương thức thanh toán</h2>
          <label className="flex items-center gap-3">
            <input type="radio" checked={payment === "cod"} onChange={() => setPayment("cod")} />
            Thanh toán khi nhận hàng (COD)
          </label>
          <label className="flex items-center gap-3">
            <input type="radio" checked={payment === "credit"} onChange={() => setPayment("credit")} />
            Thẻ tín dụng / Ghi nợ
          </label>
          <label className="flex items-center gap-3">
            <input type="radio" checked={payment === "momo"} onChange={() => setPayment("momo")} />
            Ví MoMo / ZaloPay
          </label>
        </div>

        {/* Shipping */}
        <div className="bg-white p-5 shadow rounded-2xl space-y-3">
          <h2 className="font-semibold text-lg">Phương thức vận chuyển</h2>
          <label className="flex items-center gap-3">
            <input type="radio" checked={shipping === "standard"} onChange={() => setShipping("standard")} />
            Giao hàng tiêu chuẩn (30k, 3-5 ngày)
          </label>
          <label className="flex items-center gap-3">
            <input type="radio" checked={shipping === "fast"} onChange={() => setShipping("fast")} />
            Giao hàng nhanh (50k, 1-2 ngày)
          </label>
          <label className="flex items-center gap-3">
            <input type="radio" checked={shipping === "express"} onChange={() => setShipping("express")} />
            Hỏa tốc (100k, trong ngày)
          </label>
        </div>
      </div>

      {/* RIGHT SIDE: ORDER SUMMARY */}
      <div className="bg-gray-50 p-5 rounded-2xl shadow space-y-4">
        <h2 className="font-semibold text-lg">Tóm tắt đơn hàng</h2>

        {/* Example item */}
        <div className="flex items-center justify-between">
          <div>
            <p className="font-medium">Nike Air Zoom Pegasus 39</p>
            <p className="text-sm text-gray-500">Size 42</p>
          </div>
          <p className="font-medium">2.500.000đ</p>
        </div>

        <div className="flex justify-between text-sm text-gray-600">
          <span>Tạm tính</span>
          <span>2.500.000đ</span>
        </div>
        <div className="flex justify-between text-sm text-gray-600">
          <span>Phí vận chuyển</span>
          <span>{shipping === "standard" ? "30.000đ" : shipping === "fast" ? "50.000đ" : "100.000đ"}</span>
        </div>
        <div className="flex justify-between font-semibold text-lg">
          <span>Tổng cộng</span>
          <span>
            {shipping === "standard" ? "2.530.000đ" : shipping === "fast" ? "2.550.000đ" : "2.600.000đ"}
          </span>
        </div>

        <button className="w-full bg-black text-white py-3 rounded-full font-semibold hover:opacity-80 transition">
          Đặt hàng
        </button>
      </div>
    </div>
  );
};

export default CheckoutPage;
