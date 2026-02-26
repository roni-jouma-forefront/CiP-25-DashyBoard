import "./App.css";
// import { Link } from "react-router";
import { AdminLayout } from "./components/admin/layout/AdminLayout";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { Outlet } from "react-router";

function App() {
  return (
    <>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <AdminLayout>
          <Outlet />
        </AdminLayout>
      </LocalizationProvider>
    </>
  );
}

export default App;
