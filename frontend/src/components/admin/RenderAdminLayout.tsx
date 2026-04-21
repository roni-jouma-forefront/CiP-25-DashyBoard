import { AdminLayout } from "./layout/AdminLayout";
import { Outlet } from "react-router";

function TestRender() {
  return (
    <AdminLayout>
      <Outlet />
    </AdminLayout>
  );
}

export default TestRender;
