import "./App.css";
// import { Link } from "react-router";
import { AdminLayout } from "./components/admin/layout/AdminLayout";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { Outlet } from "react-router";

function App() {
  return (
    <>
      <h1>DashyBoard</h1>
      <Watch location="Stockholm" timeZone="UTC"></Watch>

      <nav>
        <ul>
          <li>
            <Link to="/">Admin</Link>
          </li>
          <li>
            <Link to="/room/123">Room -default</Link>
          </li>
          <li>
            <Link to="/mirror">Spegel</Link>
          </li>
        </ul>
      </nav>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <AdminLayout>
          <Outlet />
        </AdminLayout>
      </LocalizationProvider>
    </>
  );
}

export default App;
