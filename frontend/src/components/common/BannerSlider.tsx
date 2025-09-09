"use client";

import { Swiper, SwiperSlide } from "swiper/react";
import { Autoplay, Pagination, Navigation } from "swiper/modules";



const banners = [
  { id: 1, image: "/banners/banner1.jpg", alt: "Khuyến mãi iPhone" },
  { id: 2, image: "/banners/banner2.jpg", alt: "Laptop giảm giá" },
  { id: 3, image: "/banners/banner3.jpg", alt: "Mua sắm gia dụng" },
];

const BannerSlider = () => {
  return (
    <div className="w-full overflow-hidden rounded-2xl shadow-md">
      <Swiper
        modules={[Autoplay, Pagination, Navigation]}
        autoplay={{ delay: 4000, disableOnInteraction: false }}
        pagination={{ clickable: true }}
        navigation
        loop={true}
        className="w-full h-[300px] md:h-[400px]"
      >
        {banners.map((banner) => (
          <SwiperSlide key={banner.id}>
            <img
              src={banner.image}
              alt={banner.alt}
              className="w-full h-full object-cover"
            />
          </SwiperSlide>
        ))}
      </Swiper>
    </div>
  );
};

export default BannerSlider;
