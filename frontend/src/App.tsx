import "./App.css";
// import { Link } from "react-router";
// import { AdminLayout } from "./components/admin/layout/AdminLayout";
// import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
// import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
// import { Outlet } from "react-router";
import { createRoot } from "react-dom/client";
import Main from "./main";

createRoot(document.getElementById("root")!).render(
  <>
    <Main />

    {/* <LocalizationProvider dateAdapter={AdapterDayjs}>
      <AdminLayout>
        <Outlet />
      </AdminLayout>
    </LocalizationProvider> */}
  </>,
);
