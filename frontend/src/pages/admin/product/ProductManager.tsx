import React, { useEffect, useState } from "react";
import type { Product } from "../../../types/Product";
import productApi from "../../../services/ProductApi";

const ProductManager: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  // Modal state
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);

  // Form state
  const [formData, setFormData] = useState({
    Name: "",
    Price: 0,
    BrandName: "",
    CategoryName: "",
    Description: "",
    ImageFile: null as File | null, // file thật
    ImagePreview: "", // base64 preview
  });

  useEffect(() => {
    productApi
      .getAll()
      .then((response) => setProducts(response))
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  const handleOpenModal = (product?: Product) => {
    if (product) {
      setEditingProduct(product);
      setFormData({
        Name: product.Name,
        Price: product.Price,
        BrandName: product.BrandName,
        CategoryName: product.CategoryName,
        Description: product.Description ?? "",
        ImageFile: null,
        ImagePreview: product.UrlProduct ?? "", // nếu backend trả về url ảnh
      });
    } else {
      setEditingProduct(null);
      setFormData({
        Name: "",
        Price: 0,
        BrandName: "",
        CategoryName: "",
        Description: "",
        ImageFile: null,
        ImagePreview: "",
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "Price" ? parseFloat(value) || 0 : value,
    }));
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      const preview = URL.createObjectURL(file);
      setFormData((prev) => ({
        ...prev,
        ImageFile: file,
        ImagePreview: preview,
      }));
    }
  };

  const handleSave = async () => {
    try {
      const data = new FormData();
      data.append("Name", formData.Name);
      data.append("Price", formData.Price.toString());
      data.append("BrandName", formData.BrandName);
      data.append("CategoryName", formData.CategoryName);
      data.append("Description", formData.Description);
      if (formData.ImageFile) {
        data.append("Image", formData.ImageFile); // backend phải nhận đúng field này
      }
  
      let savedProduct;
      if (editingProduct) {
        savedProduct = await productApi.update(editingProduct.Id, data);
        setProducts((prev) =>
          prev.map((p) => (p.Id === editingProduct.Id ? savedProduct : p))
        );
      } else {
        savedProduct = await productApi.create(data);
        setProducts((prev) => [...prev, savedProduct]);
      }
  
      handleCloseModal();
    } catch (err) {
      console.error("Lỗi khi lưu sản phẩm:", err);
    }
  };
  
  const handleDelete = async (id: string) => {
    if (window.confirm("Bạn có chắc muốn xóa sản phẩm này không?")) {
      try {
        await productApi.delete(id);
        setProducts((prev) => prev.filter((p) => p.Id !== id));
      } catch (err) {
        console.error("Lỗi khi xóa sản phẩm:", err);
      }
    }
  };
  
  return (
    <div className="p-6 bg-white rounded-2xl shadow-md">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Quản Lý Sản Phẩm</h1>
        <button
          onClick={() => handleOpenModal()}
          className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        >
          + Thêm sản phẩm
        </button>
      </div>

      <div className="overflow-x-auto">
        <table className="w-full border-collapse bg-white rounded-lg shadow-sm overflow-hidden">
          <thead className="bg-blue-600 text-white">
            <tr>
              <th className="px-4 py-3 text-left">ID</th>
              <th className="px-4 py-3 text-left">Tên Sản Phẩm</th>
              <th className="px-4 py-3 text-left">Giá</th>
              <th className="px-4 py-3 text-left">Thương Hiệu</th>
              <th className="px-4 py-3 text-left">Danh Mục</th>
              <th className="px-4 py-3 text-left">Ảnh</th>
              <th className="px-4 py-3 text-center">Hành động</th>
            </tr>
          </thead>
          <tbody>
            {products.length > 0 ? (
              products.map((product, index) => (
                <tr
                  key={product.Id}
                  className={`${
                    index % 2 === 0 ? "bg-gray-50" : "bg-white"
                  } hover:bg-gray-100 transition`}
                >
                  <td className="px-4 py-3">{product.Id}</td>
                  <td className="px-4 py-3 font-medium text-gray-800">
                    {product.Name}
                  </td>
                  <td className="px-4 py-3 text-blue-600 font-semibold">
                    {product.Price.toLocaleString("vi-VN")}₫
                  </td>
                  <td className="px-4 py-3">{product.BrandName}</td>
                  <td className="px-4 py-3">{product.CategoryName}</td>
                  <td className="px-4 py-3">
                    {product.UrlProduct ? (
                      <img
                        src={product.UrlProduct}
                        alt={product.Name}
                        className="h-12 w-12 object-cover rounded"
                      />
                    ) : (
                      <span className="text-gray-400 italic">Không có ảnh</span>
                    )}
                  </td>
                  <td className="px-4 py-3 flex gap-2 justify-center">
                    <button
                      onClick={() => handleOpenModal(product)}
                      className="px-3 py-1 text-sm bg-yellow-500 text-white rounded-lg hover:bg-yellow-600"
                    >
                      Sửa
                    </button>
                    <button 
                      onClick={() => handleDelete(product.Id)}
                      className="px-3 py-1 text-sm bg-red-500 text-white rounded-lg hover:bg-red-600">
                      Xóa
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td
                  colSpan={7}
                  className="text-center py-6 text-gray-500 italic"
                >
                  Không có sản phẩm nào.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {/* Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white p-6 rounded-xl w-full max-w-lg shadow-lg">
            <h2 className="text-xl font-bold mb-4">
              {editingProduct ? "Sửa sản phẩm" : "Thêm sản phẩm"}
            </h2>

            <div className="space-y-4">
              <div>
                <label className="block text-gray-600">Tên sản phẩm</label>
                <input
                  type="text"
                  name="Name"
                  value={formData.Name}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
              <div>
                <label className="block text-gray-600">Giá</label>
                <input
                  type="number"
                  name="Price"
                  value={formData.Price}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
              <div>
                <label className="block text-gray-600">Thương hiệu</label>
                <input
                  type="text"
                  name="BrandName"
                  value={formData.BrandName}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
              <div>
                <label className="block text-gray-600">Danh mục</label>
                <input
                  type="text"
                  name="CategoryName"
                  value={formData.CategoryName}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg"
                />
              </div>
              <div>
                <label className="block text-gray-600">Mô tả</label>
                <textarea title="Mô tả sản phẩm"
                  name="Description"
                  value={formData.Description}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg"
                />
              </div>

              {/* Upload ảnh */}
              <div>
                <label className="block text-gray-600">Ảnh sản phẩm</label>
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                  className="w-full"
                />
                {formData.ImagePreview && (
                  <img
                    src={formData.ImagePreview}
                    alt="Preview"
                    className="mt-3 w-32 h-32 object-cover rounded-lg border"
                  />
                )}
              </div>
            </div>

            <div className="flex justify-end gap-3 mt-6">
              <button
                onClick={handleCloseModal}
                className="px-4 py-2 bg-gray-300 rounded-lg hover:bg-gray-400"
              >
                Hủy
              </button>
              <button
                onClick={handleSave}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
              >
                Lưu
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductManager;
