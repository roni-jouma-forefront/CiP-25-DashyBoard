import { StrictMode } from "react";
import "./index.css";
import AdminHome from "./pages/admin/AdminHome";
import RoomAdminPage from "./pages/admin/Rooms.tsx";
import SettingsPage from "./pages/admin/Settings";
import RoomDetailsPage from "./pages/admin/RoomDetails";
import Room from "./pages/Room";
import MirrorDndProvider from "./components/mirror/MirrorDndProvider.tsx";
import { BrowserRouter, Routes, Route } from "react-router";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import TestRender from "./components/admin/RenderAdminLayout.tsx";
import "./weather-icons.css";
import BookingsPage from "./pages/admin/Bookings.tsx";

const queryClient = new QueryClient();

const Main = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <BrowserRouter>
          <StrictMode>
            <Routes>
              <Route path="/admin" element={<TestRender />}>
                <Route index element={<AdminHome />} />
                <Route path="rooms" element={<RoomAdminPage />} />
                <Route path="rooms/:id" element={<RoomDetailsPage />} />
                <Route
                  path="rooms/:id/:roomNumber"
                  element={<RoomDetailsPage />}
                />
                <Route
                  path="rooms/:id/:roomNumber/:bookingId"
                  element={<RoomDetailsPage />}
                />
                <Route path="bookings" element={<BookingsPage />} />
                <Route path="settings" element={<SettingsPage />} />
              </Route>
              <Route path="/room/:id" element={<Room />}></Route>
              <Route path="/mirror" element={<MirrorDndProvider />}></Route>
            </Routes>
          </StrictMode>
        </BrowserRouter>
        <ReactQueryDevtools initialIsOpen={true} />
      </LocalizationProvider>
    </QueryClientProvider>
  );
};
export default Main;
