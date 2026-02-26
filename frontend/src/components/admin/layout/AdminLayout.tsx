import { Box } from "@mui/material";
import React from "react";
import { Sidebar } from "./Sidebar";
import { Topbar } from "./Topbar";

interface Props {
  children: React.ReactNode;
}

export const AdminLayout = ({ children }: Props) => {
  return (
    <Box sx={{ display: "flex", height: "100vh", flexDirection: "column"}}>
      <Topbar />
      <Box sx={{ display: "flex", flex: 1}}>
        <Sidebar />
        <Box sx={{ flex: 1, display: "flex", flexDirection: "column", p:3, gap: 3, backgroundColor: "#e9ecf2" }}>
          {children}
        </Box>
      </Box>
    </Box>
  );
};
