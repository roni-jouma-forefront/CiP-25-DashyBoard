import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import Room from './pages/Room';

import { BrowserRouter, Routes, Route,} from 'react-router';


createRoot(document.getElementById('root')!).render(
  <BrowserRouter>
    <StrictMode>
      <Routes>
        <Route path="/" element={<App />}></Route>
        <Route path="/admin/room/:id"></Route>
        <Route path="/room/:id" element={<Room />}></Route>
    </Routes>
    </StrictMode>
  </BrowserRouter>
)
