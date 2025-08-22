import Header from "../components/common/Header";
import CategoryMenu from "../components/common/CategoryMenu";
import Footer from "../components/common/Footer";
import { Outlet } from "react-router-dom";

const MainLayout = () => {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <CategoryMenu />
      <main className="flex-1 container mx-auto px-4 py-6">
        <Outlet /> {/* hiển thị trang con */}
      </main>
      <Footer />
    </div>
  );
};

export default MainLayout;
