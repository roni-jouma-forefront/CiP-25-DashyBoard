import "./App.css";
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
