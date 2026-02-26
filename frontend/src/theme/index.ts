import { createTheme } from "@mui/material/styles";
import type { RoomStatus, StatusColor } from "./types";

const roomStatusColors: Record<RoomStatus, StatusColor> = {
  available: {
    background: "#E3F2FD",
    text: "#0D47A1",
    border: "#90CAF9",
  },
  occupied: {
    background: "#FCE4EC",
    text: "#880E4F",
    border: "#F48FB1",
  },
};

export const theme = createTheme({
  palette: {
    primary: {
      light: "#5E92F3",
      main: "#1B3F8B",
      dark: "#0D2461",
      contrastText: "#FFFFFF",
    },
    secondary: {
      light: "#80D8FF",
      main: "#0288D1",
      dark: "#015F8A",
      contrastText: "#FFFFFF",
    },
    background: {
      default: "#F0F4F9",
      paper: "#FFFFFF",
    },
    text: {
      primary: "#0D1B2A",
      secondary: "#4A6080",
    },
    divider: "#C8D8E8",

    roomStatus: roomStatusColors,

    sidebar: {
      background: "#0A1929",
      text: "#B0BEC5",
      activeBackground: "#1B3F8B",
    },
  },

  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    h1: { fontWeight: 700, letterSpacing: "-0.5px" },
    h2: { fontWeight: 600 },
    h3: { fontWeight: 600 },
    button: { textTransform: "none", fontWeight: 600 },
  },

  custom: {
    spacing: {
      card: 24,
      section: 32,
      badge: "2px 10px",
    },
    borderRadius: {
      card: 12,
      badge: 20,
      button: 8,
    },
  },
});

export { type RoomStatus };
export const getRoomStatusColor = (status: RoomStatus): StatusColor =>
  roomStatusColors[status];
