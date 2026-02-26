import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import RoomPage from "./pages/Room";
import AdminHome from "./pages/admin/AdminHome";
import RoomAdminPage from "./pages/admin/Rooms.tsx";
import SettingsPage from "./pages/admin/Settings";
import RoomDetailsPage from "./pages/admin/RoomDetails";

import { BrowserRouter, Routes, Route } from "react-router";

createRoot(document.getElementById("root")!).render(
  <BrowserRouter>
    <StrictMode>
      <Routes>
        <Route path="/admin" element={<App />}>
          <Route index element={<AdminHome />} />
          <Route path="rooms" element={<RoomAdminPage />} />
          <Route path="rooms/:id" element={<RoomDetailsPage />} />
          <Route path="settings" element={<SettingsPage />} />
        </Route>
        <Route path="/room/:id" element={<RoomPage />} />
      </Routes>
    </StrictMode>
  </BrowserRouter>,
);
