import { Box, List, ListItemButton, ListItemText } from "@mui/material";
import { NavLink } from "react-router";
import { ListItemIcon } from "@mui/material";
import HotelIcon from "@mui/icons-material/Hotel";
import SettingsIcon from "@mui/icons-material/Settings";
import DashboardIcon from "@mui/icons-material/Dashboard";

export const Sidebar = () => {
  return (
    <Box sx={{ width: 240, bgcolor: "#001540", color: "white" }}>
      <List>
        <ListItemButton component={NavLink} to="/admin">
          <ListItemIcon>
            <DashboardIcon />
          </ListItemIcon>
          <ListItemText primary="Dasboard" />
        </ListItemButton>
        <ListItemButton component={NavLink} to="/admin/rooms/">
          <ListItemIcon>
            <HotelIcon />
          </ListItemIcon>
          <ListItemText primary="Rooms" />
        </ListItemButton>
        <ListItemButton component={NavLink} to="/admin/settings">
          <ListItemIcon>
            <SettingsIcon />
          </ListItemIcon>
          <ListItemText primary="Setting" />
        </ListItemButton>
      </List>
    </Box>
  );
};
