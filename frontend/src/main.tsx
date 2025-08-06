import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './output.css'//KHÔNG import output.css trong Register.tsx trực tiếp, vì bạn cần import CSS 1 lần ở entrypoint (main), không nên ở từng component
import App from './App.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
