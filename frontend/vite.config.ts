// vite.config.ts
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// ⚡ Cấu hình dev server proxy
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173, // FE chạy tại http://localhost:5173
    proxy: {
      "/api": {
        target: "http://localhost:5039", // BE chạy ở đây
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
