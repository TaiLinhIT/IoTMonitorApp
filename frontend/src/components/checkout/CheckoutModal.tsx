import React from "react";
import { X } from "lucide-react"; // icon đóng
import { motion } from "framer-motion";

type CheckoutModalProps = {
  isOpen: boolean;
  onClose: () => void;
};

const CheckoutModal: React.FC<CheckoutModalProps> = ({ isOpen, onClose }) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm">
      {/* Modal content */}
      <motion.div
        initial={{ y: 50, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        className="bg-white w-full max-w-lg rounded-2xl shadow-xl p-8 relative"
      >
        {/* Close button */}
        <button
          onClick={onClose}
          className="absolute right-4 top-4 text-gray-600 hover:text-black"
        >
          <X size={22} />
        </button>

        {/* Title */}
        <h2 className="text-2xl font-bold mb-6 text-center tracking-tight">
          Checkout
        </h2>

        {/* Form */}
        <form className="space-y-5">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Full Name
            </label>
            <input
              type="text"
              className="w-full border-b border-gray-300 focus:border-black focus:outline-none py-2"
              placeholder="Enter your name"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Email
            </label>
            <input
              type="email"
              className="w-full border-b border-gray-300 focus:border-black focus:outline-none py-2"
              placeholder="example@email.com"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Address
            </label>
            <input
              type="text"
              className="w-full border-b border-gray-300 focus:border-black focus:outline-none py-2"
              placeholder="Street, City, Country"
            />
          </div>

          <div className="flex gap-5">
            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                City
              </label>
              <input
                type="text"
                className="w-full border-b border-gray-300 focus:border-black focus:outline-none py-2"
                placeholder="Your city"
              />
            </div>
            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Zip Code
              </label>
              <input
                type="text"
                className="w-full border-b border-gray-300 focus:border-black focus:outline-none py-2"
                placeholder="00000"
              />
            </div>
          </div>

          {/* Button */}
          <button
            type="submit"
            className="w-full mt-6 bg-black text-white py-3 rounded-lg font-semibold tracking-wide hover:bg-gray-900 transition-all"
          >
            Place Order
          </button>
        </form>
      </motion.div>
    </div>
  );
};

export default CheckoutModal;
