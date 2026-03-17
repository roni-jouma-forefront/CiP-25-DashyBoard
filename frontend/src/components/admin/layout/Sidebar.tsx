import { Box, List, ListItemButton, ListItemText } from "@mui/material";
import { NavLink } from "react-router";
import { ListItemIcon } from "@mui/material";
import HotelIcon from "@mui/icons-material/Hotel";
import SettingsIcon from "@mui/icons-material/Settings";
import DashboardIcon from "@mui/icons-material/Dashboard";
import { theme } from "../../../theme";
import DateRangeIcon from '@mui/icons-material/DateRange';

export const Sidebar = () => {
  return (
    <Box
      sx={{
        width: 240,
        bgcolor: theme.palette.sidebar.background,
        color: theme.palette.sidebar.text,
      }}
    >
      <List>
        <ListItemButton component={NavLink} to="/admin">
          <ListItemIcon>
            <DashboardIcon sx={{ color: theme.palette.sidebar.text }} />
          </ListItemIcon>
          <ListItemText primary="Dasboard" />
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
          <ListItemText primary="Setting" />
        </ListItemButton>
      </List>
    </Box>
  );
};
