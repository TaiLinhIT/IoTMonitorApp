// main.tsx hoặc index.tsx
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './assets/css/style.css'
import './index.css'
import { GoogleOAuthProvider } from "@react-oauth/google";
import App from './App.tsx'

// Lấy từ biến môi trường
const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID as string;

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <GoogleOAuthProvider clientId={clientId} >
      <App />
    </GoogleOAuthProvider>
  </StrictMode>,
)
