import { Box } from "@mui/material";
import React from "react";
import { Sidebar } from "./Sidebar";
import { Topbar } from "./Topbar";

interface Props {
  children: React.ReactNode;
}

export const AdminLayout = ({ children }: Props) => {
  return (
    <Box sx={{ display: "flex", height: "100vh", flexDirection: "column", overflow: "hidden" }}>
      <Topbar />
      <Box sx={{ display: "flex", flex: 1, overflow: "hidden" }}>
        <Sidebar />
        <Box sx={{ flex: 1, display: "flex", flexDirection: "column", p: 3, gap: 3, backgroundColor: "#F1F5F9", overflowY: "auto" }}>
          {children}
        </Box>
      </Box>
    </Box>
  );
};
