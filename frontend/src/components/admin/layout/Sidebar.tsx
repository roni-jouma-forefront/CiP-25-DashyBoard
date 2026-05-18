import { Box, List, ListItemButton, ListItemText } from "@mui/material";
import { NavLink, useNavigate } from "react-router";
import { ListItemIcon } from "@mui/material";
import HotelIcon from "@mui/icons-material/Hotel";
import SettingsIcon from "@mui/icons-material/Settings";
import DashboardIcon from "@mui/icons-material/Dashboard";
import LogoutIcon from "@mui/icons-material/Logout";
import { theme } from "../../../theme";
import DateRangeIcon from '@mui/icons-material/DateRange';

export const Sidebar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("auth_token");
    navigate("/login");
  };

  return (
    <Box
      sx={{
        width: 240,
        bgcolor: theme.palette.sidebar.background,
        color: theme.palette.sidebar.text,
        display: "flex",
        flexDirection: "column",
        overflow: "hidden",
      }}
    >
      <List sx={{ flex: 1 }}>
        <ListItemButton component={NavLink} to="/admin">
          <ListItemIcon>
            <DashboardIcon sx={{ color: theme.palette.sidebar.text }} />
          </ListItemIcon>
          <ListItemText primary="Dashboard" />
        </ListItemButton>
        <ListItemButton component={NavLink} to="/admin/rooms/">
          <ListItemIcon>
            <HotelIcon sx={{ color: theme.palette.sidebar.text }} />
          </ListItemIcon>
          <ListItemText primary="Rooms" />
        </ListItemButton>
        <ListItemButton component={NavLink} to="/admin/bookings/">
          <ListItemIcon>
            <DateRangeIcon sx={{ color: theme.palette.sidebar.text }} />
          </ListItemIcon>
          <ListItemText primary="Bookings" />
        </ListItemButton>
        <ListItemButton component={NavLink} to="/admin/settings">
          <ListItemIcon>
            <SettingsIcon sx={{ color: theme.palette.sidebar.text }} />
          </ListItemIcon>
          <ListItemText primary="Settings" />
        </ListItemButton>
      </List>
      <List>
        <ListItemButton onClick={handleLogout}>
          <ListItemIcon>
            <LogoutIcon sx={{ color: theme.palette.sidebar.text }} />
          </ListItemIcon>
          <ListItemText primary="Logout" />
        </ListItemButton>
      </List>
    </Box>
  );
};
